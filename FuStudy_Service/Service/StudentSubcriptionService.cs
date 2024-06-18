﻿using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<StudentSubcriptionResponse>> GetAllStudentSubcription(QueryObject queryObject)
        {
            var getall = _unitOfWork.StudentSubcriptionRepository.Get(
                filter: p => p.Status == true, includeProperties: "Student.User,Subcription",
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize)
                .ToList();

            if (!getall.Any())
            {
                throw new CustomException.DataNotFoundException("No StudentSubcription in Database");
            }

            var studentSubcriptionResponses = _mapper.Map<IEnumerable<StudentSubcriptionResponse>>(getall);

            return studentSubcriptionResponses;
        }

        public async Task<StudentSubcriptionResponse> GetStudentSubcriptionByID(long id)
        {

            var studentsubcriptionByID = _unitOfWork.StudentSubcriptionRepository.Get(
            filter: p => p.Id == id && p.Status == true, includeProperties: "Student.User,Subcription").FirstOrDefault();
            if (studentsubcriptionByID == null)
            {
                throw new CustomException.DataNotFoundException("ID Is not Found.");
            }
            var studentsubcriptionRS = _mapper.Map<StudentSubcriptionResponse>(studentsubcriptionByID);
            return await Task.FromResult(studentsubcriptionRS);
        }

        public async Task<IEnumerable<StudentSubcriptionResponse>> GetStudentSubcriptionByUserID(long userId)
        {
            var studentID = _unitOfWork.StudentRepository.Get(
                filter: s => s.User.Id == userId).FirstOrDefault();
            if (studentID == null)
            {
                throw new CustomException.DataNotFoundException("This UserId doesn't exist");
            }

            var studentsubcription = _unitOfWork.StudentSubcriptionRepository.Get(
            filter: p => p.Student.UserId == userId && p.Status == true,
            includeProperties: "Student.User,Subcription",
            orderBy: q => q.OrderBy(p => p.Id));

            if (studentsubcription == null)
            {
                throw new CustomException.DataNotFoundException("Student Subscription for this user not found.");
            }

            var studentsubcriptionResponse = _mapper.Map <IEnumerable<StudentSubcriptionResponse>>(studentsubcription);
            return studentsubcriptionResponse;

            //var studentID = _unitOfWork.StudentRepository.Get(
            //    filter:s => s.User.Id == userId).FirstOrDefault();
            //if (studentID == null)
            //{
            //    throw new CustomException.DataNotFoundException("This UserId doesn't exist");
            //}

            //var studentSubcription = _unitOfWork.StudentSubcriptionRepository.Get(
            //    filter: ss => ss.StudentId == studentID.Id && ss.Status == true,
            //    includeProperties: "Subcription");

            //var response = studentSubcription.Select(ss => new StudentSubcriptionResponse
            //{
            //    Id = ss.Id,
            //    StudentId = ss.StudentId,
            //    SubcriptionId = ss.SubcriptionId,
            //    CurrentMeeting = ss.CurrentMeeting,
            //    CurrentQuestion = ss.CurrentQuestion,
            //    StartDate = ss.StartDate,
            //    EndDate = ss.EndDate,
            //    Status = ss.Status,
            //    Student = ss.Student,
            //    SubcriptionName = ss.Subcription.SubcriptionName,
            //    SubcriptionPrice = ss.Subcription.SubcriptionPrice
            //});

            //return response;

        }

        public async Task<StudentSubcriptionResponse> CreateStudentSubcription(CreateStudentSubcriptionRequest studentSubcriptionRequest)
        {
            var studentsub = _mapper.Map<StudentSubcription>(studentSubcriptionRequest);
            var student = _unitOfWork.StudentSubcriptionRepository.Get(s => s.Student.Id == studentSubcriptionRequest.UserId,
                includeProperties: "Student.User,Subcription").FirstOrDefault();
            if (student == null)
            {
                throw new CustomException.DataNotFoundException("This User is not a student!!");
            }
            // Set trạng thái (limt cứng 20, câu đầu là 0)
            if (studentsub.CurrentQuestion >= 20)
            {
                throw new CustomException.InvalidDataException("You Subcription has been không xài được địt mẹ mày.");
            }
            else
            {
                studentsub.StudentId = student.Id;
                studentsub.CurrentQuestion = 0;
                studentsub.CurrentMeeting = 0;
                studentsub.StartDate = DateTime.Now;
                studentsub.EndDate = DateTime.Now.AddMonths(2);
                // set hiện tại là true có thể sửa thành false nếu muốn
                studentsub.Status = true;

                await _unitOfWork.StudentSubcriptionRepository.AddAsync(studentsub);
            }

            //map request vào response
            StudentSubcriptionResponse studentSubcriptionResponse = _mapper.Map<StudentSubcriptionResponse>(studentsub);
            return studentSubcriptionResponse;
        }

        public async Task<StudentSubcriptionResponse> UpdateStudentSubcription(long id, UpdateStudentSubcriptionRequest updateStudentSubcriptionRequest)
        {
            // Tạo Hàm gọi xử lí studentSubcription
            var existstudentsub = _unitOfWork.StudentSubcriptionRepository.GetByID(id);
            if (existstudentsub == null)
            {
                throw new CustomException.DataNotFoundException("StudentSubcription ID is not exist");
            }

            // Tạo Hàm gọi xử lí Subcription
            var subcription = _unitOfWork.SubcriptionRepository.GetByID(id);
            if (subcription == null)
            {
                throw new CustomException.DataNotFoundException("Subcription ID is not exist");
            }
            _mapper.Map(updateStudentSubcriptionRequest, existstudentsub);

            // Xử lí câu hỏi của student nếu vượt = cúc ngược lại thì cập nhật
            if (existstudentsub.Status == false)
            {
                throw new CustomException.DataNotFoundException("Your Subcription Doesn't exist");
            }
            else if (existstudentsub.CurrentQuestion > subcription.LimitQuestion)
            {
                throw new CustomException.DataNotFoundException("The Current Questions are out");
            }
            else if (existstudentsub.CurrentMeeting > subcription.LimitMeeting)
            {
                throw new CustomException.DataNotFoundException("The Current Meetings are out");
            }
            else
            {
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

