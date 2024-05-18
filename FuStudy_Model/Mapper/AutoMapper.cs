using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Model.DTO.Response;

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
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            CreateMap<QuestionResponse, Question>().ReverseMap();
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();

        }
    }
}
