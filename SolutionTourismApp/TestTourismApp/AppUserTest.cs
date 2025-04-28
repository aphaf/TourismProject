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
    public class AppUserTest
    {
        private Mock<IAppUserRepo> _appUserRepo;

        private AppUserController _appUserController;

        public AppUserTest()
        {
            _appUserRepo = new Mock<IAppUserRepo>();

            _appUserController = new AppUserController(_appUserRepo.Object);
        }

        [Fact]
        public void ShouldFindMostRecentReview()
        {
            //testing FindMostRecentReview to ensure the most recent review is found for the tourist
            //arrange
            Tourist tourist = CreateMockData()[0];
            DateTime expectedDate = new DateTime(2025, 2, 1);
            int expectedReviewId = 2;

            //act
            Review? mostRecentReview = tourist.FindMostRecentReview();

            //assert
            Assert.NotNull(mostRecentReview);
            Assert.Equal(expectedDate.Date, mostRecentReview.DateCreated.Date);
            Assert.Equal(expectedReviewId, mostRecentReview.ReviewId);
        }

        [Fact]
        public void ShouldNOTFindMostRecentReview()
        {
            //testing FindMostRecentReview to ensure the most recent review is NOT found for the tourist, the tourist has no reviews
            //arrange
            Tourist tourist = CreateMockData()[1];

            //act
            Review? mostRecentReview = tourist.FindMostRecentReview();

            //assert
            Assert.Null(mostRecentReview);
        }


        [Fact]
        public void ShouldListAllTouristsAndFindMostRecentReview()
        {
            //arrange
            DateTime expectedDate = new DateTime(2025, 2, 1);
            int expectedReviewId = 2;
            int expectedCountOfTourists = 2;

            List<Tourist> tourists = CreateMockData();
            Tourist tourist = CreateMockData().First();

            ListAllTouristsViewModel vm = new ListAllTouristsViewModel
            {
                TouristId = tourist.Id
            };

            _appUserRepo.Setup(r => r.GetAllTourists()).Returns(tourists);

            //act
            _appUserController.ListAllTourists(vm);

            //assert
            Assert.Equal(expectedCountOfTourists, vm.ListOfTourists.Count);
            Assert.NotNull(vm.RecentReview);
            Assert.Equal(expectedReviewId, vm.RecentReview.ReviewId);
            Assert.Equal(expectedDate, vm.RecentReview.DateCreated);
        }

        public List<Tourist> CreateMockData()
        {
            List<Tourist> users = new List<Tourist>();


            Tourist tourist = new Tourist
            {
                Id = "T1",
                Email = "tourist1@test.com",
                ReviewsCreated = new List<Review>
                {
                    new Review { ReviewId = 1, DateCreated = new DateTime(2025, 1, 1)},
                    new Review { ReviewId = 2, DateCreated = new DateTime(2025, 2, 1)},
                    new Review { ReviewId = 3, DateCreated = new DateTime(2025, 1, 5)}
                }
            };

            users.Add(tourist);

            tourist = new Tourist
            {
                Id = "T2",
                Email = "tourist2@test.com"
            };

            users.Add(tourist);

            return users;
        }
    }
}
