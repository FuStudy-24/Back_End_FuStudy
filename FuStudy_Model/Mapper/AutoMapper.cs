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
            CreateMap<QuestionRequest, Question>().ReverseMap();
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            CreateMap<QuestionResponse, Question>().ReverseMap();
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();

        }
    }
}
