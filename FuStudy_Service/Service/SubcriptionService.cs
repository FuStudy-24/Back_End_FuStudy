using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Service
{
    public class SubcriptionService : ISubcriptionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubcriptionService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
        }

        public async Task<IEnumerable<Subcription>> GetAllSubcriptions()
        {
            var subcriptions = _unitOfWork.SubcriptionRepository.Get();
            return subcriptions;
        }

        public async Task<Subcription> GetSubCriptionById(long id)
        {
            return await _unitOfWork.SubcriptionRepository.GetByIdAsync(id);
        }

        public async Task<SubcriptionResponse> CreateSubcription(CreateSubcriptionRequest subcriptionRequest)
        {
            var subcriptions = _mapper.Map<Subcription>(subcriptionRequest);

            // set trạng thái luôn true
            subcriptions.Status = true;
            await _unitOfWork.SubcriptionRepository.AddAsync(subcriptions);

            //map lại với cái response 
            SubcriptionResponse subcriptionResponse = _mapper.Map<SubcriptionResponse>(subcriptions);
            return subcriptionResponse;
        }

        public async Task<SubcriptionResponse> UpdateSubcription(long id, UpdateSubcriptionRequest updateSubcriptionRequest)
        {
                var existsubcription = _unitOfWork.SubcriptionRepository.GetByID(id);
                if (existsubcription == null)
                {
                    throw new Exception("Subcription ID is not exist");
                }
                //map với cái biến đang có giá trị id
                _mapper.Map(updateSubcriptionRequest, existsubcription);

                _unitOfWork.SubcriptionRepository.Update(existsubcription);
                _unitOfWork.Save();
            var subcriptionresponse = _mapper.Map<SubcriptionResponse>(existsubcription);
            return subcriptionresponse;
        }

        public async Task<SubcriptionResponse> DeleteSubcription(long id)
        {
            var deletesubcription = _unitOfWork.SubcriptionRepository.GetByID(id);
            if(deletesubcription == null)
            {
                throw new Exception("Subcription ID is not exist");
            }
            
            deletesubcription.Status = false;
            _unitOfWork.SubcriptionRepository.Update(deletesubcription);
            _unitOfWork.Save();

            //map vào giá trị response
            var subcriptionresponse = _mapper.Map<SubcriptionResponse>(deletesubcription);
            return subcriptionresponse;
        }
    }
}
