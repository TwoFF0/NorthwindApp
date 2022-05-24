namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    using Microsoft.EntityFrameworkCore;
    using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        public virtual DbSet<BlogArticleEntity> BlogArticles { get; set; }

        public virtual DbSet<BlogArticleProductEntity> BlogProducts { get; set; }

        public virtual DbSet<BlogArticleCommentEntity> BlogComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticleEntity>(entity =>
            {
                entity.ToTable("blog_article").HasKey(x => x.BlogArticleId);

                entity.Property(x => x.Title).HasColumnName("title").IsRequired().HasMaxLength(50);
                entity.Property(x => x.Content).HasColumnName("content").IsRequired();
                entity.Property(x => x.Posted).HasColumnName("posted");
                entity.Property(x => x.EmployeeID).HasColumnName("employee_id");

                entity.HasMany(x => x.Products)
                    .WithOne()
                    .HasForeignKey(x => x.BlogArticleId);

                entity.HasMany(x => x.Comments)
                    .WithOne()
                    .HasForeignKey(x => x.BlogArticleId);
            });

            modelBuilder.Entity<BlogArticleProductEntity>(entity =>
            {
                entity.ToTable("blog_article_product").HasKey(x => x.Id);

                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.BlogArticleId).HasColumnName("blog_article_id");
            });

            modelBuilder.Entity<BlogArticleCommentEntity>(entity =>
            {
                entity.ToTable("blog_article_comments").HasKey(x => x.Id);

                entity.Property(x => x.CustomerId).HasColumnName("customer_id");
                entity.Property(x => x.BlogArticleId).HasColumnName("blog_article_id");
                entity.Property(x => x.Comment).HasColumnName("comment").HasMaxLength(400).IsRequired();
            });
        }
    }
}
