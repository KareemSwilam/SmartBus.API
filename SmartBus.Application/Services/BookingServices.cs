using MapsterMapper;
using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.Dtos.NotificationDtos;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.Dtos.RefundRequestDtos;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.Enums;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System.ComponentModel.Design;

namespace SmartBus.Application.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IPaymentServices _paymentServices;
        private readonly IUserServices _userServices;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IMapper _mapper;
        public BookingServices(IUnitOfWork unit, IPaymentServices paymentServices,
                        IUserServices userServices, IBackgroundTaskQueue backgroundTaskQueue, IMapper mapper)
        {
            _unit = unit;
            _paymentServices = paymentServices;
            _userServices = userServices;
            _backgroundTaskQueue = backgroundTaskQueue;
            _mapper = mapper;

        }
        public async Task<CustomResult<EInvoicesResponseData>> BookingSeat(BookingRequestDto requestDto, string userId)
        {
            var TripExist = await _unit.TripRepository.GetTripWithStops(requestDto.TripId);
            if (TripExist == null)
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.NotFound("Trip You try Booking in Not Exist"));
            var LocationId = TripExist!.TripStops.Select(s => s.LocationId).ToList();
            if (!LocationId.Contains(requestDto.StartLocationId) || !LocationId.Contains(requestDto.EndLocationId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("Invalid start or end location"));
            
            var bus = await _unit.BusRepository.GetBusWithSeat(TripExist.BusId);
            var SeatsIds = bus.Seats.Select(s => s.Id).ToList();
            if (!SeatsIds.Contains(requestDto.SeatId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("InValid Seat Number"));
            
            double Price = 0;
            var FromStopOrder = TripExist.TripStops.First(ts => ts.LocationId == requestDto.StartLocationId).StopOrder;
            var ToStopOrder = TripExist.TripStops.First(ts => ts.LocationId == requestDto.EndLocationId).StopOrder;
            var ReservedSeats = await _unit.ReservedSeatRepository.GetReservedSeatsIdInTrip(requestDto.TripId, FromStopOrder, ToStopOrder);
            if(ReservedSeats.Contains(requestDto.SeatId))
                return CustomResult<EInvoicesResponseData>.Failure(CustomError.InvalidInput("Seat Already Reserved For This Segment"));
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
                Status = BookingStatus.pending,
                BookingDate = DateTime.UtcNow   

            };
            await _unit.BookingRepository.Add(booking);
            
            await _unit.SaveAsync();
            var ReservedSeat = new ReservedSeat
            {
                TripId = requestDto.TripId,
                UserId = userId,
                BookingId = booking.Id,
                StartStopOrder = FromStopOrder,
                EndStopOrder = ToStopOrder,
                SeatId = requestDto.SeatId,
                BusId = TripExist.BusId,
            };
            
            await _unit.ReservedSeatRepository.Add(ReservedSeat);
            await _unit.SaveAsync();
            var result = await GenerateEInvoice(booking, userId);
            if (result != null)
                return CustomResult<EInvoicesResponseData>.Success(result);
            return CustomResult<EInvoicesResponseData>.Failure(CustomError.ServerError("Booking Done But Fail To Generate EInvoice"));


        }
        public async Task<CustomResult<List<UserBookingDto>>> GetUserBookings(string userId)
        {
            var bookings = await _unit.BookingRepository.GetUserBookingWithTrip(userId);
            var bookingDtos = _mapper.Map<List<UserBookingDto>>(bookings);
            return CustomResult<List<UserBookingDto>>.Success(bookingDtos);
        }
        public async Task<CustomResult<UserBookingDto>> GetBooking(int id)
        {
                var booking = await _unit.BookingRepository.GetBookingWithDetails(b => b.Id == id);
                if (booking == null)
                    return CustomResult<UserBookingDto>.Failure(CustomError.NotFound("Booking Not Found"));
                var bookingDto = _mapper.Map<UserBookingDto>(booking);
                return CustomResult<UserBookingDto>.Success(bookingDto);
        }
        public async Task<CustomResult<List<UserBookingDto>>> TripBookings(Guid tripId)
        {
            var bookings = await _unit.BookingRepository.GetTripBookings(tripId);
            if (bookings == null || !bookings.Any())
                return CustomResult<List<UserBookingDto>>.Failure(CustomError.NotFound("Booking Not Found"));
            var bookingDtos = _mapper.Map<List<UserBookingDto>>(bookings);
            return CustomResult<List<UserBookingDto>>.Success(bookingDtos);
        }
        public async Task<CustomResult> CancelBooking(int bookingId, string userId) 
        {
            var bookingExist =  await _unit.BookingRepository.Get(b => b.Id == bookingId && b.UserId == userId);
            if (bookingExist == null)
                return CustomResult.Failure(CustomError.NotFound("Booking Not Found"));
            var TripExist = await _unit.TripRepository.GetTripWithStops(bookingExist.TripId);
            if (TripExist == null)
                return CustomResult.Failure(CustomError.NotFound("Trip Not Found"));
            if (bookingExist.Status == BookingStatus.cancelled)
                return CustomResult.Failure(CustomError.InvalidInput("Booking Already Cancelled"));
            if(bookingExist.Status == BookingStatus.completed)
                return CustomResult.Failure(CustomError.InvalidInput("Booking Already Completed"));
            if(bookingExist.Status == BookingStatus.confirmed)
            {
                var existingRefund = await _unit.RefundRequestRepository.Get(r => r.BookingId == bookingId);

                if (existingRefund == null)
                {
                    var refund = new RefundRequest
                    {
                        BookingId = bookingId,
                        UserId = userId,
                        CompanyId = TripExist.CompanyId,
                        InvoiceId = (long)bookingExist.InvoiceId!, 
                        RequestTime = DateTime.UtcNow,
                        Status = RefundRequestStatus.pending
                    };
                    await _unit.RefundRequestRepository.Add(refund);
                    var companyId = (await _unit.TripRepository.Get(t => t.Id == bookingExist.TripId)).CompanyId;
                    var adminUser = await _userServices.UserByCompanyId(companyId);
                    _backgroundTaskQueue.QueueNotification(new CreateNotificationDto
                    {
                        UserId = adminUser.Value!.Id,
                        TargetId = bookingExist.Id.ToString(),
                        TargetType = NotificationTargetType.Booking,
                        Message = $"There is Refund request.",

                    });
                }
            }
            bookingExist.Status = BookingStatus.cancelled;
            _unit.BookingRepository.Update(bookingExist);
            var reservedSeat = await _unit.ReservedSeatRepository.Get(r => r.TripId == bookingExist.TripId && r.BookingId == bookingId);
            if (reservedSeat != null)
            {
                _unit.ReservedSeatRepository.Delete(reservedSeat);
            }
            var complete = await _unit.SaveAsync();
            if (complete > 0)
                return CustomResult.Success();
            return CustomResult.Failure(CustomError.ServerError("Fail To Update Booking Status"));
        }
        public async Task<CustomResult> HandleCancelPaymentWebhook(CancelWebHookResponseDto responseDto)
        {
            var bookingExist = await _unit.BookingRepository.Get(b => b.InvoiceId == responseDto.TransactionId);
            if (bookingExist == null)
                return CustomResult.Failure(CustomError.NotFound("Booking Not Found"));
            var refundRequest = await _unit.RefundRequestRepository.Get(r => r.BookingId == bookingExist.Id);
            if (refundRequest == null)
                return CustomResult.Failure(CustomError.NotFound("Refund Request Not Found"));
            refundRequest.Status = RefundRequestStatus.completed;
            bookingExist.Status = BookingStatus.cancelled;
            _unit.RefundRequestRepository.Update(refundRequest);
            _unit.BookingRepository.Update(bookingExist);
            var complete = await _unit.SaveAsync();
            if (complete > 0)
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
            {
                _backgroundTaskQueue.QueueBookingTicket(bookingExist.Id);
                _backgroundTaskQueue.QueueNotification(new CreateNotificationDto
                {
                    UserId = bookingExist.UserId,
                    TargetId = bookingExist.Id.ToString(),
                    TargetType = NotificationTargetType.Booking,
                    Message = $"Your booking for trip has been confirmed.",
                    
                }); 

                var companyId = (await _unit.TripRepository.Get(t => t.Id == bookingExist.TripId)).CompanyId;
                var adminUser = await _userServices.UserByCompanyId(companyId);
                _backgroundTaskQueue.QueueNotification(new CreateNotificationDto
                {
                    UserId = adminUser.Value!.Id,
                    TargetId = bookingExist.Id.ToString(),
                    TargetType = NotificationTargetType.Booking,
                    Message = $"New Booking confirmed Trip.",

                });
                return CustomResult.Success();
            }
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
                    Success = "https://6jwt7ktz-7056.uks1.devtunnels.ms/swagger/index.html",
                    Failure = "https://6jwt7ktz-7056.uks1.devtunnels.ms/wagger/index.html"

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

        public async Task<CustomResult<List<RefundRequestDto>>> GetRefundRequest(Guid companyId)
        {
            var refundRequests = await  _unit.RefundRequestRepository.GetAll(r => r.CompanyId == companyId);
            var refundRequestDtos = _mapper.Map<List<RefundRequestDto>>(refundRequests);    
            return CustomResult<List<RefundRequestDto>>.Success(refundRequestDtos);
        }
    }
}
