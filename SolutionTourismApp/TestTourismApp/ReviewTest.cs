using LibraryTourismApp;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MvcTourismApp.Controllers;
using MvcTourismApp.Models;
using MvcTourismApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTourismApp
{
    public class ReviewTest
    {
        private Mock<IAttractionRepo> _attractionRepo;
        private Mock<IAppUserRepo> _appUserRepo;
        private Mock<IReviewRepo> _reviewRepo;

        private ReviewController _reviewController;

        public ReviewTest()
        {
            _attractionRepo = new Mock<IAttractionRepo>();
            _appUserRepo = new Mock<IAppUserRepo>();
            _reviewRepo = new Mock<IReviewRepo>();

            _appUserRepo.Setup(a => a.GetAllTourists()).Returns(new List<Tourist>());
            _attractionRepo.Setup(a => a.GetAllAttractions()).Returns(new List<Attraction>());
            //setting up drop down lists with empty lists to avoid null exception

            _reviewController = new ReviewController(_reviewRepo.Object, _appUserRepo.Object, _attractionRepo.Object);
        }

        //Class tests:
        [Fact]
        public void ShouldSearchReview_NoFilter()
        {
            //testing the search method with no filter
            //Arrange
            int expectedNumOfReviews = 5;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();
            int? inputAttractionId = null;
            string? inputTouristId = null;
            int? inputRating = null;
            DateTime? inputStartCreationDate = null;
            DateTime? inputEndCreationDate = null;

            //Act
            List<Review> searchResult = Review.SearchReviews(inputReviews, inputAttractionId, inputTouristId, inputRating, inputStartCreationDate, inputEndCreationDate);
            actualNumOfReviews = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
        }

        [Fact]
        public void ShouldSearchReview_ByAttractionId()
        {
            //testing the search method with no filter
            //Arrange
            int expectedNumOfReviews = 2;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();
            int? inputAttractionId = 1;
            string? inputTouristId = null;
            int? inputRating = null;
            DateTime? inputStartCreationDate = null;
            DateTime? inputEndCreationDate = null;

            //Act
            List<Review> searchResult = Review.SearchReviews(inputReviews, inputAttractionId, inputTouristId, inputRating, inputStartCreationDate, inputEndCreationDate);
            actualNumOfReviews = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
        }

        [Fact]
        public void ShouldSearchReview_ByTouristId()
        {
            //testing the search method with tourist id filter
            //Arrange
            int expectedNumOfReviews = 1;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();
            int? inputAttractionId = null;
            string? inputTouristId = "T1";
            int? inputRating = null;
            DateTime? inputStartCreationDate = null;
            DateTime? inputEndCreationDate = null;

            //Act
            List<Review> searchResult = Review.SearchReviews(inputReviews, inputAttractionId, inputTouristId, inputRating, inputStartCreationDate, inputEndCreationDate);
            actualNumOfReviews = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
        }

        [Fact]
        public void ShouldSearchReview_ByRating()
        {
            //testing the search method with rating filter
            //Arrange
            int expectedNumOfReviews = 3;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();
            int? inputAttractionId = null;
            string? inputTouristId = null;
            int? inputRating = 3;
            DateTime? inputStartCreationDate = null;
            DateTime? inputEndCreationDate = null;

            //Act
            List<Review> searchResult = Review.SearchReviews(inputReviews,inputAttractionId, inputTouristId, inputRating, inputStartCreationDate, inputEndCreationDate);
            actualNumOfReviews = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
        }

        [Fact]
        public void ShouldSearchReview_ByCreationDate()
        {
            //testing the search method with created date filter
            //Arrange
            int expectedNumOfReviews = 1;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();
            string? inputTouristId = null;
            int? inputAttractionId = null;
            int? inputRating = null;
            DateTime? inputStartCreationDate = new DateTime(2025, 1, 1);
            DateTime? inputEndCreationDate = new DateTime(2025, 1, 2);

            //Act
            List<Review> searchResult = Review.SearchReviews(inputReviews, inputAttractionId, inputTouristId, inputRating, inputStartCreationDate, inputEndCreationDate);
            actualNumOfReviews = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
        }

        [Fact]
        public void ShouldFindStarRatingIcons()
        {
            //testing the findstarrating to ensure the correct amount of star icons are returned based on the rating
            Review review = CreateMockData()[0];

            //expected - 2 stars
            string expectedStarDisplay = "<i class=\"fa fa-star fa_custom\"></i><i class=\"fa fa-star fa_custom\"></i><i class=\"fa fa-star-o fa_custom\"></i><i class=\"fa fa-star-o fa_custom\"></i><i class=\"fa fa-star-o fa_custom\"></i>";

            string starIcons = review.FindStarRatingIcons();

            Assert.Equal(expectedStarDisplay, starIcons);
        }


        //Controller tests:
        [Fact]
        public void ShouldSearchReviewsByController_GetMethod_ByAttraction()
        {
            //testing the SearchReviews get method to ensure reviews are being found for the attraction 
            //Arrange
            int attractionId = 1;
            int expectedNumOfReviews = 2;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData().Where(r => r.Attraction.AttractionId == attractionId).ToList();
            Attraction attraction = inputReviews.First().Attraction;

            _attractionRepo.Setup(a => a.FindAttraction(attractionId)).Returns(attraction);
            _reviewRepo.Setup(r => r.GetAllReviewsForAttraction(attractionId)).Returns(inputReviews);

            //Act
            var result = _reviewController.SearchReviews(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            SearchReviewsViewModel vm = viewResult.Model as SearchReviewsViewModel;
            actualNumOfReviews = vm.SearchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
            Assert.Equal(attraction.Name, vm.AttractionName);
        }

        [Fact]
        public void ShouldSearchReviewsByController_GetMethod()
        {
            //testing the SearchReviews get method to ensure reviews are being found 
            //Arrange
            int? attractionId = null;
            int expectedNumOfReviews = 5;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();

            _reviewRepo.Setup(r => r.GetAllReviews()).Returns(inputReviews);

            //Act
            var result = _reviewController.SearchReviews(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            SearchReviewsViewModel vm = viewResult.Model as SearchReviewsViewModel;
            actualNumOfReviews = vm.SearchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
            Assert.Null(vm.AttractionName);
        }

        [Fact]
        public void ShouldSearchReviewsByController_GetMethod_ByAttraction_NoAttractionFound()
        {
            //testing the SearchReviews get method to ensure the model error is provided when the attraction provided is NOT found 
            //Arrange
            int attractionId = 1;
            List<Review> inputReviews = CreateMockData();
            Attraction? attraction = null;

            _attractionRepo.Setup(a => a.FindAttraction(attractionId)).Returns(attraction);
            _reviewRepo.Setup(r => r.GetAllReviewsForAttraction(attractionId)).Returns(inputReviews);

            //Act
            var result = _reviewController.SearchReviews(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            SearchReviewsViewModel vm = viewResult.Model as SearchReviewsViewModel;

            //Assert
            Assert.NotNull(vm);
            Assert.Null(vm.AttractionName);
            Assert.True(_reviewController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldSearchReviewsByController_PostMethod_ByAttraction()
        {
            //testing the SearchReviews post method to ensure reviews are able to be filtered in the controller
            //Arrange
            int expectedNumOfReviews = 1;
            int actualNumOfReviews = 0;
            int attractionId = 1;
            List<Review> inputReviews = CreateMockData();
            Attraction attraction = inputReviews.First().Attraction;

            _attractionRepo.Setup(a => a.FindAttraction(attractionId)).Returns(attraction);
            _reviewRepo.Setup(r => r.GetAllReviews()).Returns(inputReviews);

            SearchReviewsViewModel vm = new SearchReviewsViewModel
            {
                AttractionId = attractionId,
                SearchRating = 2,
                SearchTouristId = "T1",
                SearchStartCreationDate = new DateTime(2025, 1, 1),
                SearchEndCreationDate = new DateTime(2025, 1, 1)
            };

            //Act
            _reviewController.SearchReviews(vm);
            actualNumOfReviews = vm.SearchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
            Assert.Equal(attraction.Name, vm.AttractionName);
        }

        [Fact]
        public void ShouldSearchReviewsByController_PostMethod()
        {
            //testing the SearchReviews post method to ensure reviews are able to be filtered in the controller
            //Arrange
            int expectedNumOfReviews = 1;
            int actualNumOfReviews = 0;
            List<Review> inputReviews = CreateMockData();

            _reviewRepo.Setup(r => r.GetAllReviews()).Returns(inputReviews);

            SearchReviewsViewModel vm = new SearchReviewsViewModel
            {
                SearchRating = 2,
                SearchTouristId = "T1",
                SearchStartCreationDate = new DateTime(2025, 1, 1),
                SearchEndCreationDate = new DateTime(2025, 1, 1)
            };

            //Act
            _reviewController.SearchReviews(vm);
            actualNumOfReviews = vm.SearchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfReviews, actualNumOfReviews);
            _attractionRepo.Verify(r => r.FindAttraction(It.IsAny<int>()), Times.Never(), "FindAttraction was called.");
        }

        [Fact]
        public void ShouldSearchReviewsByController_PostMethod_ByAttraction_AttractionNotFound()
        {
            //testing the SearchReviews post method to ensure reviews are able to be filtered in the controller
            //Arrange
            int attractionId = 1;
            List<Review> inputReviews = CreateMockData();
            Attraction attraction = null;

            _attractionRepo.Setup(a => a.FindAttraction(attractionId)).Returns(attraction);
            _reviewRepo.Setup(r => r.GetAllReviews()).Returns(inputReviews);

            SearchReviewsViewModel vm = new SearchReviewsViewModel
            {
                AttractionId = attractionId,
                SearchRating = 2,
                SearchTouristId = "T1",
                SearchStartCreationDate = new DateTime(2025, 1, 1),
                SearchEndCreationDate = new DateTime(2025, 1, 1)
            };

            //Act
            var result = _reviewController.SearchReviews(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            //Assert
            Assert.Null(vm.AttractionName);
            Assert.True(_reviewController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");

        }


        [Fact]
        public void ShouldAddReview_GetMethod()
        {
            //testing the AddReview get method to ensure an attraction exists before trying to add a review
            //arrange
            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Name",
                Street = "101 street",
                City = "City",
                State = "State",
                ZipCode = "100"
            };

            _attractionRepo.Setup(a => a.FindAttraction(attraction.AttractionId)).Returns(attraction);

            //act
            var result = _reviewController.AddReview(attraction.AttractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            AddReviewViewModel vm = viewResult.Model as AddReviewViewModel;

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");

            Assert.NotNull(vm);
            Assert.Equal(attraction.Name, vm.AttractionName);
            Assert.Equal(attraction.FullAddress, vm.AttractionAddress);
            Assert.Equal(attraction.AttractionId, vm.AttractionId);
        }

        [Fact]
        public void ShouldNOTAddReview_GetMethod_AttractionNotFound()
        {
            //testing the AddReview get method to check that it returns a model error when an attraction is not found
            //arrange
            Attraction attraction = null;

            _attractionRepo.Setup(a => a.FindAttraction(1)).Returns(attraction);

            //act
            var result = _reviewController.AddReview(1);
            var viewResult = Assert.IsType<ViewResult>(result);

            AddReviewViewModel vm = viewResult.Model as AddReviewViewModel;

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");

            Assert.NotNull(vm);
            Assert.Null(vm.AttractionName);
            Assert.Null(vm.AttractionAddress);
            Assert.True(_reviewController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");

        }

        [Fact]
        public void ShouldAddReview()
        {
            //testing the AddReview post method to check that a review can be added correctly
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Name",
                Street = "101 street",
                City = "City",
                State = "State",
                ZipCode = "100"
            };

            AddReviewViewModel vm = new AddReviewViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                Rating = 1,
                Title = "Title",
                Description = "Description",
                DateVisited = new DateTime(2025, 3, 1)
            };

            Review mockReview = null;
            int mockReviewId = 1;

            _attractionRepo.Setup(a => a.FindAttraction(attraction.AttractionId)).Returns(attraction);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _reviewRepo.Setup(a => a.AddReview(It.IsAny<Review>())).Returns(mockReviewId).Callback<Review>(addedReview => mockReview = addedReview);
            //setting up the repo methods called

            //act
            var result = _reviewController.AddReview(vm);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            _reviewRepo.Verify(r => r.AddReview(It.IsAny<Review>()), Times.Once(), "AddReview was not called once");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(mockReview);
            Assert.Equal(vm.AttractionId, mockReview.Attraction.AttractionId);
            Assert.Equal(vm.Rating, mockReview.Rating);
            Assert.Equal(tourist, mockReview.Tourist);
            Assert.Equal(DateTime.Now.Date, mockReview.DateCreated.Date);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTAddReview_AddReviewError()
        {
            //testing the AddReview post method to check that it returns a model error if something goes wrong when adding the review
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Name",
                Street = "101 street",
                City = "City",
                State = "State",
                ZipCode = "100"
            };

            AddReviewViewModel vm = new AddReviewViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                Rating = 1,
                Title = "Title",
                Description = "Description",
                DateVisited = new DateTime(2025, 3, 1)
            };

            Review mockReview = null;
            int mockReviewId = -1;

            _attractionRepo.Setup(a => a.FindAttraction(attraction.AttractionId)).Returns(attraction);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _reviewRepo.Setup(a => a.AddReview(It.IsAny<Review>())).Returns(mockReviewId);
            //setting up the repo methods called

            //act
            var result = _reviewController.AddReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            _reviewRepo.Verify(r => r.AddReview(It.IsAny<Review>()), Times.Once(), "AddReview was not called once");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_reviewController.ModelState.ContainsKey("ErrorDatabaseError"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldNOTAddReview_InvalidRating()
        {
            //testing the AddReview post method to check that it returns a model error when an invalid rating is given
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Name",
                Street = "101 street",
                City = "City",
                State = "State",
                ZipCode = "100"
            };

            AddReviewViewModel vm = new AddReviewViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                Rating = 10,
                Title = "Title",
                Description = "Description",
                DateVisited = new DateTime(2025, 3, 1)
            };

            Review mockReview = null;
            int mockReviewId = 1;

            //act
            var result = _reviewController.AddReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Never(), "FindAttraction was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Never(), "GetTouristById was called.");
            _reviewRepo.Verify(r => r.AddReview(It.IsAny<Review>()), Times.Never(), "AddReview was called");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_reviewController.ModelState.ContainsKey("InvalidRating"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldNOTAddReview_InvalidDateVisited()
        {
            //testing the AddReview post method to check that it returns a model error when an invalid date visited is given
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Name",
                Street = "101 street",
                City = "City",
                State = "State",
                ZipCode = "100"
            };

            AddReviewViewModel vm = new AddReviewViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                Rating = 1,
                Title = "Title",
                Description = "Description",
                DateVisited = DateTime.Now.AddDays(10)
            };

            Review mockReview = null;
            int mockReviewId = 1;

            //act
            var result = _reviewController.AddReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Never(), "FindAttraction was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Never(), "GetTouristById was called.");
            _reviewRepo.Verify(r => r.AddReview(It.IsAny<Review>()), Times.Never(), "AddReview was called");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_reviewController.ModelState.ContainsKey("InvalidDateVisited"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldNOTAddReview_AttractionNotFound()
        {
            //testing the AddReview post method to check that it returns a model error when an attraction is not found
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            Attraction attraction = null;

            AddReviewViewModel vm = new AddReviewViewModel
            {
                AttractionId = 1,
                Rating = 1,
                Title = "Title",
                Description = "Description",
                DateVisited = new DateTime(2025, 3, 1)
            };

            Review mockReview = null;
            int mockReviewId = 1;

            _attractionRepo.Setup(a => a.FindAttraction(vm.AttractionId)).Returns(attraction);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            //setting up the repo methods called

            //act
            var result = _reviewController.AddReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            _reviewRepo.Verify(r => r.AddReview(It.IsAny<Review>()), Times.Never(), "AddReview was called");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_reviewController.ModelState.ContainsKey("NoAttractionFound"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }


        [Fact]
        public void ShouldEditReview_GetMethod()
        {
            //testing the get method to check that a review is being found using the id, and the logged in user is the creator of the review
            Review review = CreateMockData()[0];
            _reviewRepo.Setup(r => r.FindReview(review.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(review.Tourist.Id);

            var result = _reviewController.EditReview(review.ReviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditReviewViewModel vm = viewResult.Model as EditReviewViewModel;

            Assert.Equal(review.ReviewId, vm.ReviewId);
            Assert.Equal(review.Title, vm.Title);
            Assert.Equal("Walmart", review.Attraction.Name);
        }

        [Fact]
        public void ShouldNOTEditReview_GetMethod_NoReviewFound()
        {
            //testing the get method to check that a review is NOT being found using the id (review does not exist)
            Review review = null;
            int reviewId = 1;
            _reviewRepo.Setup(r => r.FindReview(reviewId)).Returns(review);

            var result = _reviewController.EditReview(reviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditReviewViewModel vm = viewResult.Model as EditReviewViewModel;

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Once, "FindReview was not called Once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Title);
            Assert.True(_reviewController.ModelState.ContainsKey("NoReviewFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTEditReview_GetMethod_TouristDoesNotOwnReview()
        {
            //testing the get method to check that a review is found but does NOT belong to the logged in user
            Review review = CreateMockData()[0];
            _reviewRepo.Setup(r => r.FindReview(review.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _reviewController.EditReview(review.ReviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditReviewViewModel vm = viewResult.Model as EditReviewViewModel;

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Once, "FindReview was not called Once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called Once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Title);
            Assert.True(_reviewController.ModelState.ContainsKey("ReviewDoesNotBelongToLoggedInUser"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldEditReview_PostMethod()
        {
            //testing the post method to check if the review is found and edited
            Review review = CreateMockData()[0];

            EditReviewViewModel vm = new EditReviewViewModel
            {
                ReviewId = review.ReviewId,
                Title = "New title",
                Rating = 5,
                DateVisited = null,
                AttractionName = review.Attraction.Name,
                AttractionAddress = review.Attraction.FullAddress,
                DateCreated = review.DateCreated,
                TouristEmail = review.Tourist.Email,
                Description = null
            };

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(review.Tourist.Id);
            _reviewRepo.Setup(r => r.UpdateReview(It.IsAny<Review>())).Callback<Review>(addedReview => review = addedReview);
                        
            var result = _reviewController.EditReview(vm);

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Once, "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            _reviewRepo.Verify(r => r.UpdateReview(It.IsAny<Review>()), Times.Once, "UpdateReview was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(vm.ReviewId, review.ReviewId);
            Assert.Equal(vm.Title, review.Title);
            Assert.Null(review.Description);
            Assert.Equal(vm.Rating, review.Rating);
            Assert.Null(review.DateVisited);
            Assert.Equal(DateTime.Now.Date, review.LastEditedDate.Value.Date);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTEditReview_PostMethod_InvalidRating()
        {
            //testing the post method to check if the review is found, but the rating changed is invalid, the review should not be edited and the view model should be returned with a model error
            Review review = CreateMockData()[0];

            EditReviewViewModel vm = new EditReviewViewModel
            {
                ReviewId = review.ReviewId,
                Title = "New title",
                Rating = 10,
                DateVisited = null,
                AttractionName = review.Attraction.Name,
                AttractionAddress = review.Attraction.FullAddress,
                DateCreated = review.DateCreated,
                TouristEmail = review.Tourist.Email,
                Description = null
            };

            var result = _reviewController.EditReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Never, "FindReview was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            _reviewRepo.Verify(r => r.UpdateReview(It.IsAny<Review>()), Times.Never, "UpdateReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.NotEqual(vm.Title, review.Title);
            Assert.Null(review.LastEditedDate);
            Assert.True(_reviewController.ModelState.ContainsKey("InvalidRating"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldNOTEditReview_PostMethod_InvalidDateVisted()
        {
            //testing the post method to check if the review is found, but the date visited changed is invalid, the review should not be edited and the view model should be returned with a model error
            Review review = CreateMockData()[0];

            EditReviewViewModel vm = new EditReviewViewModel
            {
                ReviewId = review.ReviewId,
                Title = "New title",
                Rating = 5,
                DateVisited = DateTime.Now.AddDays(10),
                AttractionName = review.Attraction.Name,
                AttractionAddress = review.Attraction.FullAddress,
                DateCreated = review.DateCreated,
                TouristEmail = review.Tourist.Email,
                Description = null
            };

            var result = _reviewController.EditReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Never, "FindReview was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            _reviewRepo.Verify(r => r.UpdateReview(It.IsAny<Review>()), Times.Never, "UpdateReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.NotEqual(vm.Title, review.Title);
            Assert.Null(review.LastEditedDate);
            Assert.True(_reviewController.ModelState.ContainsKey("InvalidDateVisited"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldNOTEditReview_PostMethod_ReviewNotFound()
        {
            //testing the post method to check if the review is NOT found, the review should not be edited and the view model should be returned with a model error
            Review? review = null;

            EditReviewViewModel vm = new EditReviewViewModel
            {
                ReviewId = 1,
                Title = "New title",
                Rating = 5,
                DateVisited = null,
                DateCreated = DateTime.Now,
                Description = null
            };

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);

            var result = _reviewController.EditReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Once, "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            _reviewRepo.Verify(r => r.UpdateReview(It.IsAny<Review>()), Times.Never, "UpdateReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_reviewController.ModelState.ContainsKey("NoReviewFound"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldNOTEditReview_PostMethod_TouristDoesNotOwnReview()
        {
            //testing the post method to check if the review is found, but does not belong to the logged in user, the review should not be edited and the view model should be returned with a model error
            Review review = CreateMockData()[0];

            EditReviewViewModel vm = new EditReviewViewModel
            {
                ReviewId = review.ReviewId,
                Title = "New title",
                Rating = 5,
                DateVisited = null,
                AttractionName = review.Attraction.Name,
                AttractionAddress = review.Attraction.FullAddress,
                DateCreated = review.DateCreated,
                TouristEmail = review.Tourist.Email,
                Description = null
            };

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _reviewController.EditReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _reviewRepo.Verify(a => a.FindReview(It.IsAny<int>()), Times.Once, "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            _reviewRepo.Verify(r => r.UpdateReview(It.IsAny<Review>()), Times.Never, "UpdateReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_reviewController.ModelState.ContainsKey("ReviewDoesNotBelongToLoggedInUser"), "Expected error was not added.");
        }


        [Fact]
        public void ShouldDeleteReview_GetMethod()
        {
            //testing the get method to check that a review is being found using the id and belongs to the logged in user
            Review review = CreateMockData()[0];
            _reviewRepo.Setup(r => r.FindReview(review.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");

            var result = _reviewController.DeleteReview(review.ReviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteReviewViewModel vm = viewResult.Model as DeleteReviewViewModel;

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(review.ReviewId, vm.ReviewId);
            Assert.Equal(review.Title, vm.Title);
        }

        [Fact]
        public void ShouldNOTDeleteReview_GetMethod_ReviewNotFound()
        {
            //testing the get method to check that a review is NOT being found using the id (review does not exist)
            int reviewId = 2;
            Review? review = null;

            _reviewRepo.Setup(r => r.FindReview(reviewId)).Returns(review);

            var result = _reviewController.DeleteReview(reviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteReviewViewModel vm = viewResult.Model as DeleteReviewViewModel;

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Title);
            Assert.True(_reviewController.ModelState.ContainsKey("NoReviewFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTDeleteReview_GetMethod_ReviewDoesNotBelongToUser()
        {
            //testing the get method to check that a review the error message is provided if the review found does not belong to the logged in user
            Review review = CreateMockData()[0];
            _reviewRepo.Setup(r => r.FindReview(review.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _reviewController.DeleteReview(review.ReviewId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteReviewViewModel vm = viewResult.Model as DeleteReviewViewModel;

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Title);
            Assert.True(_reviewController.ModelState.ContainsKey("ReviewDoesNotBelongToLoggedInUser"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldDeleteReview_PostMethod()
        {  
            //testing the post method to check that a review is being found using the id and belongs to the logged in user
            DeleteReviewViewModel vm = new DeleteReviewViewModel
            {
                ReviewId = 1
            };

            Review review = CreateMockData()[0];

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");
            _reviewRepo.Setup(r => r.DeleteReview(It.IsAny<Review>()));

            var result = _reviewController.DeleteReview(vm);

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _reviewRepo.Verify(a => a.DeleteReview(It.IsAny<Review>()), Times.Once(), "DeleteReview was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTDeleteReview_PostMethod_ReviewNotFound()
        {
            //testing the post method to check that a review is NOT being found using the id (review does not exist)
            DeleteReviewViewModel vm = new DeleteReviewViewModel
            {
                ReviewId = 1
            };

            Review? review = null;

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);

            var result = _reviewController.DeleteReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            vm = viewResult.Model as DeleteReviewViewModel;

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            _reviewRepo.Verify(a => a.DeleteReview(It.IsAny<Review>()), Times.Never(), "DeleteReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_reviewController.ModelState.ContainsKey("NoReviewFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTDeleteReview_PostMethod_ReviewDoesNotBelongToUser()
        {
            //testing the post method to check that a review the error message is provided if the review found does not belong to the logged in user
            DeleteReviewViewModel vm = new DeleteReviewViewModel
            {
                ReviewId = 1
            };

            Review review = CreateMockData()[0];

            _reviewRepo.Setup(r => r.FindReview(vm.ReviewId)).Returns(review);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _reviewController.DeleteReview(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            vm = viewResult.Model as DeleteReviewViewModel;

            _reviewRepo.Verify(r => r.FindReview(It.IsAny<int>()), Times.Once(), "FindReview was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _reviewRepo.Verify(a => a.DeleteReview(It.IsAny<Review>()), Times.Never(), "DeleteReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_reviewController.ModelState.ContainsKey("ReviewDoesNotBelongToLoggedInUser"), "Expected database error was not added.");
        }

        public List<Review> CreateMockData()
        {
            List<Review> mockData = new List<Review>();

            List<Tourist> tourists = new List<Tourist>
            {
                new Tourist
                {
                    Id = "T1",
                    FirstName = "Tourist1",
                    Email = "Tourist1@test.com"
                },
                new Tourist
                {
                    Id = "T2",
                    FirstName = "Tourist2",
                    Email = "Tourist2@test.com"
                },
                new Tourist
                {
                    Id = "T3",
                    FirstName = "Tourist3",
                    Email = "Tourist3@test.com"
                },
            };

            List<Attraction> attractions = new List<Attraction>
            {
                new Attraction { Name = "Walmart", AttractionId = 1 },
                new Attraction { Name = "Mall", AttractionId = 2 },
                new Attraction { Name = "Park", AttractionId = 3 }
            };

            Review review = new Review
            {
                ReviewId = 1,
                Title = "title",
                Attraction = attractions[0],
                Rating = 2,
                Tourist = tourists[0],
                DateCreated = new DateTime(2025, 1, 1)
            };
            mockData.Add(review);

            review = new Review
            {
                Attraction = attractions[1],
                Rating = 2,
                Tourist = tourists[1],
                DateCreated = new DateTime(2025, 1, 3)
            };
            mockData.Add(review);

            review = new Review
            {
                Attraction = attractions[2],
                Rating = 3,
                Tourist = tourists[2],
                DateCreated = new DateTime(2025, 2, 1)
            };
            mockData.Add(review);

            review = new Review
            {
                Attraction = attractions[0],
                Rating = 4,
                Tourist = tourists[1],
                DateCreated = new DateTime(2025, 2, 1)
            };
            mockData.Add(review);


            review = new Review
            {
                Attraction = attractions[2],
                Rating = 5,
                Tourist = tourists[1],
                DateCreated = new DateTime(2025, 2, 1)
            };
            mockData.Add(review);

            return mockData;
        }
    }
}
