using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Service
{
    public class MajorService : IMajorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MajorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MajorResponse>> GetAllMajor(QueryObject queryObject)
        {
            Expression<Func<Major, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(queryObject.Search))
            {
                filter = m => m.MajorName.Contains(queryObject.Search);
            }

            var majors = _unitOfWork.MajorRepository.Get(
                filter: filter,
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize)
                .ToList();

            if (!majors.Any())
            {
                throw new CustomException.DataNotFoundException("No Major in Database");
            }

            var majorResponses = _mapper.Map<IEnumerable<MajorResponse>>(majors);

            return majorResponses;
        }

        public async Task<MajorResponse> GetMajorById(long id)
        {
            var major = _unitOfWork.MajorRepository.GetByID(id);

            if (major == null)
            {
                throw new CustomException.DataNotFoundException($"Major not found with ID: {id}");
            }

            var majorResponse = _mapper.Map<MajorResponse>(major);
            return majorResponse;
        }

        public async Task<MajorResponse> CreateMajor(MajorRequest majorRequest)
        {
            var existingMajor = _unitOfWork.MajorRepository.Get().FirstOrDefault(p => p.MajorName.ToLower() == majorRequest.MajorName.ToLower());

            if (existingMajor != null)
            {
                throw new CustomException.DataExistException($"Major with name '{majorRequest.MajorName}' already exists.");
            }
            var majorResponse = _mapper.Map<MajorResponse>(existingMajor);
            var newMajor = _mapper.Map<Major>(majorRequest);

            _unitOfWork.MajorRepository.Insert(newMajor);
            _unitOfWork.Save();

            _mapper.Map(newMajor, majorResponse);
            return majorResponse;
        }
        public async Task<MajorResponse> UpdateMajor(long id, MajorRequest majorRequest)
        {
            var existingMajor = _unitOfWork.MajorRepository.GetByID(id);

            if (existingMajor == null)
            {
                throw new CustomException.DataNotFoundException($"Major with ID {id} not found.");
            }

            var duplicateExists = await _unitOfWork.MajorRepository.ExistsAsync(p =>
                p.Id != id &&
                p.MajorName.ToLower() == majorRequest.MajorName.ToLower()
            );

            if (duplicateExists)
            {
                throw new CustomException.DataExistException($"Major with name '{majorRequest.MajorName}' already exists in Data.");
            }

            _mapper.Map(majorRequest, existingMajor);
            _unitOfWork.Save();

            var majorResponse = _mapper.Map<MajorResponse>(existingMajor);
            return majorResponse;
        }

        public async Task<bool> DeleteMajor(long id)
        {
            try
            {
                var major = _unitOfWork.MajorRepository.GetByID(id);
                if (major == null)
                {
                    throw new CustomException.DataNotFoundException("Major not found.");
                }

                _unitOfWork.MajorRepository.Delete(major);
                _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
