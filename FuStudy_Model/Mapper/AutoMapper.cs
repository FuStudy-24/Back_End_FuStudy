﻿using AutoMapper;
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
            CreateMap<CreateAccountDTOResponse, User>().ReverseMap();
            CreateMap<LoginDTOResponse, User>().ReverseMap();
            CreateMap<User, UserDTOResponse>().ReverseMap();
            CreateMap<User, LoginDTOResponse>().ReverseMap();
            CreateMap<Mentor, RegisterTutorRequest>().ReverseMap();
            CreateMap<Mentor, RegisterTutorResponse>().ReverseMap();
            CreateMap<Token, TokenRequest>().ReverseMap();
            CreateMap<RegisterRequest, User>().ReverseMap();
            #region RolePermission
            CreateMap<RolePermissionRequest, RolePermission>().ReverseMap();
            CreateMap<RolePermission, RolePermissionResponse>().ReverseMap();
            #endregion

            #region Role
            CreateMap<RoleRequest, Role>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();
            #endregion

            #region Permission
            CreateMap<PermissionRequest, Permission>().ReverseMap();
            CreateMap<Permission, PermissionResponse>().ReverseMap();
            #endregion

            #region RolePermission
            CreateMap<RolePermissionRequest,  RolePermission>().ReverseMap();
            CreateMap<RolePermission, RolePermissionResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission));
            #endregion

            #region Major
            CreateMap<MajorRequest, Major>().ReverseMap();
            CreateMap<Major, MajorResponse>().ReverseMap();
            #endregion

            #region Mentor
            CreateMap<MentorRequest, Mentor>().ReverseMap();
            CreateMap<Mentor, MentorResponse>().ReverseMap();
            #endregion

            #region MentorOnlineStatus
            CreateMap<UpdateMentorOnlineStatusResquest, Mentor>().ReverseMap();
            CreateMap<Mentor,  UpdateMentorOnlineStatusResponse>().ReverseMap();
            #endregion

            #region MentorMajor
            CreateMap<MentorMajorRequest, MentorMajor>().ReverseMap();
            CreateMap<MentorMajor, MentorMajorResponse>().ReverseMap();
            #endregion

            #region Student

            CreateMap<Student, StudentResponse>().ReverseMap();
            #endregion

            #region MeetingHistory

            CreateMap<MeetingHistory, MeetingHistoryResponse>().ReverseMap();
            #endregion
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

            #region Booking 
            CreateMap<CreateBookingRequest, Booking>().ReverseMap();
            CreateMap<BookingResponse, Booking>().ReverseMap();
            #endregion

            #region Comment Image
            CreateMap<CommentImageRequest, CommentImage>();
            CreateMap<CommentImage, CommentImageResponse>();
            #endregion

            #region Question Request
            CreateMap<QuestionRequest, Question>().ReverseMap();
            CreateMap<QuestionCommentRequest, QuestionComment>().ReverseMap();
            CreateMap<QuestionRatingRequest, QuestionRating>().ReverseMap();
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

            #region Category Request
            CreateMap<CategoryRequest, Category>().ReverseMap();
            #endregion

            //Reponse
            //CreateMap<FuStudyReponse, FuStudy>().ReverseMap();
            #region Question Response
            CreateMap<Question, QuestionResponse>()
                .ForMember(question => question.CategoryName, 
                    src
                        => src.MapFrom(x => x.Category.CategoryName))
                .ReverseMap();
            CreateMap<QuestionCommentResponse, QuestionComment>()
                .ForMember(comment => comment.Question
                    , src => src.MapFrom(x => x.QuestionResponse) )
                .ReverseMap();
            CreateMap<QuestionRating, QuestionRatingResponse>()
                .ReverseMap();
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
            
            #region Category Response
            CreateMap<CategoryResponse, Category>().ReverseMap();
            #endregion
            //Conversation
            CreateMap<ConversationRequest, Conversation>().ForMember(dest => dest.Duration, opt => opt.Ignore());

            CreateMap<Conversation, ConversationResponse>();

            CreateMap<ConversationMessageRequest, ConversationMessage>();
            CreateMap<ConversationMessage, ConversationMessageResponse>().ReverseMap();

            CreateMap<MessageReactionRequest, MessageReaction>();

            CreateMap<MessageReaction, MessageReactionResponse>().ReverseMap();

            CreateMap<Attachment, AttachmentResponse>();

            #region StudentResponse

            CreateMap<Student, StudentResponse>().ReverseMap();

            #endregion
        }
    }
}
