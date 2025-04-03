using Microsoft.EntityFrameworkCore;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;

namespace PageTurner.Services
{
	public class PublisherRepository : IPublisherRepository
	{
		private readonly ApplicationDbContext _context;
		public PublisherRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Publisher>> GetPublishersAsync()
		{
			return await _context.Publishers.ToListAsync();
		}

		public async Task<IEnumerable<int>> GetPublishersIDsAsync()
		{
			return await _context.Publishers.Select(p => p.ID).ToListAsync();
		}

		public async Task<IEnumerable<string>> GetPublishersNamesAsync()
		{
			return await _context.Publishers.Select(p => p.Name).ToListAsync();
		}

		public async Task<IEnumerable<SelectablePublisherViewModel>> SelectablePublisherViewModelsAsync()
		{
			return await _context.Publishers.Select(p => new SelectablePublisherViewModel
			{
				PublisherID = p.ID,
				PublisherName = p.Name,
			}).ToListAsync();
		}
	}
}
