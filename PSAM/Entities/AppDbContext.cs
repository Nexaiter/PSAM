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
            /*modelBuilder.Entity<SubscribersEntity>()
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
            */

            // Configure the relationship between AccountEntity and SubscribersEntity
            modelBuilder.Entity<SubscribersEntity>()
                .HasKey(e => new { e.SubscriberId, e.SubscribeeId }); // Composite key for the join table

            modelBuilder.Entity<SubscribersEntity>()
                .HasOne(e => e.SubscriberAccount)
                .WithMany(a => a.Subscribees)
                .HasForeignKey(e => e.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict); // Avoids circular cascade delete

            modelBuilder.Entity<SubscribersEntity>()
                .HasOne(e => e.SubscribeeAccount)
                .WithMany(a => a.Subscribers)
                .HasForeignKey(e => e.SubscribeeId)
                .OnDelete(DeleteBehavior.Restrict); // Avoids circular cascade delete

            // Konfiguracja relacji Comment -> Post (bez kaskadowego usuwania)
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments) // Zakładając, że PostEntity ma kolekcję Comments
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Bez kaskadowego usuwania

            modelBuilder.Entity<CommentEntity>()
               .HasOne(c => c.ParentComment)
               .WithMany(c => c.Replies)
               .HasForeignKey(c => c.CommentId)
               .OnDelete(DeleteBehavior.Restrict); // Bez kaskadowego usuwania

            base.OnModelCreating(modelBuilder);

        }
    }
}
