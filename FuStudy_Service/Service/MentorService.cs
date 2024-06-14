using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto;
using Tools;

namespace FuStudy_Service.Service
{
    public class MentorService : IMentorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MentorService(IUnitOfWork unitOfWork, IMapper mapper,  IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<MentorResponse>> GetAllMentorVerify(QueryObject queryObject)
        {
            var mentors = _unitOfWork.MentorRepository.Get(p => p.VerifyStatus == true, includeProperties: "User,Major",
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize)
                .ToList();

            if (!mentors.Any())
            {
                throw new CustomException.DataNotFoundException("No Mentor in Database");
            }

            var mentorResponses = _mapper.Map<IEnumerable<MentorResponse>>(mentors);

            return mentorResponses;
        }
        public async Task<IEnumerable<MentorResponse>> GetAllMentorWaiting(QueryObject queryObject)
        {
            var mentors = _unitOfWork.MentorRepository.Get(p => p.VerifyStatus == false, includeProperties: "User,Major",
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize)
                .ToList();

            if (!mentors.Any())
            {
                throw new CustomException.DataNotFoundException("No Mentor in Database");
            }

            var mentorResponses = _mapper.Map<IEnumerable<MentorResponse>>(mentors);

            return mentorResponses;
        }

        public async Task<MentorResponse> GetMentorById(long id)
        {
            var mentor = _unitOfWork.MentorRepository.Get(p => p.Id == id, includeProperties: "User,Major");

            if (mentor == null)
            {
                throw new CustomException.DataNotFoundException($"Mentor not found with ID: {id}");
            }

            var mentorResponse = _mapper.Map<MentorResponse>(mentor);
            return mentorResponse;
        }

        public async Task<MentorResponse> UpdateMentor(long id, MentorRequest mentorRequest)
        {
            var mentor = _unitOfWork.MentorRepository.GetByID(id);

            if (mentor == null)
            {
                throw new CustomException.InvalidDataException($"Mentor with ID: {id} not found!");
            }
            _mapper.Map(mentorRequest, mentor);
            _unitOfWork.MentorRepository.Update(mentor);
            return _mapper.Map<MentorResponse>(mentor);
        }

        public async Task<MentorResponse> UpdateMentorLoggingIn(MentorRequest mentorRequest)
        { 
            var userId = long.Parse(Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext));
            var mentor = _unitOfWork.MentorRepository.GetByID(userId);

            if (mentor == null)
            {
                throw new CustomException.InvalidDataException("This user is not a mentor!!");
            }
            _mapper.Map(mentorRequest, mentor);
            _unitOfWork.MentorRepository.Update(mentor);
            return _mapper.Map<MentorResponse>(mentor);
        }

        public Task<bool> DeleteMentor(long id)
        {
            throw new NotImplementedException();
        }
    }
}
