﻿using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Model.Enum;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Service
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /*public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            *//*var conversation = new Conversation
            {
                User1Id = request.User1Id,
                User2Id = request.User2Id,
                CreateAt = DateTime.Now,
                EndTime = DateTime.Now + request.Duration,
                LastMessage = request.LastMessage,
                Duration = request.Duration,
                IsClose = false
            };*//*
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            var booking = _unitOfWork.BookingRepository.Get(b => b.UserId == userId && b.Status == "Confirmed");
            var conversation = _mapper.Map<Conversation>(request);
            conversation.User1Id = userId;
            conversation.CreateAt = DateTime.Now;
            conversation.IsClose = false;

            _unitOfWork.ConversationRepository.Insert(conversation);
            _unitOfWork.Save();

            var conversationResponse = _mapper.Map<ConversationResponse>(conversation);

            return conversationResponse;
        }*/
        /*public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            // Lấy user ID từ JWT token
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid);
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim không tồn tại.");
            }

            if (!long.TryParse(userIdClaim.Value, out long userId))
            {
                throw new Exception("User ID claim không hợp lệ.");
            }

            // Lấy booking
            var booking = _unitOfWork.BookingRepository.Get(b => b.UserId == userId && b.Status == "Confirmed");
            if (booking == null)
            {
                throw new Exception("Không tìm thấy booking đã được xác nhận cho người dùng này.");
            }

            // Map request thành một đối tượng Conversation
            var conversation = _mapper.Map<Conversation>(request);
            conversation.User1Id = userId;
            conversation.CreateAt = DateTime.Now;
            conversation.LastMessage = "";
            conversation.IsClose = false;

            // Chèn cuộc hội thoại mới
            _unitOfWork.ConversationRepository.Insert(conversation);
            _unitOfWork.Save();

            // Map đối tượng Conversation thành một ConversationResponse
            var conversationResponse = _mapper.Map<ConversationResponse>(conversation);

            return conversationResponse;
        }*/
        public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            var userIdStr = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (!long.TryParse(userIdStr, out long userId))
            {
                throw new Exception("User ID claim invalid.");
            }

            var existingConversation = _unitOfWork.ConversationRepository
            .Get(c => c.User1Id == userId && c.User2Id == request.User2Id).FirstOrDefault();

            if (existingConversation != null)
            {
                throw new CustomException.InternalServerErrorException("A conversation with this user already exists.");
            }

            var conversation = _mapper.Map<Conversation>(request);
            conversation.User1Id = userId;
            conversation.CreateAt = DateTime.Now;
            conversation.LastMessage = "";
            conversation.IsClose = false;

            var userId2 = _unitOfWork.UserRepository.GetByID(conversation.User2Id);

            var durationBooking = _unitOfWork.BookingRepository.Get(d => d.UserId == userId && d.MentorId == conversation.User2Id).FirstOrDefault();


            if (userId2 == null || userId2.RoleId != 4)
            {
                conversation.EndTime = DateTime.MaxValue;
            }

            if (durationBooking != null)
            {
                conversation.Duration = durationBooking.Duration;
                conversation.EndTime = conversation.CreateAt + durationBooking.Duration;
            }

            _unitOfWork.ConversationRepository.Insert(conversation);
            _unitOfWork.Save();

            var conversationResponse = _mapper.Map<ConversationResponse>(conversation);

            return conversationResponse;
        }

        public Conversation CreateConversationWithStudent(long id, long userId1, long userId2)
        {
            var conversation = new Conversation();
            conversation.User1Id = userId1;
            conversation.User2Id = userId2;
            conversation.LastMessage = "";

            var userId = _unitOfWork.UserRepository.GetByID(conversation.User2Id);

            if (userId == null || userId.RoleId != 4)
            {
                conversation.EndTime = DateTime.MaxValue;
            }

            var mentor = _unitOfWork.MentorRepository.Get(m => m.UserId == userId2).FirstOrDefault();

            var durationBooking = _unitOfWork.BookingRepository.Get(
                    d => d.Id == id &&
                    d.UserId == userId1 && 
                    d.MentorId == mentor.Id &&
                    d.Status == BookingStatus.Accepted.ToString(), includeProperties: "User").FirstOrDefault();

            if (durationBooking != null)
            {
                conversation.CreateAt = durationBooking.StartTime;
                conversation.Duration = durationBooking.Duration;
                conversation.EndTime = durationBooking.EndTime;
                conversation.IsClose = true;
            }

            _unitOfWork.ConversationRepository.Insert(conversation);
            _unitOfWork.Save();

            return conversation;
        }

        public async Task<List<ConversationResponse>> GetConversation()
        {
            var userIdStr = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            if (!long.TryParse(userIdStr, out long userId))
            {
                throw new Exception("User ID claim invalid.");
            }

            var conversation = _unitOfWork.ConversationRepository.Get(c => c.User1Id == userId || c.User2Id == userId);
            if (conversation.IsNullOrEmpty())
            {
                throw new CustomException.UnauthorizedAccessException("Conversation not found for the current user.");
            }

            var conversationResponse = _mapper.Map<List<ConversationResponse>>(conversation);
            return conversationResponse;
        }
    }
}
