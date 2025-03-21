using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _webHostEnvironment; 
		public FileService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<string> Upload(IFormFile file,string location)
		{
			if (file == null || file.Length == 0)
				return "No file uploaded.";

			try
			{
				var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
				var extension = Path.GetExtension(file.FileName).ToLower();
				if (!allowedExtensions.Contains(extension))
					return "Invalid file type.";

				var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, location);
				Directory.CreateDirectory(uploadsDir); 

				var fileName = $"{Guid.NewGuid()}{extension}";
				var fullPath = Path.Combine(uploadsDir, fileName);

				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				return $"/{location}/{fileName}";
			}
			catch
				{
					return "problem";
				}
			}
	}
}
