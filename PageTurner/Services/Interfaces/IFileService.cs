namespace PageTurner.Services.Interfaces
{
	public interface IFileService
	{
		public Task<string> Upload(IFormFile file, string location);
		public bool Delete(string fileName,string location);
	}
}
