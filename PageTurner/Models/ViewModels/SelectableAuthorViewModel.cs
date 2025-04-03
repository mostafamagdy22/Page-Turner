namespace PageTurner.Models.ViewModels
{
	public class SelectableAuthorViewModel
	{
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
		public bool IsSelected { get; internal set; } = false;
	}
}
