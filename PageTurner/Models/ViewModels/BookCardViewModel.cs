namespace PageTurner.Models.ViewModels
{
	public class BookCardViewModel
	{
		public int ID { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
		public List<BookCategories> BookCategories { get; set; } = new List<BookCategories>();
		public Publisher Publisher { get; set; }
	}
}
