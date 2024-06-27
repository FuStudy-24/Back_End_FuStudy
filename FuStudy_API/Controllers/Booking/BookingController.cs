using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Service.Interface;
using FuStudy_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tools;

namespace FuStudy_API.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAllBooking")]
        public async Task<IActionResult> GetAllBooking([FromQuery] QueryObject queryPbject)
        {
            try
            {
                var bookings = _bookingService.GetAllBooking(queryPbject);
                return CustomResult("Data Load Successfully", bookings);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllStudentBookingByUserId")]
        [Authorize]
        public async Task<IActionResult> GetAllStudentBookingByUserId()
        {
            try
            {
                var bookings = await _bookingService.GetAllStudentBookingByUserId();

                return CustomResult("Data Load Successfully", bookings);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllBookingByMentorId/{id}")]
        public async Task<IActionResult> GetAllBookingByMentorId(long id)
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingByMentorId(id);

                return CustomResult("Data Load Successfully", bookings);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost("CreateBooking")]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                BookingResponse response = await _bookingService.CreateBooking(request);
                return CustomResult("Created Successfully", response, HttpStatusCode.OK);
            }catch (CustomException.InvalidDataException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (CustomException.UnauthorizedAccessException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AcceptBooking")]
        public async Task<IActionResult> AcceptBooking(long id)
        {
            try
            {
                var booking = await _bookingService.AcceptBooking(id);
                return CustomResult("Accept Booking Susseccful", id, HttpStatusCode.OK);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("RejectBooking")]
        public async Task<IActionResult> RejectBooking(long id)
        {
            try
            {
                var booking = await _bookingService.RejectBooking(id);
                return CustomResult("Reject Booking Susseccful", id, HttpStatusCode.OK);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
