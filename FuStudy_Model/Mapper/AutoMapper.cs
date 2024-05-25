using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;

namespace FuStudy_Model.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Request
            //CreateMap<FuStudyRequest, FuStudy>().ReverseMap();
            CreateMap<CreateAccountDTORequest, User>().ReverseMap();

            #region Blog
            CreateMap<BlogRequest, Blog>();
            CreateMap<Blog, BlogResponse>();
            #endregion

            CreateMap<QuestionRequest, Question>().ReverseMap();

            #region Subcription Request
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();
            CreateMap<CreateStudentSubcriptionRequest,  StudentSubcription>().ReverseMap();
            CreateMap<UpdateStudentSubcriptionRequest, StudentSubcription>().ReverseMap();
            #endregion

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            CreateMap<QuestionResponse, Question>().ReverseMap();

            #region Subcription Response
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();
            CreateMap<StudentSubcriptionResponse,  StudentSubcription>().ReverseMap();
            #endregion
        }
    }
}
