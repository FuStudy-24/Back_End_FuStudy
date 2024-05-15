using FuStudy_Repository.Repository;
using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository
{
    public class UnitOfWork
    {
        private MyDbContext _context = new MyDbContext();
        private GenericRepository<Attachment> _attachmentRepository;
        private GenericRepository<Blog> _blogRepository;
        private GenericRepository<BlogComment> _blogCommentRepository;
        private GenericRepository<BlogLike> _blogLikeRepository;
        private GenericRepository<Booking> _bookingRepository;
        private GenericRepository<Category> _categoryRepository;
        private GenericRepository<Conversation> _conversationRepository;
        private GenericRepository<ConversationMessage> _conversationMessageRepository;
        private GenericRepository<Major> _majorRepository;
        private GenericRepository<MeetingHistory> _meetingHistoryRepository;
        private GenericRepository<Mentor> _mentorRepository;
        private GenericRepository<MentorMajor> _mentorMajorRepository;
        private GenericRepository<MessageReaction> _messageReactionRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<Permission> _permissionRepository;
        private GenericRepository<Question> _questionRepository;
        private GenericRepository<QuestionComment> _questionCommentRepository;
        private GenericRepository<QuestionRating> _questionRatingRepository;
        private GenericRepository<Role> _roleRepository;
        private GenericRepository<RolePermission> _rolePermissionRepository;
        private GenericRepository<Student> _studentRepository;
        private GenericRepository<StudentSubcription> _StudentSubcriptionRepository;
        private GenericRepository<Subcription> _subcriptionRepository;
        private GenericRepository<Transaction> _transactionRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Wallet> _walletRepository;

        public GenericRepository<Attachment> AttachmentRepository
        {
            get
            {

                if (this._attachmentRepository == null)
                {
                    this._attachmentRepository = new GenericRepository<Attachment>(_context);
                }
                return _attachmentRepository;
            }
        }
        public GenericRepository<Blog> BlogRepository
        {
            get
            {

                if (this._blogRepository == null)
                {
                    this._blogRepository = new GenericRepository<Blog>(_context);
                }
                return _blogRepository;
            }
        }
        public GenericRepository<BlogComment> BlogCommentRepository
        {
            get
            {

                if (this._blogCommentRepository == null)
                {
                    this._blogCommentRepository = new GenericRepository<BlogComment>(_context);
                }
                return _blogCommentRepository;
            }
        }
        public GenericRepository<BlogLike> BlogLikeRepository
        {
            get
            {

                if (this._blogLikeRepository == null)
                {
                    this._blogLikeRepository = new GenericRepository<BlogLike>(_context);
                }
                return _blogLikeRepository;
            }
        }
        public GenericRepository<Booking> BookingRepository
        {
            get
            {

                if (this._bookingRepository == null)
                {
                    this._bookingRepository = new GenericRepository<Booking>(_context);
                }
                return _bookingRepository;
            }
        }
        public GenericRepository<Category> CategoryRepository
        {
            get
            {

                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new GenericRepository<Category>(_context);
                }
                return _categoryRepository;
            }
        }
        public GenericRepository<Conversation> ConversationRepository
        {
            get
            {

                if (this._conversationRepository == null)
                {
                    this._conversationRepository = new GenericRepository<Conversation>(_context);
                }
                return _conversationRepository;
            }
        }
        public GenericRepository<ConversationMessage> ConversationMessageRepository
        {
            get
            {

                if (this._conversationMessageRepository == null)
                {
                    this._conversationMessageRepository = new GenericRepository<ConversationMessage>(_context);
                }
                return _conversationMessageRepository;
            }
        }
        public GenericRepository<Major> MajorRepository
        {
            get
            {

                if (this._majorRepository == null)
                {
                    this._majorRepository = new GenericRepository<Major>(_context);
                }
                return _majorRepository;
            }
        }
        public GenericRepository<MeetingHistory> MeetingHistoryRepository
        {
            get
            {

                if (this._meetingHistoryRepository == null)
                {
                    this._meetingHistoryRepository = new GenericRepository<MeetingHistory>(_context);
                }
                return _meetingHistoryRepository;
            }
        }
        public GenericRepository<Mentor> AMentorRepository
        {
            get
            {

                if (this._mentorRepository == null)
                {
                    this._mentorRepository = new GenericRepository<Mentor>(_context);
                }
                return _mentorRepository;
            }
        }
        public GenericRepository<MentorMajor> MentorMajorRepository
        {
            get
            {

                if (this._mentorMajorRepository == null)
                {
                    this._mentorMajorRepository = new GenericRepository<MentorMajor>(_context);
                }
                return _mentorMajorRepository;
            }
        }
        public GenericRepository<MessageReaction> MessageReactionRepository
        {
            get
            {

                if (this._messageReactionRepository == null)
                {
                    this._messageReactionRepository = new GenericRepository<MessageReaction>(_context);
                }
                return _messageReactionRepository;
            }
        }
        public GenericRepository<Order> OrderRepository
        {
            get
            {

                if (this._orderRepository == null)
                {
                    this._orderRepository = new GenericRepository<Order>(_context);
                }
                return _orderRepository;
            }
        }
        public GenericRepository<Permission> PermissionRepository
        {
            get
            {

                if (this._permissionRepository == null)
                {
                    this._permissionRepository = new GenericRepository<Permission>(_context);
                }
                return _permissionRepository;
            }
        }
        public GenericRepository<Question> QuestionRepository
        {
            get
            {

                if (this._questionRepository == null)
                {
                    this._questionRepository = new GenericRepository<Question>(_context);
                }
                return _questionRepository;
            }
        }
        public GenericRepository<QuestionComment> QuestionCommentRepository
        {
            get
            {

                if (this._questionCommentRepository == null)
                {
                    this._questionCommentRepository = new GenericRepository<QuestionComment>(_context);
                }
                return _questionCommentRepository;
            }
        }
        public GenericRepository<QuestionRating> QuestionRatingRepository
        {
            get
            {

                if (this._questionRatingRepository == null)
                {
                    this._questionRatingRepository = new GenericRepository<QuestionRating>(_context);
                }
                return _questionRatingRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {

                if (this._roleRepository == null)
                {
                    this._roleRepository = new GenericRepository<Role>(_context);
                }
                return _roleRepository;
            }
        }
        public GenericRepository<RolePermission> RolePermissionRepository
        {
            get
            {

                if (this._rolePermissionRepository == null)
                {
                    this._rolePermissionRepository = new GenericRepository<RolePermission>(_context);
                }
                return _rolePermissionRepository;
            }
        }
        public GenericRepository<Student> StudentRepository
        {
            get
            {

                if (this._studentRepository == null)
                {
                    this._studentRepository = new GenericRepository<Student>(_context);
                }
                return _studentRepository;
            }
        }
        public GenericRepository<StudentSubcription> StudentSubcriptionRepository
        {
            get
            {

                if (this._StudentSubcriptionRepository == null)
                {
                    this._StudentSubcriptionRepository = new GenericRepository<StudentSubcription>(_context);
                }
                return _StudentSubcriptionRepository;
            }
        }
        public GenericRepository<Subcription> SubcriptionRepository
        {
            get
            {

                if (this._subcriptionRepository == null)
                {
                    this._subcriptionRepository = new GenericRepository<Subcription>(_context);
                }
                return _subcriptionRepository;
            }
        }
        public GenericRepository<Transaction> TransactionRepository
        {
            get
            {

                if (this._transactionRepository == null)
                {
                    this._transactionRepository = new GenericRepository<Transaction>(_context);
                }
                return _transactionRepository;
            }
        }
        public GenericRepository<User> UserRepository
        {
            get
            {

                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }
        public GenericRepository<Wallet> WalletRepository
        {
            get
            {

                if (this._walletRepository == null)
                {
                    this._walletRepository = new GenericRepository<Wallet>(_context);
                }
                return _walletRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
