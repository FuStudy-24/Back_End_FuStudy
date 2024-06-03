using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Service
{
    public class StudentSubcriptionService : IStudentSubcriptionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StudentSubcriptionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StudentSubcription>> GetAllStudentSubcription()
        {
            var getall = _unitOfWork.StudentSubcriptionRepository.Get(
                filter:p => p.Status == true, includeProperties: "Student,Subcription");
            return await Task.FromResult(getall);
        }

        public async Task<StudentSubcriptionResponse> GetStudentSubcriptionByID(long id)
        {

            var studentsubcriptionByID = _unitOfWork.StudentSubcriptionRepository.Get(
            filter: p => p.Id == id && p.Status == true, includeProperties: "Student,Subcription").FirstOrDefault();
            if (studentsubcriptionByID == null)
            {
                throw new CustomException.DataNotFoundException("ID Is not Found.");
            }
            var studentsubcriptionRS = _mapper.Map<StudentSubcriptionResponse>(studentsubcriptionByID);
            return await Task.FromResult(studentsubcriptionRS);
        }

        public async Task<StudentSubcriptionResponse> CreateStudentSubcription(CreateStudentSubcriptionRequest studentSubcriptionRequest)
        {
            var studentsub = _mapper.Map<StudentSubcription>(studentSubcriptionRequest);

            // Set trạng thái (limt cứng 20, câu đầu là 0)
            if (studentsub.CurrentQuestion == 20)
            {
                throw new CustomException.DataNotFoundException("You Subcription has been không xài được địt mẹ mày.");
            }
            else
            {
                //studentsub.LimitQuestion = 20;
                studentsub.CurrentQuestion = 0;
                studentsub.StartDate = DateTime.Now;
                studentsub.EndDate = DateTime.Now.AddMonths(2);
                studentsub.Status = false;

                await _unitOfWork.StudentSubcriptionRepository.AddAsync(studentsub);
            }

            //map request vào response
            StudentSubcriptionResponse studentSubcriptionResponse = _mapper.Map<StudentSubcriptionResponse>(studentsub);
            return studentSubcriptionResponse;
        }

        public async Task<StudentSubcriptionResponse> UpdateStudentSubcription(long id, UpdateStudentSubcriptionRequest updateStudentSubcriptionRequest)
        {
            var existstudentsub = _unitOfWork.StudentSubcriptionRepository.GetByID(id);
            if (existstudentsub == null)
            {
                throw new CustomException.DataNotFoundException("StudentSubcription ID is not exist");
            }
            _mapper.Map(updateStudentSubcriptionRequest, existstudentsub);

            if (existstudentsub.CurrentQuestion >= 20 || existstudentsub.Status == false)
            {
                throw new CustomException.DataNotFoundException("You Subcription has been không xài được địt mẹ mày.");
            }
            else
            {
                existstudentsub.CurrentQuestion += 1;
                await _unitOfWork.StudentSubcriptionRepository.UpdateAsync(existstudentsub);
                _unitOfWork.Save();
            }
            var studentSubcriptionResponse = _mapper.Map<StudentSubcriptionResponse>(existstudentsub);
            return studentSubcriptionResponse;
        }

        public async Task<StudentSubcriptionResponse> DeleteStudentSubcription(long id, UpdateStudentSubcriptionRequest updateStudentSubcriptionRequest)
        {
            var existstudentsub = _unitOfWork.StudentSubcriptionRepository.GetByID(id);
            if (existstudentsub == null)
            {
                throw new CustomException.DataNotFoundException("StudentSubcription ID is not exist");
            }
            _mapper.Map(updateStudentSubcriptionRequest, existstudentsub);

            // set trạng thái nếu trả lời câu hỏi => cộng currentQuestion + 1
            if (existstudentsub.Status == false)
            {
                throw new CustomException.DataNotFoundException("Your Subcription Has Been Deleted");
            }
            else
            {
                existstudentsub.Status = false;
                await _unitOfWork.StudentSubcriptionRepository.UpdateAsync(existstudentsub);
                _unitOfWork.Save();
            }
            var studentSubcriptionResponse = _mapper.Map<StudentSubcriptionResponse>(existstudentsub);
            return studentSubcriptionResponse;
        }
    }
}

