namespace PageTurner.Models.ViewModels
{
	public class ReviewDetailsViewModel
	{
		public int ID { get; set; }
		public string ReviewContent { get; set; }
		public string UserName { get; set; }
		public string BookName { get; set; }
		public int BookID { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
