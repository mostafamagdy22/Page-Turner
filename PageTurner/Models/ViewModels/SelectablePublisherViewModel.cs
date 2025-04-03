namespace PageTurner.Models.ViewModels
{
	public class SelectablePublisherViewModel
	{
		public int PublisherID { get; set; }
		public string PublisherName { get; set; }
		public bool IsSelected { get; set; } = false;
	}
}
