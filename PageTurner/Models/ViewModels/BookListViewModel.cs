namespace PageTurner.Models.ViewModels
{
	public class BookListViewModel
	{
		public List<BookCardViewModel> Books { get; set; } = new List<BookCardViewModel>();
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
	}
}
