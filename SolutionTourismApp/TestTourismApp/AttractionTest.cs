using LibraryTourismApp;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    public class AttractionTest
    {
        private Mock<IAttractionRepo> _attractionRepo;
        private Mock<IAppUserRepo> _appUserRepo;
        private Mock<IReviewRepo> _reviewRepo;
        //private Mock<IAttractionEmailSender> _emailSender;

        private AttractionController _attractionController;

        public AttractionTest()
        {
            _attractionRepo = new Mock<IAttractionRepo>();
            _appUserRepo = new Mock<IAppUserRepo>();
            _reviewRepo = new Mock<IReviewRepo>();
            // _emailSender = new Mock<IAttractionEmailSender>();

            _attractionRepo.Setup(ar => ar.GetAllThemes()).Returns(new List<Theme>());
            _attractionRepo.Setup(ar => ar.GetAllAttractions()).Returns(new List<Attraction>());
            //including empty lists to avoid null exception from controller (from creating dropdown lists)

            _attractionController = new AttractionController(_attractionRepo.Object, _appUserRepo.Object, _reviewRepo.Object);
        }

        //Class tests:
        [Fact]
        public void ShouldSearchAttraction_NoFilter()
        {
            //checking that SearchAttractions returns the appropriate count of attractions with no filters
            //Arrange
            int expectedNumOfAttractions = 4;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = null;
            string? inputCity = null;
            string? inputCounty = null;
            int? inputThemeId = null;
            AttractionTypes? inputType = null;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldSearchAttraction_ById()
        {
            //checking that SearchAttractions returns the appropriate count of attractions by id (name on view) filter
            //Arrange
            int expectedNumOfAttractions = 1;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = 1;
            string? inputCity = null;
            string? inputCounty = null;
            int? inputThemeId = null;
            AttractionTypes? inputType = null;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldSearchAttraction_ByCity()
        {
            //checking that SearchAttractions returns the appropriate count of attractions by city filter
            //Arrange
            int expectedNumOfAttractions = 3;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = null;
            string? inputCity = "MorganTown";
            string? inputCounty = null;
            int? inputThemeId = null;
            AttractionTypes? inputType = null;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldSearchAttraction_ByCounty()
        {
            //checking that SearchAttractions returns the appropriate count of attractions by county filter
            //Arrange
            int expectedNumOfAttractions = 3;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = null;
            string? inputCity = null;
            string? inputCounty = "Mon";
            int? inputThemeId = null;
            AttractionTypes? inputType = null;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldSearchAttraction_ThemeId()
        {
            //checking that SearchAttractions returns the appropriate count of attractions by themeId filter, on the view the name will be displayed and pass an id
            //Arrange
            int expectedNumOfAttractions = 2;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = null;
            string? inputCity = null;
            string? inputCounty = null;
            int? inputThemeId = 1;
            AttractionTypes? inputType = null;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldSearchAttraction_ByType()
        {
            //checking that SearchAttractions returns the appropriate count of attractions by type filter
            //Arrange
            int expectedNumOfAttractions = 2;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();
            int? inputId = null;
            string? inputCity = null;
            string? inputCounty = null;
            int? inputThemeId = null;
            AttractionTypes? inputType = AttractionTypes.Lodging;

            //Act
            List<Attraction> searchResult = Attraction.SearchAttractions(inputAttractions, inputId, inputCity, inputCounty, inputThemeId, inputType);
            actualNumOfAttractions = searchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }

        [Fact]
        public void ShouldFindMostRecentReviews()
        {
            //checking that FindMostRecentReviews returns the reviews on an attraction sorted by most recent
            Attraction attraction = CreateMockData()[0];

            DateTime expectedReviewDate = new DateTime(2023, 4, 9);

            DateTime actualReviewDate = attraction.FindMostRecentReviews().First().DateCreated;

            Assert.Equal(expectedReviewDate, actualReviewDate);
        }
        [Fact]
        public void ShouldNOTFindMostRecentReviews_NoReviews()
        {
            //checking that FindMostRecentReviews does not return 'recent reviews' because the attraction has no reviews, ensuring the method does not break
            Attraction attraction = CreateMockData()[2];

            List<Review> recentReviews = attraction.FindMostRecentReviews();

            Assert.NotNull(recentReviews);
            Assert.Empty(recentReviews);
        }

        [Fact]
        public void ShouldFindAverageReviewRating()
        {
            //checking that FindAverageReviewRating returns the average rating based on the attractions reviews
            Attraction attraction = CreateMockData()[0];

            double expectedReviewRating = 2.6;

            double actualReviewRating = attraction.FindAverageReviewRating();

            Assert.Equal(expectedReviewRating, actualReviewRating);
        }

        [Fact]
        public void ShouldNOTFindAverageReviewRating_NoReviews()
        {
            //checking that FindAverageReviewRating does not return an average (return 0) because the attraction has no reviews, ensuring the method does not break
            Attraction attraction = CreateMockData()[1];

            double expectedReviewRating = 0;

            double actualReviewRating = attraction.FindAverageReviewRating();

            Assert.Equal(expectedReviewRating, actualReviewRating);
        }

        [Fact]
        public void ShouldFindAvgStarRatingIcons()
        {
            //checking that FindAvgStarRatingIcons returns the star icons based on the attractions average rating from reviews
            Attraction attraction = CreateMockData()[0];

            string expectedStarDisplay = "<i class=\"fa fa-star fa_custom\"></i><i class=\"fa fa-star fa_custom\"></i><i class=\"fa fa-star-half-o fa_custom\"></i><i class=\"fa fa-star-o fa_custom\"></i><i class=\"fa fa-star-o fa_custom\"></i>";

            string starIcons = attraction.FindAvgStarRatingIcons();

            Assert.Equal(expectedStarDisplay, starIcons);
        }

        [Fact]
        public void ShouldNOTFindAvgStarRatingIcons_NoReviews()
        {
            //checking that FindAvgStarRatingIcons does not return star icons based on the attractions average rating (zero) because the attraction has no reviews, ensuring the method does not break
            Attraction attraction = CreateMockData()[2];

            string expectedStarDisplay = "";
            string starIcons = attraction.FindAvgStarRatingIcons();

            Assert.Equal(expectedStarDisplay, starIcons);
        }


        //Controller tests:
        [Fact]
        public void ShouldSearchAttractionsByController()
        {
            //checking that SearchAttractions returns the appropriate count of attractions, when used in the controller
            //Arrange
            int expectedNumOfAttractions = 1;
            int actualNumOfAttractions = 0;
            List<Attraction> inputAttractions = CreateMockData();

            SearchAttractionsViewModel vm = new SearchAttractionsViewModel
            {
                SearchNameById = 1,
                SearchCity = "morgantown",
                SearchCounty = "mon",
                SearchThemeId = 1,
                SearchAttractionType = AttractionTypes.Activity
            };
            _attractionRepo.Setup(a => a.GetAllAttractions()).Returns(inputAttractions);

            //Act
            _attractionController.SearchAttractions(vm);
            actualNumOfAttractions = vm.SearchResult.Count;

            //Assert
            Assert.Equal(expectedNumOfAttractions, actualNumOfAttractions);
        }


        [Fact]
        public void ShouldAddAttraction_NoThemes()
        {
            //testing the AddAttraction method to ensure an attraction can be added properly
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            AddAttractionViewModel vm = new AddAttractionViewModel
            {
                Name = "Suburban Lanes",
                Description = "Bowling lanes.",
                TypeOfAttraction = AttractionTypes.Activity,
                Street = "Bowling street",
                City = "Morgantown",
                ZipCode = "26501",
                State = "West Virginia",
                County = "Mon",
                Website = "suburbanlanes.com"
            };

            Attraction mockAttraction = null;
            int mockAttractionId = 100;

            //_appUserRepo.Setup(a => a.GetAllModerators()).Returns(new List<Moderator>());
            //_emailSender.Setup(e => e.SendAddAttractionEmail("controllerAndMethod", "email", "subject", "message"));
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _attractionRepo.Setup(a => a.AddAttraction(It.IsAny<Attraction>())).Returns(mockAttractionId).Callback<Attraction>(addedAttraction => mockAttraction = addedAttraction);
            //setting up the repo methods called

            //act
            var result = _attractionController.AddAttraction(vm);

            //assert
            _attractionRepo.Verify(a => a.AddAttraction(It.IsAny<Attraction>()), Times.Once(), "AddAttraction was not called once.");
            _attractionRepo.Verify(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>()), Times.Never(), "AddAttractionTheme was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(mockAttraction);
            Assert.Equal(vm.Name, mockAttraction.Name);
            Assert.Equal(AttractionStatus.Pending, mockAttraction.Status);
            Assert.Equal(tourist, mockAttraction.TouristWhoAdded);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldAddAttraction_WithThemes()
        {
            //testing the AddAttraction method to ensure an attraction with themes can be added properly (adding the attraction plus the attractiontheme)
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            List<Theme> themes = new List<Theme>
            {
                new Theme { ThemeId = 1, Name = "Outdoors" },
                new Theme { ThemeId = 2, Name = "Water" }
            };
            AddAttractionViewModel vm = new AddAttractionViewModel
            {
                Name = "Suburban Lanes",
                Description = "Bowling lanes.",
                TypeOfAttraction = AttractionTypes.Activity,
                Street = "Bowling street",
                City = "Morgantown",
                ZipCode = "26501",
                State = "West Virginia",
                County = "Mon",
                Website = "suburbanlanes.com",
                ThemeIds = new List<int> { themes[0].ThemeId, themes[1].ThemeId }
            };

            AttractionTheme mockAttractionTheme = null;
            Attraction mockAttraction = null;
            int mockAttractionId = 100;

            //_appUserRepo.Setup(a => a.GetAllModerators()).Returns(new List<Moderator>());
            //_emailSender.Setup(e => e.SendAddAttractionEmail("controllerAndMethod", "email", "subject", "message"));
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _attractionRepo.Setup(a => a.FindTheme(themes[0].ThemeId)).Returns(themes[0]);
            _attractionRepo.Setup(a => a.FindTheme(themes[1].ThemeId)).Returns(themes[1]);
            _attractionRepo.Setup(a => a.AddAttraction(It.IsAny<Attraction>())).Returns(mockAttractionId).Callback<Attraction>(addedAttraction => mockAttraction = addedAttraction);
            _attractionRepo.Setup(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>())).Returns(themes[1].ThemeId).Callback<AttractionTheme>(addedAttractionTheme => mockAttractionTheme = addedAttractionTheme);
            //AddAttractionTheme set up will return the last one added, which is the water theme
            //setting up the repo methods called

            //act
            var result = _attractionController.AddAttraction(vm);

            //assert
            _attractionRepo.Verify(a => a.AddAttraction(It.IsAny<Attraction>()), Times.Once(), "AddAttraction was not called once.");
            _attractionRepo.Verify(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>()), Times.Exactly(2), "AddAttractionTheme was not called twice.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(mockAttraction);
            Assert.Equal(vm.Name, mockAttraction.Name);
            Assert.Equal(AttractionStatus.Pending, mockAttraction.Status);
            Assert.Equal(tourist, mockAttraction.TouristWhoAdded);
            Assert.NotNull(mockAttractionTheme);
            Assert.Equal(themes[1].Name, mockAttractionTheme.Theme.Name);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTAddAttraction_AddAttractionError()
        {
            //testing the AddAttraction method to check that the model error is added if an issue occurs when adding the attraction
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            AddAttractionViewModel vm = new AddAttractionViewModel
            {
                Name = "Suburban Lanes",
                Description = "Bowling lanes.",
                TypeOfAttraction = AttractionTypes.Activity,
                Street = "Bowling street",
                City = "Morgantown",
                ZipCode = "26501",
                State = "West Virginia",
                County = "Mon",
                Website = "suburbanlanes.com"
            };

            //_appUserRepo.Setup(a => a.GetAllModerators()).Returns(new List<Moderator>());
            //_emailSender.Setup(e => e.SendAddAttractionEmail("controllerAndMethod", "email", "subject", "message"));
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _attractionRepo.Setup(a => a.AddAttraction(It.IsAny<Attraction>())).Returns(-1);
            //setting up the repo methods called

            //act
            var result = _attractionController.AddAttraction(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.AddAttraction(It.IsAny<Attraction>()), Times.Once(), "AddAttraction was not called Once.");
            _attractionRepo.Verify(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>()), Times.Never(), "AddAttractionTheme was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_attractionController.ModelState.ContainsKey("ErrorNameAndStreetRepeated"), "Expected duplicate error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldNOTAddAttraction_AddAttractionThemeError()
        {
            //testing the AddAttraction method to check that the model error is added if an issue occurs when adding the AttractionTheme
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            AddAttractionViewModel vm = new AddAttractionViewModel
            {
                Name = "Suburban Lanes",
                Description = "Bowling lanes.",
                TypeOfAttraction = AttractionTypes.Activity,
                Street = "Bowling street",
                City = "Morgantown",
                ZipCode = "26501",
                State = "West Virginia",
                County = "Mon",
                Website = "suburbanlanes.com",
                ThemeIds = new List<int> { 1 }
            };

            //_appUserRepo.Setup(a => a.GetAllModerators()).Returns(new List<Moderator>());
            //_emailSender.Setup(e => e.SendAddAttractionEmail("controllerAndMethod", "email", "subject", "message"));
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _attractionRepo.Setup(a => a.AddAttraction(It.IsAny<Attraction>())).Returns(-1);
            _attractionRepo.Setup(a => a.FindTheme(vm.ThemeIds[0])).Returns(new Theme());
            _attractionRepo.Setup(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>())).Returns(-1);
            //setting up the repo methods called

            //act
            var result = _attractionController.AddAttraction(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.AddAttraction(It.IsAny<Attraction>()), Times.Once(), "AddAttraction was not called Once.");
            _attractionRepo.Verify(a => a.AddAttractionTheme(It.IsAny<AttractionTheme>()), Times.Once(), "AddAttractionTheme was not called Once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_attractionController.ModelState.ContainsKey("ErrorDatabaseError"), "Expected database error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }


        [Fact]
        public void ShouldEditAttraction_GetMethod()
        {
            //testing the get method to check that an attraction is being found using the id
            Attraction attraction = CreateMockData()[0];
            _attractionRepo.Setup(m => m.FindAttraction(attraction.AttractionId)).Returns(attraction);

            var result = _attractionController.EditAttraction(attraction.AttractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditAttractionViewModel vm = viewResult.Model as EditAttractionViewModel;

            Assert.Equal(attraction.AttractionId, vm.AttractionId);
            Assert.Equal(attraction.Name, vm.Name);
        }

        [Fact]
        public void ShouldNOTEditAttraction_GetMethod_AttractionNotFound()
        {
            //testing the get method to check that an attraction is NOT being found using the id (attraction does not exist)
            int attractionId = 2;
            Attraction attraction = null;

            _attractionRepo.Setup(m => m.FindAttraction(attractionId)).Returns(attraction);

            var result = _attractionController.EditAttraction(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditAttractionViewModel vm = viewResult.Model as EditAttractionViewModel;

            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once, "FindAttraction was not called Once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_attractionController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldEditAttraction_PostMethod()
        {
            //testing the post edit attraction method to ensure an edited view model updates the attraction & checking a 'pending' status changes the attraction's decision information
            EditAttractionViewModel evm = new EditAttractionViewModel
            {
                AttractionId = 1,
                Name = "attraction1",
                Description = "desc",
                TypeOfAttraction = AttractionTypes.Activity,
                Status = AttractionStatus.Pending,
                Street = "street 101",
                City = "city",
                ZipCode = "12345",
                State = "state",
                County = "county",
                Website = "website",
                TouristWhoAdded = new Tourist { FirstName = "tourist" },
                ModeratorWhoMadeDecision = null,
                DateDecisionMade = DateTime.Now
            };

            Attraction attraction = new Attraction { AttractionId = evm.AttractionId, DateDecisionMade = DateTime.Now };

            _attractionRepo.Setup(m => m.FindAttraction(evm.AttractionId)).Returns(attraction);
            _attractionRepo.Setup(m => m.UpdateAttraction(It.IsAny<Attraction>())).Callback<Attraction>(modifiedAttraction => attraction = modifiedAttraction);

            var result = _attractionController.EditAttraction(evm);

            _attractionRepo.Verify(m => m.UpdateAttraction(It.IsAny<Attraction>()), Times.Once(), "UpdateAttraction not called Once");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            _appUserRepo.Verify(a => a.GetModeratorById(It.IsAny<string>()), Times.Never, "GetModeratorById was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(evm.AttractionId, attraction.AttractionId);
            Assert.Equal(evm.Name, attraction.Name);
            Assert.Null(attraction.DateDecisionMade);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldEditAttraction_PostMethod_UpdatedStatus()
        {
            //testing the post edit attraction method to ensure an edited view model updates the attraction - and checking that the updated status changes the attraction's decision info
            Moderator moderator = new Moderator { Email = "moderator1@test.com" };
            EditAttractionViewModel evm = new EditAttractionViewModel
            {
                AttractionId = 1,
                Name = "attraction1",
                Description = "desc",
                TypeOfAttraction = AttractionTypes.Activity,
                Status = AttractionStatus.Denied,
                Street = "street 101",
                City = "city",
                ZipCode = "12345",
                State = "state",
                County = "county",
                Website = "website",
                TouristWhoAdded = new Tourist { FirstName = "tourist" },
                ModeratorWhoMadeDecision = null,
                DateDecisionMade = DateTime.Now.AddDays(-1)
            };

            Attraction attraction = new Attraction { AttractionId = evm.AttractionId, Status = AttractionStatus.Approved };

            _attractionRepo.Setup(m => m.FindAttraction(evm.AttractionId)).Returns(attraction);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(moderator.Id);
            _appUserRepo.Setup(a => a.GetModeratorById(moderator.Id)).Returns(moderator);
            _attractionRepo.Setup(m => m.UpdateAttraction(It.IsAny<Attraction>())).Callback<Attraction>(modifiedAttraction => attraction = modifiedAttraction);

            var result = _attractionController.EditAttraction(evm);

            _attractionRepo.Verify(m => m.UpdateAttraction(It.IsAny<Attraction>()), Times.Once(), "UpdateAttraction not called Once");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetModeratorById(It.IsAny<string>()), Times.Once, "GetModeratorById was not called Once.");
            //verifying the repo methods were called the appropriate number of times


            Assert.Equal(evm.AttractionId, attraction.AttractionId);
            Assert.Equal(evm.Name, attraction.Name);
            Assert.Equal(DateTime.Now.Date, attraction.DateDecisionMade.Value.Date);
            Assert.Equal(moderator, attraction.ModeratorWhoMadeDecision);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTEditAttraction_PostMethod_AttractionNotFound()
        {
            //testing the post edit attraction method to check that an attraction has to exist before being edited
            EditAttractionViewModel evm = new EditAttractionViewModel
            {
                AttractionId = 1,
                Name = "attraction1",
                Description = "desc",
                TypeOfAttraction = AttractionTypes.Activity,
                Status = AttractionStatus.Pending,
                Street = "street 101",
                City = "city",
                ZipCode = "12345",
                State = "state",
                County = "county",
                Website = "website",
                TouristWhoAdded = new Tourist { FirstName = "tourist" },
                ModeratorWhoMadeDecision = null,
                DateDecisionMade = DateTime.Now
            };

            Attraction attraction = null;

            _attractionRepo.Setup(m => m.FindAttraction(evm.AttractionId)).Returns(attraction);

            var result = _attractionController.EditAttraction(evm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called Once.");
            _attractionRepo.Verify(m => m.UpdateAttraction(It.IsAny<Attraction>()), Times.Never(), "UpdateAttraction was called.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            _appUserRepo.Verify(a => a.GetModeratorById(It.IsAny<string>()), Times.Never, "GetModeratorById was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Null(attraction);
            Assert.Null(viewResult.Model);
            Assert.True(_attractionController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }


        [Fact]
        public void ShouldDeleteAttraction_GetMethod()
        {
            //testing the get method to check that an attraction is being found using the id
            Attraction attraction = CreateMockData()[0];
            _attractionRepo.Setup(m => m.FindAttraction(attraction.AttractionId)).Returns(attraction);

            var result = _attractionController.DeleteAttraction(attraction.AttractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteAttractionViewModel vm = viewResult.Model as DeleteAttractionViewModel;

            Assert.Equal(attraction.AttractionId, vm.AttractionId);
            Assert.Equal(attraction.Name, vm.Name);
        }

        [Fact]
        public void ShouldNOTDeleteAttraction_GetMethod_AttractionNotFound()
        {
            //testing the get method to check that an attraction is NOT being found using the id (attraction does not exist)
            int attractionId = 2;
            Attraction attraction = null;

            _attractionRepo.Setup(m => m.FindAttraction(attractionId)).Returns(attraction);

            var result = _attractionController.DeleteAttraction(attractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteAttractionViewModel vm = viewResult.Model as DeleteAttractionViewModel;

            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once, "FindAttraction was not called Once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_attractionController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldDeleteAttraction_PostMethod()
        {
            //testing the post method to check that an attraction is being found using the id
            DeleteAttractionViewModel vm = new DeleteAttractionViewModel
            {
                AttractionId = 2
            };

            Attraction attraction = CreateMockData()[1];

            _attractionRepo.Setup(m => m.FindAttraction(vm.AttractionId)).Returns(attraction);
            _attractionRepo.Setup(a => a.DeleteAttraction(It.IsAny<Attraction>()));

            /* not needed unless manually deleting
            _attractionRepo.Setup(a => a.DeleteAttractionTheme(It.IsAny<AttractionTheme>()))
                .Callback<AttractionTheme>(theme => attraction.Themes.Remove(theme));

            _reviewRepo.Setup(r => r.DeleteReview(It.IsAny<Review>()))
                .Callback<Review>(review => attraction.Reviews.Remove(review));
            */

            var result = _attractionController.DeleteAttraction(vm);

            _attractionRepo.Verify(a => a.DeleteAttraction(It.IsAny<Attraction>()), Times.Once(), "DeleteAttraction was not called Once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTDeleteAttraction_PostMethod_AttractionNotFound()
        {
            //testing the post method to check that an attraction is NOT being found using the id (attraction does not exist)
            DeleteAttractionViewModel vm = new DeleteAttractionViewModel
            {
                AttractionId = 1,
                Name = "attraction1",
                Description = "desc"
            };

            Attraction? attraction = null;

            _attractionRepo.Setup(m => m.FindAttraction(vm.AttractionId)).Returns(attraction);

            var result = _attractionController.DeleteAttraction(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            vm = viewResult.Model as DeleteAttractionViewModel;

            _attractionRepo.Verify(a => a.DeleteAttraction(It.IsAny<Attraction>()), Times.Never(), "DeleteAttraction was called.");
            _attractionRepo.Verify(a => a.DeleteAttractionTheme(It.IsAny<AttractionTheme>()), Times.Never, "DeleteAttractionTheme was called.");
            _reviewRepo.Verify(a => a.DeleteReview(It.IsAny<Review>()), Times.Never(), "DeleteReview was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_attractionController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }


        [Fact]
        public void ShouldMakeDecisionsOnAttractions()
        {
            //testing the MakeDecisionsOnAttractions method to ensure decisions are made and handled correctly
            MakeDecisionsOnAttractionsViewModel vm = new MakeDecisionsOnAttractionsViewModel
            {
                AttractionIds = new List<int> { 1, 2, 3 },
                Decisions = new List<string> { null, "0", "1" }//0 is pending, 1 is approved, 2 is denied
            };

            Attraction attraction = null;
            Moderator moderator = new Moderator { Email = "moderator1@test.com" };

            List<Attraction> mockData = CreateMockData();

            _attractionRepo.Setup(m => m.FindAttraction(vm.AttractionIds[0])).Returns(mockData[0]);
            _attractionRepo.Setup(m => m.FindAttraction(vm.AttractionIds[1])).Returns(mockData[1]);
            _attractionRepo.Setup(m => m.FindAttraction(vm.AttractionIds[2])).Returns(mockData[2]);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(moderator.Id);
            _appUserRepo.Setup(a => a.GetModeratorById(moderator.Id)).Returns(moderator);
            _attractionRepo.Setup(m => m.UpdateAttraction(It.IsAny<Attraction>())).Callback<Attraction>(modifiedAttraction => attraction = modifiedAttraction);

            var result = _attractionController.MakeDecisionsOnAttractions(vm);

            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called Once");
            _attractionRepo.Verify(m => m.UpdateAttraction(It.IsAny<Attraction>()), Times.Once(), "UpdateAttraction not called Once");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetModeratorById(It.IsAny<string>()), Times.Once, "GetModeratorById was not called Once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(AttractionStatus.Approved, attraction.Status);
            Assert.Equal(moderator, attraction.ModeratorWhoMadeDecision);
            Assert.Equal(DateTime.Now.Date, attraction.DateDecisionMade.Value.Date);
            Assert.IsType<RedirectToActionResult>(result);
        }


        public List<Attraction> CreateMockData()
        {
            List<Attraction> mockData = new List<Attraction>();

            List<Theme> themes = new List<Theme>
            {
                new Theme { Name = "Outdoors", ThemeId = 1 },
                new Theme { Name = "Food and Drink", ThemeId = 2 },
                new Theme { Name = "Arts and Culture", ThemeId = 3 },
                new Theme { Name = "By the Water", ThemeId = 4 }
            };

            Attraction attraction = new Attraction
            {
                AttractionId = 1,
                Name = "Krogers",
                Description = "A grocery store.",
                City = "Morgantown",
                County = "Monongalia ",
                Street = "Suncrest Town Centre Drive",
                Themes = new List<AttractionTheme> {
                    new AttractionTheme { Theme = themes[0] }, //outdoors
                    new AttractionTheme { Theme = themes[1] } //food & drink
                },
                TypeOfAttraction = AttractionTypes.Activity,
                Status = AttractionStatus.Approved,
                Reviews = new List<Review>
                {
                    new Review { DateCreated = new DateTime(2020, 1, 1), Rating = 1},
                    new Review { DateCreated = new DateTime(2021, 1, 3), Rating = 2},
                    new Review { DateCreated = new DateTime(2023, 3, 1), Rating = 3},
                    new Review { DateCreated = new DateTime(2023, 4, 9), Rating = 2 },
                    new Review { DateCreated = new DateTime(2020, 12, 25), Rating = 5 }
                }
            };
            mockData.Add(attraction);

            attraction = new Attraction
            {
                AttractionId = 2,
                Name = "Restaurant A",
                Description = "Dining.",
                City = "Moundsville",
                County = "Marshall",
                Street = "Street 101",
                Themes = new List<AttractionTheme> {
                    new AttractionTheme { Theme = themes[0] }, //outdoors
                    new AttractionTheme { Theme = themes[2] } //art
                },
                TypeOfAttraction = AttractionTypes.Restaurant,
                Status = AttractionStatus.Pending,
                Reviews = new List<Review>
                {
                    new Review { ReviewId = 1 },
                    new Review { ReviewId = 2 }
                }
            };
            mockData.Add(attraction);

            attraction = new Attraction
            {
                AttractionId = 3,
                Name = "Walmart University Town Centre",
                Description = "A big grocery store.",
                City = "Morgantown",
                County = "Monongalia",
                Street = "University Town Centre Drive",
                Themes = new List<AttractionTheme> {
                    new AttractionTheme {Theme = themes[0]}, //outdoors
                    new AttractionTheme {Theme = themes[3]} //water
                },
                TypeOfAttraction = AttractionTypes.Activity,
                Status = AttractionStatus.Approved
            };
            mockData.Add(attraction);

            attraction = new Attraction
            {
                AttractionId = 4,
                Name = "A hotel",
                Description = "Stay for the night.",
                City = "Pittsburgh",
                County = "Allegheny",
                Street = "Example street 101",
                Themes = new List<AttractionTheme> {
                    new AttractionTheme {Theme = themes[3]} //water
                },
                TypeOfAttraction = AttractionTypes.Lodging,
                Status = AttractionStatus.Denied
            };
            mockData.Add(attraction);

            attraction = new Attraction
            {
                AttractionId = 5,
                Name = "A hotel",
                Description = "Sleep here.",
                City = "Pittsburgh",
                County = "Allegheny",
                Street = "Example street 102",
                TypeOfAttraction = AttractionTypes.Lodging,
                Status = AttractionStatus.Approved
            };
            mockData.Add(attraction);

            attraction = new Attraction
            {
                AttractionId = 6,
                Name = "A nice hotel",
                Description = "Sleep here.",
                City = "Morgantown",
                County = "Monongalia",
                Street = "Example street 103",
                TypeOfAttraction = AttractionTypes.Lodging,
                Status = AttractionStatus.Approved
            };
            mockData.Add(attraction);

            return mockData;
        }
    }
}
