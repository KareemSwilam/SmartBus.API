using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SmartBus.Application.Dtos.TicketDtos;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using SkiaSharp;
using Document = QuestPDF.Fluent.Document;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;

namespace SmartBus.Application.Services
{
    public class TicketServices : ITicketServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendingEmailService _emailService;
        private readonly IUserServices _userServices;
        public TicketServices(IUnitOfWork unitOfWork, ISendingEmailService emailService, IUserServices userServices)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userServices = userServices;
        }
        public async Task GenerateAndSendTicketAsync(int bookingId)
        {
            Console.WriteLine($"Generating ticket for booking ID: {bookingId}");
            var booking = await _unitOfWork.BookingRepository.Get(b => b.Id == bookingId);
            var Trip = await _unitOfWork.TripRepository.GetWithDetails(booking.TripId);
            var User = await _userServices.User(booking.UserId);
            
            var ticketInfo = new GenerateTicketInfoDto
            {
                UserName = User.Value.UserName,
                Email = User.Value.Email,
                CompanyName = Trip.Company.Name,
                BusNumber = Trip.Bus.BusNumber, 
                StartLocation = Trip.StartLocation.Name,
                Endlocation = Trip.EndLocation.Name,
                DepartureTime = Trip.DepartureTime,
                ArrivalTime = Trip.ArrivalTime,
                PaymentAmount = booking.Price,
               
                SeatNumber = booking.SeatNumber
            };
            var qr = GenerateQrCode(booking.Id);    
            var pdfBytes = GeneratePdf(ticketInfo, qr);
                await _emailService.SendingEmailWithAttachment(
                    User.Value.Email,
                    "Your Bus Ticket",
                    "Please find your bus ticket attached.",
                    pdfBytes,
                    $"BusTicket_{booking.Id}.pdf"
                );
            
        }
        public async Task<CustomResult<string>> ValidateTicket(int bookingId)
        {
            var bookingExist = await _unitOfWork.BookingRepository.Get(b => b.Id == bookingId);
            if(bookingExist == null) 
                return CustomResult<string>.Failure(CustomError.NotFound("Booking Not Found"));
            if (bookingExist.Status == BookingStatus.confirmed)
                return CustomResult<string>.Success("Valid Ticket");
            return CustomResult<string>.Failure(CustomError.InvalidInput("Not Valid Ticket"));

        }
        private byte[] GeneratePdf(
                 GenerateTicketInfoDto ticket,
             byte[] qrCodeImage)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Text(ticket.CompanyName)
                                .FontSize(24)
                                .Bold();

                            column.Item()
                                .AlignCenter()
                                .Text("Electronic Bus Ticket")
                                .FontSize(16);

                            column.Item()
                                .PaddingTop(5)
                                .AlignCenter()
                                .Text($"Generated: {DateTime.Now:dd MMM yyyy HH:mm}");
                        });

                    page.Content()
                        .PaddingVertical(20)
                        .Column(column =>
                        {
                            // Passenger Section
                            column.Item()
                                .Border(1)
                                .Padding(10)
                                .Column(section =>
                                {
                                    section.Item()
                                        .Text("Passenger Information")
                                        .Bold()
                                        .FontSize(16);

                                    section.Item().Text($"Name: {ticket.UserName}");
                                    section.Item().Text($"Email: {ticket.Email}");
                                });

                            column.Item().PaddingVertical(10);

                            // Journey Section
                            column.Item()
                                .Border(1)
                                .Padding(10)
                                .Column(section =>
                                {
                                    section.Item()
                                        .Text("Journey Information")
                                        .Bold()
                                        .FontSize(16);

                                    section.Item().Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Text($"From: {ticket.StartLocation}");

                                        row.RelativeItem()
                                            .Text($"To: {ticket.Endlocation}");
                                    });

                                    section.Item().Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Text($"Departure: {ticket.DepartureTime:dd MMM yyyy HH:mm}");

                                        row.RelativeItem()
                                            .Text($"Arrival: {ticket.ArrivalTime:dd MMM yyyy HH:mm}");
                                    });

                                    section.Item().Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Text($"Bus Number: {ticket.BusNumber}");

                                        row.RelativeItem()
                                            .Text($"Seat Number: {ticket.SeatNumber}");
                                    });
                                });

                            column.Item().PaddingVertical(10);

                            // Payment Section
                            column.Item()
                                .Border(1)
                                .Padding(10)
                                .Column(section =>
                                {
                                    section.Item()
                                        .Text("Payment Information")
                                        .Bold()
                                        .FontSize(16);

                                    section.Item()
                                        .Text($"Amount Paid: {ticket.PaymentAmount:N2} EGP");
                                });

                            column.Item().PaddingVertical(20);

                            // QR Section
                            column.Item()
                                .AlignCenter()
                                .Column(section =>
                                {
                                    section.Item()
                                        .Text("Ticket Validation")
                                        .Bold()
                                        .FontSize(16);

                                    section.Item()
                                        .PaddingTop(10)
                                        .Width(180)
                                        .Height(180)
                                        .Image(qrCodeImage);

                                    section.Item()
                                        .PaddingTop(10)
                                        .Text("Scan this QR code to validate the ticket.")
                                        .AlignCenter();
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Thank you for choosing ");
                            text.Span(ticket.CompanyName).Bold();
                        });
                });
            })
             .GeneratePdf();
        }
        private byte[] GenerateQrCode(int bookingReference)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 1
                }
            };

            var pixelData = writer.Write(bookingReference.ToString());

            using var bitmap = new SKBitmap(
                pixelData.Width,
                pixelData.Height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

            System.Runtime.InteropServices.Marshal.Copy(
                pixelData.Pixels,
                0,
                bitmap.GetPixels(),
                pixelData.Pixels.Length);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray(); ;
        }

        
    }
}
