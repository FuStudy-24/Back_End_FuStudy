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
        Task<List<BookingResponse>> GetAllBooking(QueryObject queryObject);
        Task<List<BookingResponse>> GetAllStudentBookingByUserId();
        Task<List<BookingResponse>> GetAllBookingByMentorId(long id);
        Task<BookingResponse> CreateBooking(CreateBookingRequest request);
        Task<bool> AcceptBooking(long id);
        Task<bool> RejectBooking(long id);
    }
}
