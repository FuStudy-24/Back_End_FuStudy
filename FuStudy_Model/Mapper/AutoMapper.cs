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

            CreateMap<QuestionRequest, Question>().ReverseMap();
            CreateMap<QuestionCommentRequest, QuestionComment>().ReverseMap();
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();
            CreateMap<CreateAccountDTORequest, User>().ReverseMap();
            CreateMap<UpdateAccountDTORequest, User>().ReverseMap();
            

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            
            #region Question
            CreateMap<QuestionResponse, Question>().ReverseMap();
            CreateMap<QuestionCommentResponse, QuestionComment>()
                .ForMember(comment => comment.Question
                    , src => src.MapFrom(x => x.QuestionResponse) )
                .ReverseMap();
            #endregion
            
            
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();

            CreateMap<TransactionRequest, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionResponse>().ReverseMap();

            CreateMap<OrderRequest, Order>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();

            CreateMap<Wallet, WalletResponse>().ReverseMap();
            CreateMap<WalletRequest, Wallet>().ReverseMap();

            #region BlogLike
            CreateMap<BlogLikeRequest, BlogLike>().ReverseMap();
            CreateMap<BlogLike, BlogLikeResponse>().ReverseMap();
            #endregion
        }
    }
}
