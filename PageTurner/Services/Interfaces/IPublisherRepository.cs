using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Interfaces
{
	public interface IPublisherRepository
	{
		public Task<IEnumerable<Publisher>> GetPublishersAsync();
		public Task<IEnumerable<int>> GetPublishersIDsAsync();
		public Task<IEnumerable<string>> GetPublishersNamesAsync();
		public Task<IEnumerable<SelectablePublisherViewModel>> SelectablePublisherViewModelsAsync();
	}
}
