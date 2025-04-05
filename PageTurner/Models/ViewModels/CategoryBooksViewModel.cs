namespace PageTurner.Models.ViewModels
{
	public class CategoryBooksViewModel
	{
		public string CategoryName { get; set; }
		public List<BookCardViewModel> Books { get; set; }
		public PagintionViewModel PagintionViewModel { get; set; }
	}
}
