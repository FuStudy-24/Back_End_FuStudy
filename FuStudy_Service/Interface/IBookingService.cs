using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetAllBooking(QueryObject queryObject);
        Task<List<BookingResponse>> GetAllStudentBookingByUserId();
        Task<List<BookingResponse>> GetAllMentorBookingByUserId();
        Task<List<BookingResponse>> GetAllBookingByMentorId(long id);
        Task<List<BookingResponse>> GetAllAcceptedBookingByMentorId(long id);
        Task<BookingResponse> CreateBooking(CreateBookingRequest request);
        Task<bool> CancelBooking(long id);
        Task<bool> AcceptBooking(long id);
        Task<bool> RejectBooking(long id);
    }
}
