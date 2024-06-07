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
using Tools;

namespace FuStudy_Service.Service
{
    public class MentorService : IMentorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MentorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public Task<MentorResponse> UpdateMentor(long id, MentorRequest mentorRequest)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteMentor(long id)
        {
            throw new NotImplementedException();
        }
    }
}
