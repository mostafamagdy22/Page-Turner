using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace PageTurner.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			
		}
		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<Publisher> Publishers { get; set; }
		public DbSet<BookAuthor> bookAuthors { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<BookCategories> BookCategories { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<BookCategories>().ToTable("BookCategory");

			builder.Entity<BookCategories>(options =>
				{
					options.Property(bc => bc.BookID).HasColumnName("BooksID");
					options.Property(bc => bc.CategoryID).HasColumnName("CategoriesID");
				});

			builder.Entity<BookAuthor>()
				.HasKey(ba => new {ba.BookID, ba.AuthorID}); // Composite Key

			builder.Entity<BookAuthor>()
			.HasOne(ba => ba.Book)
			.WithMany(b => b.BookAuthors)
			.HasForeignKey(ba => ba.BookID)
			.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookAuthor>()
			.HasOne(ba => ba.Author)
			.WithMany(a => a.BookAuthors)
			.HasForeignKey(ba => ba.AuthorID)
			.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Book>()
			.HasMany(b => b.BookAuthors)
			.WithOne(ba => ba.Book)
			.HasForeignKey(ba => ba.BookID)
			.IsRequired();

			builder.Entity<Author>()
				.Property(a => a.Gender)
				.HasConversion<string>();

			builder.Entity<Review>()
				.HasOne(r => r.user)
				.WithMany(u => u.Reviews)
				.HasForeignKey(r => r.UserID)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Review>()
				.HasOne(r => r.book)
				.WithMany(b => b.Reviews)
				.HasForeignKey(r => r.BookID)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookCategories>()
				.HasKey(ba => new { ba.BookID, ba.CategoryID }); // Composite Key

			builder.Entity<BookCategories>()
			.HasOne(ba => ba.Book)
			.WithMany(b => b.BookCategories)
			.HasForeignKey(ba => ba.BookID)
			.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookCategories>()
			.HasOne(ba => ba.Category)
			.WithMany(a => a.BookCategories)
			.HasForeignKey(ba => ba.CategoryID)
			.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
