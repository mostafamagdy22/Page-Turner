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
		public BooksController(IBookRepository bookRepository,IAuthorRepository authorRepository,IFileService fileService)
		{
			_bookRepository = bookRepository;
            _authorReposotory = authorRepository;
			_fileService = fileService;
		}
        [HttpGet]
		public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllBooksAsyncWithAuthor();
            return View(books);
        }
        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel { Authors = await _authorReposotory.GetAuthorsAsync()};

			return View(model);
        }
        public async Task<IActionResult> Create(AddBookViewModel addBookViewModel)
        {
			try
			{
				Book book = new Book();

				if (ModelState.IsValid)
				{
					book.ISBN = addBookViewModel.ISBN;
					book.Price = addBookViewModel.Price;
					book.Title = addBookViewModel.Title;
					book.Descreption = addBookViewModel.Descreption;
					book.BookAuthors = addBookViewModel.AuthorIds.Select(ai => new BookAuthor { AuthorID = ai, BookISBN = book.ISBN }).ToList();


					book.Image = await _fileService.Upload(addBookViewModel.ImageFile, "images");
					
					await _bookRepository.AddBookAsync(book);


					var books = await _bookRepository.GetAllBooksAsync();

					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				return Content($"{ex.Message}");
			}
			


            return View("Add", addBookViewModel);
        }
		public async Task<IActionResult> Delete(string isbn)
		{
			if (await _bookRepository.DeleteBook(isbn))
			{
				return RedirectToAction("Index");
			}
			return Content("Cant Delete!");
		}
		public async Task<IActionResult> Edit(string isbn)
		{

			return View();
		}
		public async Task<IActionResult> Details(string isbn)
		{
			Book book = await _bookRepository.GetByIsbnAsyncWithAuthors(isbn);
			if (book != null)
			{ 
				return View(book);
			}

			return RedirectToAction("Index","Books");
		}
	}
}