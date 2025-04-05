namespace PageTurner.Models.ViewModels
{
	public class PagintionViewModel
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
	}
}
