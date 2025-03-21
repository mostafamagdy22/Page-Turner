using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace PageTurner.Models
{
	public class ApplicationDbContext : IdentityDbContext
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
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<BookAuthor>()
				.HasKey(ba => new {ba.BookISBN, ba.AuthorID}); // Composite Key

			builder.Entity<BookAuthor>()
			.HasOne(ba => ba.Book)
			.WithMany(b => b.BookAuthors)
			.HasForeignKey(ba => ba.BookISBN)
			.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookAuthor>()
			.HasOne(ba => ba.Author)
			.WithMany(a => a.BookAuthors)
			.HasForeignKey(ba => ba.AuthorID)
			.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Book>()
			.HasMany(b => b.BookAuthors)
			.WithOne(ba => ba.Book)
			.HasForeignKey(ba => ba.BookISBN)
			.IsRequired();
		}
	}
}
