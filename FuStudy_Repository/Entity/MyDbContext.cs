using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using Microsoft.Extensions.Configuration;

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
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<StudentSubcription>()
                .HasKey(ss => new { ss.StudentId, ss.SubcriptionId });
        }

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
    }
}
