using LibraryTourismApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcTourismApp.Models;
using MvcTourismApp.ViewModels;

namespace MvcTourismApp.Controllers
{
    public class ReviewController : Controller
    {
        private IReviewRepo _reviewRepo;
        private IAttractionRepo _attractionRepo;
        private IAppUserRepo _appUserRepo;

        public ReviewController(IReviewRepo reviewRepo, IAppUserRepo appUserRepo, IAttractionRepo attractionRepo)
        {
            _reviewRepo = reviewRepo;
            _appUserRepo = appUserRepo;
            _attractionRepo = attractionRepo;
        }
        public void CreateDropDownLists()
        {
            //Drop down list for tourist emails
            List<Tourist> allTourists = _appUserRepo.GetAllTourists();
            ViewData["AllTourists"] = new SelectList(allTourists, "Id", "Email");

            //drop down for attractions and their address
            List<Attraction> allAttractions = _attractionRepo.GetAllAttractions();

            var allAttractionsForDDL = allAttractions.Select(a => new
            {
                AttractionId = a.AttractionId,
                AttractionDetails = $"{a.Name} - {a.FullAddress}"
            });

            ViewData["AllAttractions"] = new SelectList(allAttractionsForDDL, "AttractionId", "AttractionDetails");

        }

        //tourist actions
        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult AddReview(int attractionId)
        {
            AddReviewViewModel vm = new AddReviewViewModel();

            Attraction attraction = _attractionRepo.FindAttraction(attractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(vm);
            }

            vm.AttractionId = attraction.AttractionId;
            vm.AttractionName = attraction.Name;
            vm.AttractionAddress = attraction.FullAddress;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult AddReview(AddReviewViewModel vm)
        {
            if (vm.Rating <= 0 || vm.Rating > 5)
            {
                ModelState.AddModelError("InvalidRating", "Rating needs to be between 1 and 5.");
                return View(vm);
            }

            if (vm.DateVisited > DateTime.Now)
            {
                ModelState.AddModelError("InvalidDateVisited", "You cannot select a future date visited.");
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();
            Tourist tourist = _appUserRepo.GetTouristById(loggedInTouristId);

            Attraction attraction = _attractionRepo.FindAttraction(vm.AttractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(vm);
            }

            Review review = new Review(tourist, attraction, vm.Title, vm.Rating.Value, DateTime.Now, vm.Description, vm.DateVisited);

            int reviewId = _reviewRepo.AddReview(review);

            if (reviewId == -1)
            {
                ModelState.AddModelError("ErrorDatabaseError", "Error when adding to database.");
                CreateDropDownLists();
                return View(vm);
            }

            return RedirectToAction(nameof(SearchReviews), new { attractionId = vm.AttractionId });
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult EditReview(int reviewId)
        {
            EditReviewViewModel vm = new EditReviewViewModel();

            Review review = _reviewRepo.FindReview(reviewId);

            if (review == null)
            {
                ModelState.AddModelError("NoReviewFound", "The review no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != review.Tourist.Id)
            {
                ModelState.AddModelError("ReviewDoesNotBelongToLoggedInUser", "This review does not belong to you.");
                return View(vm);
            }

            //setting up the view model to hold all of the review's info
            vm = new EditReviewViewModel
            {
                ReviewId = review.ReviewId,
                Title = review.Title,
                Rating = review.Rating,
                DateVisited = review.DateVisited,
                DateCreated = review.DateCreated,
                Description = review.Description,
                AttractionName = review.Attraction.Name,
                AttractionAddress = review.Attraction.FullAddress,
                TouristEmail = review.Tourist.Email,
                TouristFullName = review.Tourist.FullName,
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult EditReview(EditReviewViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Rating <= 0 || vm.Rating > 5)
            {
                ModelState.AddModelError("InvalidRating", "Rating needs to be between 1 and 5.");
                return View(vm);
            }

            if (vm.DateVisited > DateTime.Now)
            {
                ModelState.AddModelError("InvalidDateVisited", "You cannot select a future date.");
                return View(vm);
            }

            Review review = _reviewRepo.FindReview(vm.ReviewId);

            if (review == null)
            {
                ModelState.AddModelError("NoReviewFound", "The review no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != review.Tourist.Id)
            {
                ModelState.AddModelError("ReviewDoesNotBelongToLoggedInUser", "This review does not belong to you.");
                return View(vm);
            }

            review.Title = vm.Title;
            review.Rating = vm.Rating.Value;
            review.DateVisited = vm.DateVisited == null ? null : vm.DateVisited.Value;
            review.Description = vm.Description;
            review.LastEditedDate = DateTime.Now;

            _reviewRepo.UpdateReview(review);

            return RedirectToAction("ListAllReviewsForUser");
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult DeleteReview(int reviewId)
        {
            DeleteReviewViewModel vm = new DeleteReviewViewModel();

            Review? review = _reviewRepo.FindReview(reviewId);

            if (review == null)
            {
                ModelState.AddModelError("NoReviewFound", "The review no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != review.Tourist.Id)
            {
                ModelState.AddModelError("ReviewDoesNotBelongToLoggedInUser", "This review does not belong to you.");
                return View(vm);
            }

            vm = new DeleteReviewViewModel
            {
                ReviewId = reviewId,
                Title = review.Title,
                Rating = review.Rating,
                RatingStarIcons = review.FindStarRatingIcons(),
                Description = review.Description,
                DateCreated = review.DateCreated,
                AttractionName = review.Attraction.Name,
                AttractionFullAddress = review.Attraction.FullAddress
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult DeleteReview(DeleteReviewViewModel vm)
        {
            Review? review = _reviewRepo.FindReview(vm.ReviewId);

            if (review == null)
            {
                ModelState.AddModelError("NoReviewFound", "The review no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != review.Tourist.Id)
            {
                ModelState.AddModelError("ReviewDoesNotBelongToLoggedInUser", "This review does not belong to you.");
                return View(vm);
            }

            _reviewRepo.DeleteReview(review);

            return RedirectToAction("ListAllReviewsForUser");
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult ListAllReviewsForUser()
        {
            string loggedInUserId = _appUserRepo.GetLoggedInUserId();

            List<Review> allReviewsByUser = _reviewRepo.GetAllReviewsForTourist(loggedInUserId);

            return View(allReviewsByUser);
        }



        //all user actions
        [HttpGet]
        public IActionResult SearchReviews(int? attractionId)
        {
            CreateDropDownLists();

            SearchReviewsViewModel vm = new SearchReviewsViewModel();

            if (attractionId != null)
            {                
                Attraction attraction = _attractionRepo.FindAttraction(attractionId.Value);
                
                if (attraction == null)
                {
                    ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                    return View(vm);
                }

                vm.AttractionId = attractionId;
                vm.AttractionName = attraction.Name;
                vm.AttractionFullAddress = attraction.FullAddress;
                vm.SearchResult = _reviewRepo.GetAllReviewsForAttraction(vm.AttractionId.Value);
            }
            else
            {
                vm.SearchResult = _reviewRepo.GetAllReviews();
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult SearchReviews(SearchReviewsViewModel vm)
        {
            CreateDropDownLists();

            List<Review> allReviews = new List<Review>();

            if (vm.AttractionId != null)
            {
                Attraction attraction = _attractionRepo.FindAttraction(vm.AttractionId.Value);

                if (attraction == null)
                {
                    ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                    return View(vm);
                }

                vm.AttractionName = attraction.Name;
                vm.AttractionFullAddress= attraction.FullAddress;
            }

            allReviews = _reviewRepo.GetAllReviews();

            vm.SearchResult = Review.SearchReviews(allReviews, vm.AttractionId, vm.SearchTouristId, vm.SearchRating, vm.SearchStartCreationDate, vm.SearchEndCreationDate);

            return View(vm);
        }


    }
}
