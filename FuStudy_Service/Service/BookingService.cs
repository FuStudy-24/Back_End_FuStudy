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
using FuStudy_Model.Enum;
using Microsoft.IdentityModel.Tokens;
using Firebase.Auth;

namespace FuStudy_Service.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationService _conversationService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConversationService conversationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _conversationService = conversationService;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBooking(QueryObject queryObject)
        {
            var bookings = _unitOfWork.BookingRepository.Get(
                includeProperties: "User,Mentor",
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize);

            if (bookings.IsNullOrEmpty())
            {
                throw new CustomException.DataNotFoundException("No Booking in Database");
            }

            var bookingResponses = _mapper.Map<IEnumerable<BookingResponse>>(bookings);

            return bookingResponses;
        }

        public async Task<List<BookingResponse>> GetAllStudentBookingByUserId()
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
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to get all self booking history.");
            }

            var bookings = _unitOfWork.BookingRepository.Get(filter: p =>
                                                        p.UserId == userId,
                                                        orderBy: q => q.OrderBy(b => b.Status),
                                                        includeProperties: "User,Mentor");

            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException($"Booking not found with UserId: {userId}");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);

            return bookingResponses;

        }

        public async Task<List<BookingResponse>> GetAllMentorBookingByUserId()
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

            if (!userRoles.Contains("Mentor"))
            {
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to get all self booking history.");
            }
            var mentor = _unitOfWork.MentorRepository.Get(m => m.UserId == userId).FirstOrDefault();

            var bookings = _unitOfWork.BookingRepository.Get(filter: p =>
                                                        p.MentorId == mentor.Id,
                                                        orderBy: q => q.OrderBy(b => b.Status),
                                                        includeProperties: "User,Mentor");

            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException($"Booking not found with MentorId: {mentor.Id}");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);

            return bookingResponses;

        }

        public async Task<List<BookingResponse>> GetAllBookingByMentorId(long id)
        {
            var bookings = _unitOfWork.BookingRepository.Get(filter: p =>
                                                        p.MentorId == id, includeProperties: "User,Mentor");

            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException($"Booking not found with MentorId: {id}");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);
            return bookingResponses;
        }

        public async Task<List<BookingResponse>> GetAllAcceptedBookingByMentorId(long id)
        {
            var bookings = _unitOfWork.BookingRepository.Get(filter: p =>
                                                        p.MentorId == id && p.Status == BookingStatus.Accepted.ToString(),
                                                        includeProperties: "User,Mentor");

            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException($"Booking not found with MentorId: {id}");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);
            return bookingResponses;
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
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to create a booking.");
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

            // Get StudentSubcription with Subscription included
            var studentSubcription = _unitOfWork.StudentSubcriptionRepository.Get(
                p => p.StudentId == student.Id, includeProperties: "Subcription").FirstOrDefault();

            if (studentSubcription == null)
            {
                throw new CustomException.DataNotFoundException("Student subscription not found.");
            }

            var existingBookings = _unitOfWork.BookingRepository.Get(
                    filter: p => p.UserId == userId &&
                    p.MentorId == request.MentorId &&
                    p.Status == BookingStatus.Accepted.ToString(),
                    includeProperties: "Mentor,User").ToList();

            foreach (var existingBooking in existingBookings)
            {
                if (existingBooking.StartTime <= request.StartTime && request.StartTime < existingBooking.EndTime)
                {
                    var existingMentor = _unitOfWork.MentorRepository.GetByID(existingBooking.MentorId);
                    var existingUser = _unitOfWork.UserRepository.GetByID(existingMentor.UserId);
                    throw new CustomException.DataExistException($"Booking with {existingUser} still exists within the requested time.");
                }
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(
                    filter: p => p.UserId == userId &&
                    p.MentorId == request.MentorId &&
                    p.Status == BookingStatus.Pending.ToString(),
                    includeProperties: "Mentor,User").FirstOrDefault();

            if (pendingBooking != null)
            {
                var pendingMentor = _unitOfWork.MentorRepository.GetByID(pendingBooking.MentorId);
                var pendingUser = _unitOfWork.UserRepository.GetByID(pendingMentor.UserId);
                throw new CustomException.DataExistException($"The Previous Booking with {pendingUser.Fullname} still Pending. Please Cancel!!!");
            }

            try
            {
                // Tạo biến mapper với model booking
                var booking = _mapper.Map<Booking>(request);
                booking.UserId = userId;
                booking.CreateAt = DateTime.Now;
                booking.StartTime = request.StartTime;

                if (booking.StartTime <= booking.CreateAt.AddHours(1))
                {
                    throw new CustomException.InvalidDataException("Start time must be at least 1 hours away from create time!");
                }

                booking.EndTime = request.StartTime.Add(request.Duration);
                booking.Status = BookingStatus.Pending.ToString();

                if (studentSubcription.CurrentMeeting == studentSubcription.Subcription.LimitMeeting)
                {
                    var bookingResponse = _mapper.Map<BookingResponse>(booking);
                    bookingResponse.Warning = "You have reached the meeting limit for your subscription. The system will use your coins to buy them.";

                    var wallet = _unitOfWork.WalletRepository.Get(w => w.UserId == userId).FirstOrDefault();
                    if (wallet.Balance <= 0)
                    {
                        throw new CustomException.InvalidDataException("Your coin is not enough to make a booking.");
                    }
                }

                await _unitOfWork.BookingRepository.AddAsync(booking);
                await _unitOfWork.BookingRepository.SaveChangesAsync();

                var finalBookingResponse = _mapper.Map<BookingResponse>(booking);

                if (studentSubcription.CurrentMeeting == studentSubcription.Subcription.LimitMeeting)
                {
                    finalBookingResponse.Warning = "You have reached the meeting limit for your subscription. The system will use your coins to buy them.";
                }

                return finalBookingResponse;
            }
            catch (DbUpdateException ex)
            {
                // Log lỗi để có thể xem được thông tin chi tiết
                Console.WriteLine($"DbUpdateException occurred: {ex.InnerException?.Message}");

                // Bao gồm chi tiết lỗi từ inner exception
                throw new Exception($"Error occurred while saving to database. Details: {ex.InnerException?.Message}");
            }
        }

        public async Task<BookingResponse> CreateBookingByCoin(CreateBookingRequest request)
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
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to create a booking.");
            }

            // Check if MentorId exists
            bool mentorExists = await _unitOfWork.MentorRepository.ExistsAsync(m => m.Id == request.MentorId);
            if (!mentorExists)
            {
                throw new CustomException.DataNotFoundException($"Mentor with ID {request.MentorId} does not exist.");
            }

            var existingBooking = _unitOfWork.BookingRepository.Get(
                    filter: p => p.Status == BookingStatus.Accepted.ToString(),
                    includeProperties: "Mentor,User").FirstOrDefault();

            if (existingBooking != null)
            {
                var existingMentor = _unitOfWork.MentorRepository.GetByID(existingBooking.MentorId);
                var existingUser = _unitOfWork.UserRepository.GetByID(existingMentor.UserId);
                throw new CustomException.DataExistException($"Booking with {existingUser} still exists within the requested time.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(
                    filter: p => p.Status == BookingStatus.Pending.ToString(),
                    includeProperties: "Mentor,User").FirstOrDefault();

            if (pendingBooking != null)
            {
                var pendingMentor = _unitOfWork.MentorRepository.GetByID(pendingBooking.MentorId);
                var pendingUser = _unitOfWork.UserRepository.GetByID(pendingMentor.UserId);
                throw new CustomException.DataExistException($"The Previous Booking with {pendingUser.Fullname} still Pending. Please Cancel!!!");
            }

            /*var existingConversation = _unitOfWork.ConversationRepository.Get(
                    filter: p => p.EndTime > DateTime.Now && p.IsClose == false, includeProperties: "User2").FirstOrDefault();

            if (existingConversation != null)
            {
                throw new CustomException.DataExistException($"The Conversation with {existingConversation.User2.Fullname} still exists within the requested time.");
            }*/

            var walet = _unitOfWork.WalletRepository.Get(w => w.UserId == userId && w.Status == true).FirstOrDefault();
            if (walet == null)
            {
                throw new CustomException.DataNotFoundException("User need to create wallet first!!!");
            }
            if (walet.Balance < 1)
            {
                throw new CustomException.InvalidDataException("Your current coins are not available. Please go to the payment page to add more coins.");
            }
            walet.Balance--;
            _unitOfWork.WalletRepository.Update(walet);

            try
            {
                // Tạo biến mapper với model booking
                var booking = _mapper.Map<Booking>(request);

                booking.UserId = userId;
                booking.CreateAt = DateTime.Now;
                booking.StartTime = request.StartTime;
                if (booking.StartTime <= booking.CreateAt.AddHours(2))
                {
                    throw new CustomException.InvalidDataException("Start time must be at least 2 hour away from create time!!!");
                }
                booking.EndTime = request.StartTime.Add(request.Duration);
                booking.Status = BookingStatus.Pending.ToString();

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

        public async Task<bool>  CancelBooking(long id)
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
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to create a booking.");
            }

            var booking = _unitOfWork.BookingRepository.GetByID(id);
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException($"Booking with id:{id} not found");
            }

            //Cancel before OverTime
            if (booking.Status.Contains("Pending"))
            {
                var student = _unitOfWork.StudentRepository.Get(s => s.UserId == userId).FirstOrDefault();
                var studentSubcription = _unitOfWork.StudentSubcriptionRepository
                    .Get(ss => ss.StudentId == student.Id)
                    .FirstOrDefault();

                studentSubcription.CurrentMeeting -= 1;
                booking.Status = BookingStatus.Canceled.ToString();

                _unitOfWork.StudentSubcriptionRepository.Update(studentSubcription);
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();
            }

            //Cancel after Mentor Accepted
            if (booking.Status.Contains("Accepted")) 
            {
                booking.Status = BookingStatus.Ended.ToString();
                var conversation = _unitOfWork.ConversationRepository
                    .Get(c => c.User1Id == userId && c.IsClose == false)
                    .FirstOrDefault();

                conversation.IsClose = true;

                _unitOfWork.ConversationRepository.Update(conversation);
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();
            }

            return true;
        }

        public async Task<bool> AcceptBooking(long id)
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

            if (!userRoles.Contains("Mentor"))
            {
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to create a booking.");
            }

            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException($"Booking with id:{id} not found");
            }

            booking.Status = BookingStatus.Accepted.ToString();
            await _unitOfWork.BookingRepository.UpdateAsync(booking);

            _conversationService.CreateConversationWithStudent(booking.UserId, booking.MentorId);

            var student = _unitOfWork.StudentRepository.Get(s => s.UserId == booking.UserId).FirstOrDefault();

            var studentSubcription = _unitOfWork.StudentSubcriptionRepository.Get(
                        filter: p => p.StudentId == student.Id, includeProperties: "Subcription").FirstOrDefault();

            if (studentSubcription.CurrentMeeting == studentSubcription.Subcription.LimitMeeting)
            {
                var wallet = _unitOfWork.WalletRepository.Get(w => w.UserId == studentSubcription.Student.UserId).FirstOrDefault();
                wallet.Balance--;
                await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            }
            else
            {
                studentSubcription.CurrentMeeting++;
                await _unitOfWork.StudentSubcriptionRepository.UpdateAsync(studentSubcription);
            }
            
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> RejectBooking(long id)
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

            if (!userRoles.Contains("Mentor"))
            {
                throw new CustomException.UnauthorizedAccessException("User does not have the required role to create a booking.");
            }

            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException($"Booking with id:{id} not found");
            }
            booking.Status = BookingStatus.Declined.ToString();
            await _unitOfWork.BookingRepository.UpdateAsync(booking);

            return true;
        }

    }
}
