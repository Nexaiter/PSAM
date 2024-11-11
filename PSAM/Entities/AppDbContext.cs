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
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<PostLikeEntity> PostLikes { get; set; }
        public DbSet<CommentLikeEntity> CommentLikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSqlLocalDb;Database=psamDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
               .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracja dla ImageEntity
            modelBuilder.Entity<ImageEntity>()
                .HasOne(i => i.Post)
                .WithMany()
                .HasForeignKey(i => i.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure PostLikeEntity relationships with Post and Account
            modelBuilder.Entity<PostLikeEntity>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostLikeEntity>()
                .HasOne(pl => pl.Account)
                .WithMany(a => a.PostLikes)
                .HasForeignKey(pl => pl.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure CommentLikeEntity relationships with Comment and Account
            modelBuilder.Entity<CommentLikeEntity>()
                .HasOne(cl => cl.Comment)
                .WithMany(c => c.CommentLikes)
                .HasForeignKey(cl => cl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentLikeEntity>()
                .HasOne(cl => cl.Account)
                .WithMany(a => a.CommentLikes)
                .HasForeignKey(cl => cl.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }
    }
}
