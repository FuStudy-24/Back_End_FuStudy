using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using Microsoft.Extensions.Configuration;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Internal;

namespace FuStudy_Repository.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogComment> BlogsComments { get; set; }
        public DbSet<BlogLike> BlogsLikes { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<Major> Majors { get; set; }

        public DbSet<MentorMajor> MentorMajors { get; set; }

        public DbSet<MeetingHistory> MeetingHistories { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionComment> QuestionComments { get; set; }
        public DbSet<QuestionRating> QuestionRatings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentSubcription> StudentSubcriptions { get; set; }
        public DbSet<Subcription> Subcriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("MyDB");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // -- Roles & Permissions --
            var roles = new List<Role>
            {
                new Role {Id = 1, RoleName = "Admin" },
                new Role {Id = 2, RoleName = "Moderator" },
                new Role {Id = 3, RoleName = "Student" },
                new Role { Id = 4, RoleName = "Mentor" }

            };

            modelBuilder.Entity<Role>().HasData(roles);

            var permissions = new List<Permission>
            {
                new Permission {Id = 1, PermissionName = "ViewQuestions" },
                new Permission {Id = 2, PermissionName = "AskQuestions" },
                new Permission {Id = 3, PermissionName = "AnswerQuestions" },
                new Permission {Id = 4, PermissionName = "CreateBlogs" }
            };


            // Assign permissions to roles (e.g., ViewQuestions to both Student and Mentor)
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { Id = 1, RoleId = roles[0].Id, PermissionId = permissions[0].Id }, // Student - ViewQuestions
                new RolePermission { Id = 2, RoleId = roles[1].Id, PermissionId = permissions[0].Id }, // Mentor - ViewQuestions
                new RolePermission { Id = 3, RoleId = roles[1].Id, PermissionId = permissions[2].Id } // Mentor - AnswerQuestions
                // ... Add more RolePermission instances as needed
            );
            modelBuilder.Entity<Permission>().HasData(permissions);

            // -- Users --
            var users = new List<User>
            {
                new User
                {
                    Id = 1, RoleId = roles[2].Id, Username = "student1", Password = "hashedPassword1", Fullname = "John Doe",
                    Email = "johndoe@example.com", Avatar = "ahihi", Gender = "male", IdentityCard = "ahihi", Status = true, Phone = "123123"
                },
                new User
                {
                    Id = 2, RoleId = roles[3].Id, Username = "mentor1", Password = "hashedPassword2", Fullname = "Jane Smith",
                    Email = "janesmith@example.com", Avatar = "ahihi", Gender = "Gay", IdentityCard = "Ahihi", Status = true, Phone = "12312321"
                },
                new User
                {
                    Id = 3, RoleId = roles[0].Id, Username = "admin1", Password = "hashedPassword3", Fullname = "Alice Johnson",
                    Email = "alicejohnson@example.com", Avatar = "ahihi", Gender = "Female", IdentityCard = "Ahihi", Status = true, Phone = "123123"
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            // -- Students & Mentors --
            var students = new List<Student> { new Student { Id = 1, UserId = users[0].Id } };
            

            modelBuilder.Entity<Student>().HasData(students);


            // -- Majors & MentorMajors --
            var majors = new List<Major>
            {
                new Major {Id = 1, MajorName = "Computer Science" },
                new Major {Id = 2, MajorName = "Mathematics" }
            };
            modelBuilder.Entity<Major>().HasData(majors);
            var mentors = new List<Mentor>
                { new Mentor {Id = 1, UserId = users[2].Id, AcademicLevel = "Master's"
                    , WorkPlace = "Tech Company", OnlineStatus = "offline", Skill = "Ahihi", Video = "ahihi"} };

            var mentorMajor = new MentorMajor {Id = 1, MentorId = mentors[0].Id, MajorId = majors[0].Id };

            modelBuilder.Entity<Mentor>().HasData(mentors);
            modelBuilder.Entity<MentorMajor>().HasData(mentorMajor);
            


            // ... (Continue adding data for other entities) ...


            // ... (Continuing from the previous code) ...

            // -- Categories --
            var categories = new List<Category>
            {
                new Category {Id = 1, CategoryName = "Programming" },
                new Category {Id = 2, CategoryName = "Algorithms" },
                new Category {Id = 3,  CategoryName = "Data Structures" }
            };
            modelBuilder.Entity<Category>().HasData(categories);


            // -- Questions --
            var questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    StudentId = students[0].Id,
                    CategoryId = categories[0].Id,
                    Content = "How to sort an array in C#?",
                    CreateDate = DateTime.Now,
                    Image = "ahihi"
                    
                }
            };
            modelBuilder.Entity<Question>().HasData(questions);


            // -- Subscriptions --
            var subscriptions = new List<Subcription>
            {
                new Subcription {Id = 1, SubcriptionName = "Basic", SubcriptionPrice = 9.99, Status = true },
                new Subcription {Id = 2, SubcriptionName = "Premium", SubcriptionPrice = 19.99, Status = true }
            };
            modelBuilder.Entity<Subcription>().HasData(subscriptions);


            // -- StudentSubscriptions --
            modelBuilder.Entity<StudentSubcription>().HasData(new StudentSubcription
            {
                Id = 1,
                StudentId = students[0].Id,
                SubcriptionId = subscriptions[0].Id,
                LimitQuestion = 10,
                CurrentQuestion = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            });

            // -- Orders --
            modelBuilder.Entity<Order>().HasData(new Order
            {
                Id = 1,
                StudentId = students[0].Id,
                SubcriptionId = subscriptions[0].Id,
                PaymentCode = "PAY12345",
                Description = "Payment for Basic subscription",
                CreateDate = DateTime.Now,
                Money = 9.99,
                Status = true
            });

            // -- Wallets --
            var wallets = new List<Wallet>
            {
                new Wallet {Id = 1, UserId = users[0].Id, Balance = 100, Status = true },
                new Wallet {Id = 2, UserId = users[1].Id, Balance = 200, Status = false }
            };
            modelBuilder.Entity<Wallet>().HasData(wallets);

            // -- Transactions --
            modelBuilder.Entity<Transaction>().HasData(new Transaction
            {
                Id = 1,
                WalletId = wallets[0].Id,
                Type = "Payment",
                Ammount = -9.99,
                CreateTime = DateTime.Now,
                Description = "Subscription payment"
            });

            // -- Blogs --
            var blogs = new List<Blog>
            {
                new Blog { Id = 1 ,UserId = users[1].Id, Content = "How to be a better mentor", CreateDate = DateTime.Now, Image = "ahihi"}
            };
            modelBuilder.Entity<Blog>().HasData(blogs);


            // -- BlogComments & BlogLikes --
            modelBuilder.Entity<BlogComment>().HasData(new BlogComment
                { Id = 1, UserId = users[0].Id, BlogId = 1, Content = "Great post!", CreateDate = DateTime.Now });
            modelBuilder.Entity<BlogLike>().HasData(new BlogLike
                {Id = 2, UserId = users[0].Id, BlogId = 1, TotalLike = 1, Status = true });

            // -- QuestionComments & QuestionRatings --
            modelBuilder.Entity<QuestionComment>().HasData(new QuestionComment
                {Id = 1, UserId = users[1].Id, QuestionId = questions[0].Id, Content = "Good question!", CreateDate = DateTime.Now });
            modelBuilder.Entity<QuestionRating>().HasData(new QuestionRating
                {Id = 1, UserId = users[1].Id, QuestionId = questions[0].Id, TotalRating = 5, Status = true });


            // -- Conversations & ConversationMessages --
            var conversation = new Conversation
                {Id = 1, User1Id = users[0].Id, User2Id = users[1].Id, CreateAt = DateTime.Now, LastMessage = "Hello!" };

            modelBuilder.Entity<Conversation>().HasData(conversation);

            var conversationMessage = new ConversationMessage
                {Id = 1, ConversationId = conversation.Id, SenderId = users[0].Id, Content = "Hello!", CreateTime = DateTime.Now };
            modelBuilder.Entity<ConversationMessage>().HasData(conversationMessage);

            // -- MessageReactions --
            modelBuilder.Entity<MessageReaction>().HasData(new MessageReaction
            {
                Id = 1, UserId = users[1].Id, ConversationMessageId = conversationMessage.Id, ReactionType = "like", CreateAt = DateTime.Now
            });
        }
    }
}
