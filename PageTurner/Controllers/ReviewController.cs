using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Security.Claims;

namespace PageTurner.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
		public ReviewController(IMapper mapper,IReviewRepository reviewRepository)
		{
            _reviewRepository = reviewRepository;
		    _mapper = mapper;
		}
		public IActionResult Index(int id)
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            CreateReviewViewModel review = new CreateReviewViewModel() { BookID = id};
			return View(review);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create(CreateReviewViewModel review)
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			review.UserID = userId;
            ModelState.Remove("UserID");
			if (ModelState.IsValid)
            {
				Review reviewModel = _mapper.Map<Review>(review);
				bool result = await _reviewRepository.CreateReviewAsync(reviewModel);
                if (result)
                {
                    return RedirectToAction("Index","Books");
                }
			}
            return View(review);
        }
    }
}
