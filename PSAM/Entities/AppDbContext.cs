using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PSAM.Entities
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TechnologyEntity> Technologies { get; set; }
        public DbSet<SubscribersEntity> Subscribers { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSqlLocalDb;Database=psamDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja SubscribersEntity
            modelBuilder.Entity<SubscribersEntity>()
                .HasKey(s => new { s.SubscriberId, s.SubscribeeId });

            modelBuilder.Entity<SubscribersEntity>()
                .HasOne(s => s.SubscriberAccount)
                .WithMany(a => a.Subscribing)
                .HasForeignKey(s => s.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubscribersEntity>()
                .HasOne(s => s.SubscribeeAccount)
                .WithMany(a => a.Subscribers)
                .HasForeignKey(s => s.SubscribeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracja TechnologyEntity
            modelBuilder.Entity<TechnologyEntity>()
                .HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            /////////////////////////////////////////////////////
           
        }
    }
}
