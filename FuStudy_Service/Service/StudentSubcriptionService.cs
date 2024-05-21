using AutoMapper;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Service
{
    public class StudentSubcriptionService : IStudentSubcriptionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StudentSubcriptionService (IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StudentSubcription>> GetAllStudentSubcription()
        {
            var getall = _unitOfWork.StudentSubcriptionRepository.Get(
                includeProperties: "Student,Subcription");
            return await Task.FromResult(getall);
        }
    }
}
