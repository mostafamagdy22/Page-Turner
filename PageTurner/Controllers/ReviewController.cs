using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PageTurner.Models;
using PageTurner.Models.ViewModels;
using PageTurner.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PageTurner.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewController(IMapper mapper, IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            Review review = await _reviewRepository.GetReviewByIdAsync(id);

			if (review == null)
			{
				return NotFound();
			}

			return View(review);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
		public async Task<IActionResult> Edit(Review review)
        {
            try
            {
                if (review == null)
                {
                    return NotFound();
                }

                bool result = await _reviewRepository.UpdateReviewAsync(review, review.ReviewContent);

                if (!result)
                {
                    return BadRequest();
                }

                return RedirectToAction("Index","Profile");
            }
            catch
            {
				// Handle the exception (e.g., log it)
				return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
			}
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            CreateReviewViewModel review = new CreateReviewViewModel() { BookID = id };
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
                    return RedirectToAction("Index", "Books");
                }
            }
            return View(review);
        }
        [HttpGet]
        public async Task<IActionResult> Reviews(int BookID)
        {
            try
            {
                var reviews = await _reviewRepository.GetReviewsByBookIdAsync(BookID);
                return View(reviews);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Review review = await _reviewRepository.GetReviewByIdAsync(id);

                if (review == null)
                {
                    return NotFound();
                }
				ReviewDetailsViewModel reviewDetails = _mapper.Map<ReviewDetailsViewModel>(review);
				return View(reviewDetails);
			}
            catch
            {
				// Handle the exception (e.g., log it)
				return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
			}
		}
    }
}