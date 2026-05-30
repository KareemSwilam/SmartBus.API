using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;

namespace SmartBus.Application.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IPaymentServices _paymentServices;
        private readonly IUserServices _userServices;
        public BookingServices(IUnitOfWork unit, IPaymentServices paymentServices, IUserServices userServices)
        {
            _unit = unit;
            _paymentServices = paymentServices;
            _userServices = userServices;
        }
        public async Task<CustomResult<EInvoicesResponseData>> BookingSeat(BookingRequestDto requestDto, string userId)
        {
            var TripExist = await _unit.TripRepository.GetTripWithStops(requestDto.TripId);
            if (TripExist == null)
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.NotFound("Trip You try Booking in Not Exist"));
            var LocationId = TripExist!.TripStops.Select(s => s.LocationId).ToList();
            if (!LocationId.Contains(requestDto.StartLocationId) || !LocationId.Contains(requestDto.EndLocationId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("Invalid start or end location"));
            if (!(TripExist.BusId == requestDto.BusId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("This bus Not in This trip"));
            var bus = await _unit.BusRepository.GetBusWithSeat(requestDto.BusId);
            var SeatsIds = bus.Seats.Select(s => s.Id).ToList();
            if (!SeatsIds.Contains(requestDto.SeatId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("InValid Seat Number"));
            double Price = 0;
            var FromStopOrder = TripExist.TripStops.First(ts => ts.LocationId == requestDto.StartLocationId).StopOrder;
            var ToStopOrder = TripExist.TripStops.First(ts => ts.LocationId == requestDto.EndLocationId).StopOrder;
            var endStopOrder = TripExist.TripStops.First(ts => ts.IsEndStop).StopOrder;
            if (FromStopOrder == 1 && ToStopOrder == endStopOrder)
                Price = TripExist.Price;
            else
                Price = await _unit.StopSegmentRepository.GetPriceForSegment(requestDto.TripId, FromStopOrder, ToStopOrder);
            var booking = new Booking
            {
                TripId = requestDto.TripId,
                StartLocationId = requestDto.StartLocationId,
                EndLocationId = requestDto.EndLocationId,
                SeatNumber = bus.Seats.First(s => s.Id == requestDto.SeatId).SeatNumber,
                UserId = userId,
                Price = Price,
                Status = BookingStatus.pending

            };
            var ReservedSeat = new ReservedSeat
            {
                TripId = requestDto.TripId,
                StartStopOrder = FromStopOrder,
                EndStopOrder = ToStopOrder,
                SeatId = requestDto.SeatId,
                BusId = requestDto.BusId,
            };
            await _unit.BookingRepository.Add(booking);
            await _unit.ReservedSeatRepository.Add(ReservedSeat);
            await _unit.SaveAsync();
            var result = await GenerateEInvoice(booking, userId);
            if (result != null)
                return CustomResult<EInvoicesResponseData>.Success(result);
            return CustomResult<EInvoicesResponseData>.Failure(CustomError.ServerError("Booking Done But Fail To Generate EInvoice"));


        }

        public async Task<CustomResult> HandleCancelPaymentWebhook(CancelWebHookResponseDto responseDto)
        {
            var bookingExist = await _unit.BookingRepository.Get(b => b.PaymentReferenceNumber == responseDto.TransactionId);
            if (bookingExist == null)
                return CustomResult.Failure(CustomError.NotFound("Booking Not Found"));
            bookingExist.Status = BookingStatus.cancelled;
            _unit.BookingRepository.Update(bookingExist);
            var complete = await _unit.SaveAsync();
            if (complete == 1)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.ServerError("Fail To Update Booking Status"));
        }

        public async Task<CustomResult> HandlePaymentWebhook(WebHookResponseDto responseDto)
        {
            var bookingExist = await _unit.BookingRepository.Get(b => b.Id == responseDto.Payload.BookingId);
            if (bookingExist == null)
                return CustomResult.Failure(CustomError.NotFound("Booking Not Found"));
            if (responseDto.InvoiceStatus == "paid")
                bookingExist.Status = BookingStatus.confirmed;
            bookingExist.InvoiceId = responseDto.InvoiceId;
            bookingExist.PaymentReferenceNumber = responseDto.ReferenceNumber;
            _unit.BookingRepository.Update(bookingExist);
            var complete = await _unit.SaveAsync();
            if (complete == 1)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.ServerError("Fail To Update Booking Status"));

        }

        private async Task<EInvoicesResponseData> GenerateEInvoice(Booking booking, string userId)
        {
            var UserExist = await _userServices.User(userId);
            var eInvoice = new EInvoicesRequest()
            {
                Customer = new CustomerModel
                {
                    FirstName = UserExist.Value!.UserName,
                    email = UserExist.Value.Email,
                    Phone = UserExist.Value.PhoneNumber,
                    CustomerId = UserExist.Value.Id
                },
                Items = new List<CartItemModel>
                {
                    new CartItemModel
                    {
                        Name = $"Booking for Trip {booking.TripId}",
                        Quantity = 1,
                        Price = (decimal)booking.Price
                    }
                },
                Payload = new BookingPayload
                {
                    BookingId = booking.Id,
                    TripId = booking.TripId,
                    SeatNumber = booking.SeatNumber,

                },
                Currency = "EGP",
                SendEmail = false,
                RedirectionUrls = new RedirectionUrls
                {
                    Success = "https://5k5w970b-7056.uks1.devtunnels.ms/swagger/index.html",
                    Failure = "https://5k5w970b-7056.uks1.devtunnels.ms/swagger/index.html"

                }


            };
            var eInvoiceResponse = await _paymentServices.CreateEInvoice(eInvoice);
            if (eInvoiceResponse.IsSuccess)
            {
                return eInvoiceResponse.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
