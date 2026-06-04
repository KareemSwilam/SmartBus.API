using SmartBus.Application.Dtos.BookingDtos;
using SmartBus.Application.Dtos.PaymentMethodDtos;
using SmartBus.Application.Dtos.RefundRequestDtos;
using SmartBus.Application.Result;
using System.Diagnostics.Contracts;

namespace SmartBus.Application.IServices
{
    public interface IBookingServices
    {
        public Task<CustomResult<EInvoicesResponseData>> BookingSeat(BookingRequestDto requestDto, string userId);
        public Task<CustomResult> HandlePaymentWebhook(WebHookResponseDto responseDto);
        public Task<CustomResult> HandleCancelPaymentWebhook(CancelWebHookResponseDto responseDto);
        public Task<CustomResult<List<UserBookingDto>>> GetUserBookings(string userId);
        public Task<CustomResult<UserBookingDto>> GetBooking(int id);
        public Task<CustomResult<List<UserBookingDto>>> TripBookings(Guid tripId);
        public Task<CustomResult> CancelBooking(int bookingId, string userId);
        public Task<CustomResult<List<RefundRequestDto>>> GetRefundRequest(Guid companyId);

    }
}
