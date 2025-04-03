using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Repositories.Interfaces;
using PageTurner.Services.Interfaces;

namespace PageTurner.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
		private readonly IAuthorRepository _authorReposotory;
		private readonly IFileService _fileService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IMapper _mapper;
		private readonly IPublisherRepository _publisherRepository;
		private readonly ICategoryRepository _categoryRepository;
		public BooksController(ICategoryRepository categoryRepository,IPublisherRepository publisherRepository,IMapper mapper,IWebHostEnvironment webHostEnvironment,IBookRepository bookRepository,IAuthorRepository authorRepository,IFileService fileService)
		{
			_bookRepository = bookRepository;
            _authorReposotory = authorRepository;
			_fileService = fileService;
			_webHostEnvironment = webHostEnvironment;
			_mapper = mapper;
			_publisherRepository = publisherRepository;
			_categoryRepository = categoryRepository;
		}
        [HttpGet]
		public async Task<IActionResult> Index(int pageNumber = 1,int pageSize = 10)
        {
			bool isAdmin = User.IsInRole("Admin");
			int TotalBooks = await _bookRepository.GetBooksCountAsync();
			var books = await _bookRepository.GetBooks(pageNumber,pageSize,isAdmin);
			IEnumerable<BookCardViewModel> booksModel = _mapper.Map<IEnumerable<BookCardViewModel>>(books);
			BookListViewModel model = new BookListViewModel
			{
				Books = booksModel.ToList(),
				CurrentPage = pageNumber,
				TotalPages = (int)Math.Ceiling((double)TotalBooks / pageSize)
			};

			return View(model);
        }
        public async Task<IActionResult> Add()
        {
			var model = new AddBookViewModel
			{
				Authors = await _authorReposotory.selectableAuthorViewModelsAsync(),
				Publishers = await _publisherRepository.SelectablePublisherViewModelsAsync(),
				Categories = await _categoryRepository.GetCategoriesAsync()
			};

			return View(model);
        }
        public async Task<IActionResult> Create(AddBookViewModel addBookViewModel)
        {
			try
			{
				if (ModelState.IsValid)
				{
					Book book = _mapper.Map<Book>(addBookViewModel);

					book.Image = await _fileService.Upload(addBookViewModel.ImageFile, "images/Books");
					book.PublisherID = addBookViewModel.SelectedPublisher;

					await _bookRepository.AddBookAsync(book);

					book.BookAuthors = addBookViewModel.SelectedAuthors
					.Select(authorId => new BookAuthor
					{
						AuthorID = authorId,
						BookID = book.ID
					}).ToList();

					book.BookCategories = addBookViewModel.SelectedCategories
						.Select(categoryId => new BookCategories
						{
							BookID = book.ID,
							CategoryID = categoryId
						}).ToList();

					await _bookRepository.SaveChangesAsync();


					var books = await _bookRepository.GetAllBooksAsync();

					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				return Content($"{ex.Message}");
			}

			addBookViewModel.Authors = await _authorReposotory.selectableAuthorViewModelsAsync();
			addBookViewModel.Publishers = await _publisherRepository.SelectablePublisherViewModelsAsync();

            return View("Add", addBookViewModel);
        }
		public async Task<IActionResult> Delete(int id)
		{
			if (await _bookRepository.DeleteBook(id))
			{
				return RedirectToAction("Index");
			}
			return Content("Cant Delete!");
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			Book book = await _bookRepository.GetBookAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			IEnumerable<Author> authors = await _authorReposotory.GetAuthorsAsync();
			IEnumerable<Category> categories = await _categoryRepository.GetCategoriesAsync();
			IEnumerable<Publisher> publishers = await _publisherRepository.GetPublishersAsync();

			var viewModel = _mapper.Map<AddBookViewModel>(book);
			
			viewModel.Authors = _mapper.Map<IEnumerable<SelectableAuthorViewModel>>(authors);
			viewModel.Categories = categories;
			viewModel.Publishers = _mapper.Map<IEnumerable<SelectablePublisherViewModel>>(publishers);

			viewModel.SelectedAuthors = book.BookAuthors.Select(ba => ba.AuthorID).ToList();
			viewModel.SelectedCategories = book.BookCategories.Select(bc => bc.CategoryID).ToList();
			viewModel.SelectedPublisher = book.PublisherID;

			return View("Add", viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AddBookViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.ImageFile != null)
				{
					string newImageName = await _fileService.Upload(model.ImageFile, "images/Books");
					model.Image = newImageName;
				}
				Book book = _mapper.Map<Book>(model);
				book.Image = model.Image;
				book.BookAuthors = new List<BookAuthor>();
				foreach (var authorID in model.SelectedAuthors)
				{
					book.BookAuthors.Add(new BookAuthor() { AuthorID = authorID, BookID = book.ID });
				}
				book.BookCategories = new List<BookCategories>();
				foreach (var categoryID in model.SelectedCategories)
				{
					book.BookCategories.Add(new BookCategories() { CategoryID = categoryID, BookID = book.ID });
				}

				var result = await _bookRepository.UpdateBookAsync(book);
			
				if (result.Success)
				{
					if (model.ImageFile != null && !string.IsNullOrEmpty(result.OldImage))
					{
						_fileService.Delete(result.OldImage, "images/Books");
					}
					return RedirectToAction("Index");
				}
			}
			return BadRequest();
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			Book book = await _bookRepository.GetDetailsByIDAsync(id);
			if (book != null)
			{ 
				return View(book);
			}

			return RedirectToAction("Index","Books");
		}
	}
}