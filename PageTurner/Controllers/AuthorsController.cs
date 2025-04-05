using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Repositories;
using PageTurner.Repositories.Interfaces;
using PageTurner.Services.Interfaces;

namespace PageTurner.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
		public AuthorsController(IMapper mapper,IAuthorRepository authorRepository,IFileService fileService)
		{
			_authorRepository = authorRepository;
            _fileService = fileService;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index()
        {
            IEnumerable<Author> authors = await _authorRepository.GetAuthorsAsync();
            return View(authors);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            return View("CreateUpdate");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AddAuthorViewModel author)
        {
            if (ModelState.IsValid) 
            {
                Author authorToSave = new Author
                {
                    Name = author.Name,
                    Descreption = author.Descreption,
                    DateOfBirth = author.DateOfBirth,
                    Nationality = author.Nationality,
                    Gender = author.Gender
                };
                if (author.ImageFile != null && author.ImageFile.Length > 0)
                {
                   authorToSave.Image = await _fileService.Upload(author.ImageFile,"images/Authors");
                }
            bool result = await _authorRepository.AddAuthorAsync(authorToSave);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("CreateUpdate", author);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Author author = await _authorRepository.GetAuthorByIDAsync(id);

			if (author == null)
			{
				return NotFound();
			}

			bool result = await _authorRepository.DeleteAuthorAsync(author);

            if (result)
            {
                return RedirectToAction("Index");
            }

			ModelState.AddModelError("", "حدث خطأ أثناء حذف المؤلف.");
			return View("Index", await _authorRepository.GetAuthorsAsync());
		}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Author author = await _authorRepository.GetAuthorByIDAsync(id);
            AddAuthorViewModel viewModel = new AddAuthorViewModel
            {
                DateOfBirth = author.DateOfBirth,
                Descreption = author.Descreption,
                Gender = author.Gender,
                ID = author.ID,
                ImageFile = null,
                Name = author.Name,
                Nationality = author.Nationality
            };
            return View("CreateUpdate", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(AddAuthorViewModel model)
        {
			if (ModelState.IsValid)
			{
				if (model.ImageFile != null)
				{
					string newImageName = await _fileService.Upload(model.ImageFile, "images/Authors");
					model.Image = newImageName;
				}
				Author author = _mapper.Map<Author>(model);
				author.Image = model.Image;
				

				var result = await _authorRepository.UpdateAuthorAsync(author);

				if (result.result)
				{
					if (model.ImageFile != null && !string.IsNullOrEmpty(result.image))
					{
						_fileService.Delete(result.image, "images/Authors");
					}
					return RedirectToAction("Index");
				}
			}
			return BadRequest();
		}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
		{
			Author author = await _authorRepository.GetAuthorByIDWithBooksAsync(id);
			return View(author);
		}
	}
}
