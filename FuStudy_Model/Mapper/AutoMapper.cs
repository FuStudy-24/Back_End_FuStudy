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
            CreateMap<Blog, BlogResponse>()
                .ForMember(dst => dst.Fullname, src => src.MapFrom(x => x.User.Fullname))
                .ForMember(dst => dst.Avatar, src => src.MapFrom(x => x.User.Avatar))
                .ReverseMap();
            #endregion

            #region Question Request
            CreateMap<QuestionRequest, Question>().ReverseMap();
            CreateMap<QuestionCommentRequest, QuestionComment>().ReverseMap();
            #endregion

            #region Subcription Request
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();
            #endregion

            #region Account(Create, Update) RQ, Response
            CreateMap<CreateAccountDTORequest, User>().ReverseMap();
            CreateMap<UpdateAccountDTORequest, User>().ReverseMap();
            
            CreateMap<CreateStudentSubcriptionRequest,  StudentSubcription>().ReverseMap();
            CreateMap<UpdateStudentSubcriptionRequest, StudentSubcription>().ReverseMap();
            #endregion

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            #region Question
            CreateMap<QuestionResponse, Question>().ReverseMap();
            CreateMap<QuestionCommentResponse, QuestionComment>().ReverseMap();
            #endregion

            #region Transaction
            CreateMap<TransactionRequest, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionResponse>().ReverseMap();
            #endregion

            #region Order
            CreateMap<OrderRequest, Order>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            #endregion

            #region Wallet 
            CreateMap<Wallet, WalletResponse>().ReverseMap();
            CreateMap<WalletRequest, Wallet>().ReverseMap();
            #endregion

            #region BlogLike
            CreateMap<BlogLikeRequest, BlogLike>().ReverseMap();
            CreateMap<BlogLike, BlogLikeResponse>().ReverseMap();
            #endregion

            #region Subcription Response
            CreateMap<StudentSubcriptionResponse,  StudentSubcription>().ReverseMap();
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();
            #endregion
        }
    }
}
