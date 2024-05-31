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
            CreateMap<LoginDTORequest, User>().ReverseMap();
            CreateMap<User, UserDTOResponse>().ReverseMap();
            CreateMap<User, LoginDTOResponse>().ReverseMap();

            #region Blog
            CreateMap<BlogRequest, Blog>();
            CreateMap<Blog, BlogResponse>()
                .ForMember(dst => dst.Fullname, src => src.MapFrom(x => x.User.Fullname))
                .ForMember(dst => dst.Avatar, src => src.MapFrom(x => x.User.Avatar))
                .ReverseMap();
            #endregion

            #region Blog comment
            CreateMap<BlogCommentRequest, BlogComment>()
                .ForSourceMember(src => src.CommentImage, opt => opt.DoNotValidate())
                .ReverseMap();

            CreateMap<BlogComment, BlogCommentResponse>()
                .ForMember(dest => dest.Avatar, src => src.MapFrom(x => x.User.Avatar))
                .ForMember(dest => dest.Fullname, src => src.MapFrom(x => x.User.Fullname))
                .ReverseMap();
            #endregion

            #region Comment Image
            CreateMap<CommentImageRequest, CommentImage>();
            CreateMap<CommentImage, CommentImageResponse>();
            #endregion

            CreateMap<QuestionRequest, Question>().ReverseMap();
            CreateMap<CreateSubcriptionRequest, Subcription>().ReverseMap();

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            CreateMap<QuestionResponse, Question>().ReverseMap();
            CreateMap<SubcriptionResponse, Subcription>().ReverseMap();


            #region BlogLike
            CreateMap<BlogLikeRequest, BlogLike>().ReverseMap();
            CreateMap<BlogLike, BlogLikeResponse>().ReverseMap();
            #endregion

            //Conversation
            CreateMap<ConversationRequest, Conversation>().ForMember(dest => dest.Duration, opt => opt.Ignore());

            CreateMap<Conversation, ConversationResponse>();

            CreateMap<ConversationMessageRequest, ConversationMessage>();
            CreateMap<ConversationMessage, ConversationMessageResponse>().ReverseMap();

            CreateMap<MessageReactionRequest, MessageReaction>();

            CreateMap<MessageReaction, MessageReactionResponse>().ReverseMap();

            CreateMap<Attachment, AttachmentResponse>();
        }
    }
}
