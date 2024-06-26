using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tools;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Data.Entity.Core.Common.EntitySql;

namespace FuStudy_Service.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BookingResponse> CreateBooking(CreateBookingRequest request)
        {
            var userIdStr = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (!long.TryParse(userIdStr, out long userId))
            {
                throw new Exception("User ID claim invalid.");
            }

            // hàm để check role trong HttpContext
            var userRoles = _httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();

            if (!userRoles.Contains("Student"))
            {
                throw new Exception("User does not have the required role to create a booking.");
            }

            // Check if MentorId exists
            bool mentorExists = await _unitOfWork.MentorRepository.ExistsAsync(m => m.Id == request.MentorId);
            if (!mentorExists)
            {
                throw new CustomException.DataNotFoundException($"Mentor with ID {request.MentorId} does not exist.");
            }

            // Get Student by UserId
            var student = _unitOfWork.StudentRepository.Get(s => s.UserId == userId).FirstOrDefault();
            if (student == null)
            {
                throw new CustomException.DataNotFoundException("Student not found.");
            }

            // Get StudentSubscription with Subscription included
            var studentSubscription = _unitOfWork.StudentSubcriptionRepository.Get(
                p => p.StudentId == student.Id, includeProperties: "Subcription").FirstOrDefault();

            if (studentSubscription == null)
            {
                throw new CustomException.DataNotFoundException("Student subscription not found.");
            }

            // Check if currentMeeting has reached limitMeeting
            if (studentSubscription.CurrentMeeting >= studentSubscription.Subcription.LimitMeeting)
            {
                throw new Exception("You have reached the limit of meetings for your subscription.");
            }

            try
            {
                // Tạo biến mapper với model booking
                var booking = _mapper.Map<Booking>(request);

                booking.UserId = userId;
                booking.EndTime = request.StartTime.Add(request.Duration);
                booking.Status = "Pending";

                await _unitOfWork.BookingRepository.AddAsync(booking);
                await _unitOfWork.BookingRepository.SaveChangesAsync();

                // Map lại với response
                var response = _mapper.Map<BookingResponse>(booking);
                Console.WriteLine(response);
                return response;
            }
            catch (DbUpdateException ex)
            {
                // Log lỗi để có thể xem được thông tin chi tiết
                Console.WriteLine($"DbUpdateException occurred: {ex.InnerException?.Message}");

                // Bao gồm chi tiết lỗi từ inner exception
                throw new Exception($"Error occurred while saving to database. Details: {ex.InnerException?.Message}");
            }
        }

        
    }
}
