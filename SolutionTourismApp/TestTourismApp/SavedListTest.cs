using LibraryTourismApp;
using Microsoft.AspNetCore.Authorization;
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
    public class SavedListTest
    {
        private Mock<IAppUserRepo> _appUserRepo;
        private Mock<ISavedListRepo> _savedListRepo;
        private Mock<IAttractionRepo> _attractionRepo;

        private SavedListController _savedListController;

        public SavedListTest()
        {
            _appUserRepo = new Mock<IAppUserRepo>();
            _savedListRepo = new Mock<ISavedListRepo>();
            _attractionRepo = new Mock<IAttractionRepo>();

            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("");
            _savedListRepo.Setup(s => s.GetAllSavedListsForUser(It.IsAny<string>())).Returns(new List<SavedList>());
            //setting up repo methods called in CreateDropDowns

            _savedListController = new SavedListController(_savedListRepo.Object, _appUserRepo.Object, _attractionRepo.Object);
        }

        [Fact]
        public void ShouldGetSavedListDetails()
        {
            //testing the GetSavedListDetails method to ensure a list is found and belongs to the logged in user
            SavedList sl = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(sl.SavedListId)).Returns(sl);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");

            var result = _savedListController.GetSavedListDetails(sl.SavedListId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultModel = viewResult.Model as SavedList;

            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult);
            Assert.Equal(sl.SavedListId, resultModel.SavedListId);
            Assert.Equal(sl.Name, resultModel.Name);
            Assert.Equal(sl.Tourist.Id, resultModel.Tourist.Id);
        }

        [Fact]
        public void ShouldNOTGetSavedListDetails_SavedListNotFound()
        {
            //testing the GetSavedListDetails method to ensure the user is redirected if the saved list no longer exists
            SavedList sl = null;
            int savedListId = 1;

            _savedListRepo.Setup(s => s.FindSavedList(savedListId)).Returns(sl);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");

            var result = _savedListController.GetSavedListDetails(savedListId);

            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTGetSavedListDetails_SavedListDoesNotBelongToUser()
        {
            //testing the GetSavedListDetails method to ensure the user is redirected if the saved list does not belong to the logged in user
            SavedList sl = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(sl.SavedListId)).Returns(sl);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _savedListController.GetSavedListDetails(sl.SavedListId);

            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.IsType<RedirectToActionResult>(result);
        }


        [Fact]
        public void ShouldCreateSavedList()
        {
            //testing the CreateSavedList method to ensure a saved list can be added properly
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            CreateSavedListViewModel vm = new CreateSavedListViewModel
            {
                Name = "name",
                Description = "desc"
            };

            SavedList mockSavedList = null;
            int mockSavedListId = 1;

            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _savedListRepo.Setup(s => s.AddSavedList(It.IsAny<SavedList>())).Returns(mockSavedListId).Callback<SavedList>(newSavedList => mockSavedList = newSavedList);
            //setting up the repo methods called

            //act
            var result = _savedListController.CreateSavedList(vm);

            //assert
            _savedListRepo.Verify(s => s.AddSavedList(It.IsAny<SavedList>()), Times.Once(), "AddSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(mockSavedList);
            Assert.Equal(vm.Name, mockSavedList.Name);
            Assert.Equal(DateTime.Now.Date, mockSavedList.DateCreated.Date);
            Assert.Equal(tourist, mockSavedList.Tourist);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTCreateSavedList_AddSavedListError()
        {
            //testing the CreateSavedList to check that the model error is added if an issue occurs when adding the saved list
            //arrange
            Tourist tourist = new Tourist { Id = "T1", Email = "touristTest1@test.com" };
            CreateSavedListViewModel vm = new CreateSavedListViewModel
            {
                Name = "name",
                Description = "desc"
            };

            SavedList mockSavedList = null;
            int mockSavedListId = -1;

            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(tourist.Id);
            _appUserRepo.Setup(a => a.GetTouristById(tourist.Id)).Returns(tourist);
            _savedListRepo.Setup(s => s.AddSavedList(It.IsAny<SavedList>())).Returns(mockSavedListId).Callback<SavedList>(newSavedList => mockSavedList = newSavedList);
            //setting up the repo methods called

            //act
            var result = _savedListController.CreateSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _savedListRepo.Verify(s => s.AddSavedList(It.IsAny<SavedList>()), Times.Once(), "AddSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _appUserRepo.Verify(a => a.GetTouristById(It.IsAny<string>()), Times.Once(), "GetTouristById was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_savedListController.ModelState.ContainsKey("ErrorDatabaseError"), "Expected database error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }


        [Fact]
        public void ShouldEditSavedList_GetMethod()
        {
            //testing the get method to check that a saved list is being found using the id, and the logged in user is the creator of the saved list
            SavedList savedList = CreateMockData()[0];
            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(savedList.Tourist.Id);

            var result = _savedListController.EditSavedList(savedList.SavedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditSavedListViewModel vm = viewResult.Model as EditSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(savedList.SavedListId, vm.SavedListId);
            Assert.Equal(savedList.Name, vm.Name);
        }

        [Fact]
        public void ShouldNOTEditSavedList_GetMethod_NoSavedListFound()
        {
            //testing the get method to check that a saved list is NOT being found using the id (saved list does not exist)
            SavedList? savedList = null;
            int savedListId = 5;
            _savedListRepo.Setup(s => s.FindSavedList(savedListId)).Returns(savedList);

            var result = _savedListController.EditSavedList(savedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditSavedListViewModel vm = viewResult.Model as EditSavedListViewModel;

            _savedListRepo.Verify(a => a.FindSavedList(It.IsAny<int>()), Times.Once, "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_savedListController.ModelState.ContainsKey("NoSavedListFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTEditSavedList_GetMethod_TouristDoesNotOwnSavedList()
        {
            //testing the get method to check that a saved list is found but does NOT belong to the logged in user
            SavedList savedList = CreateMockData()[0];
            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _savedListController.EditSavedList(savedList.SavedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            EditSavedListViewModel vm = viewResult.Model as EditSavedListViewModel;

            _savedListRepo.Verify(a => a.FindSavedList(It.IsAny<int>()), Times.Once, "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_savedListController.ModelState.ContainsKey("SavedListDoesNotBelongToLoggedInUser"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldEditSavedList_PostMethod()
        {
            //testing the post method to check if the saved list is found and edited
            SavedList savedList = CreateMockData()[0];

            EditSavedListViewModel vm = new EditSavedListViewModel
            {
                SavedListId = savedList.SavedListId,
                Name = "new Name",
                DateCreated = DateTime.Now,
                Description = null
            };

            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns(savedList.Tourist.Id);
            _savedListRepo.Setup(r => r.UpdateSavedList(It.IsAny<SavedList>())).Callback<SavedList>(addedSavedList => savedList = addedSavedList);

            var result = _savedListController.EditSavedList(vm);

            _savedListRepo.Verify(a => a.FindSavedList(It.IsAny<int>()), Times.Once, "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            _savedListRepo.Verify(r => r.UpdateSavedList(It.IsAny<SavedList>()), Times.Once, "UpdateSavedList was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.Equal(vm.SavedListId, savedList.SavedListId);
            Assert.Equal(vm.Name, savedList.Name);
            Assert.Null(savedList.Description);
            Assert.NotEqual(vm.DateCreated, savedList.DateCreated);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTEditSavedList_PostMethod_SavedListNotFound()
        {
            //testing the post method to check if the saved list is NOT found, the list should not be edited and the view model should be returned with a model error
            SavedList savedList = null;

            EditSavedListViewModel vm = new EditSavedListViewModel
            {
                SavedListId = 1,
                Name = "new Name",
                DateCreated = DateTime.Now,
                Description = null
            };

            _savedListRepo.Setup(s => s.FindSavedList(vm.SavedListId)).Returns(savedList);

            var result = _savedListController.EditSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _savedListRepo.Verify(a => a.FindSavedList(It.IsAny<int>()), Times.Once, "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never, "GetLoggedInUserId was called.");
            _savedListRepo.Verify(r => r.UpdateSavedList(It.IsAny<SavedList>()), Times.Never, "UpdateSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_savedListController.ModelState.ContainsKey("NoSavedListFound"), "Expected error was not added.");
        }

        [Fact]
        public void ShouldNOTEditSavedList_PostMethod_TouristDoesNotOwnSavedList()
        {
            //testing the post method to check if the saved list is found, but does NOT belong to the logged in user, the list should not be edited and the view model should be returned with a model error
            SavedList savedList = CreateMockData()[0];

            EditSavedListViewModel vm = new EditSavedListViewModel
            {
                SavedListId = savedList.SavedListId,
                Name = "new Name",
                DateCreated = DateTime.Now,
                Description = null
            };

            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _savedListController.EditSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            _savedListRepo.Verify(a => a.FindSavedList(It.IsAny<int>()), Times.Once, "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once, "GetLoggedInUserId was not called once.");
            _savedListRepo.Verify(r => r.UpdateSavedList(It.IsAny<SavedList>()), Times.Never, "UpdateSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_savedListController.ModelState.ContainsKey("SavedListDoesNotBelongToLoggedInUser"), "Expected error was not added.");
        }


        [Fact]
        public void ShouldDeleteSavedList_GetMethod()
        {
            //testing the get method to check that a saved list is being found using the id and belongs to the logged in user
            SavedList savedList = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");

            var result = _savedListController.DeleteSavedList(savedList.SavedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteSavedListViewModel vm = viewResult.Model as DeleteSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times
                       
            Assert.Equal(savedList.SavedListId, vm.SavedListId);
            Assert.Equal(savedList.Name, vm.Name);
            Assert.Equal(savedList.AttractionList.Count, vm.CountOfAttractions);
        }

        [Fact]
        public void ShouldNOTDeleteSavedList_GetMethod_SavedListNotFound()
        {
            //testing the get method to check that a saved list is NOT being found using the id (saved list does not exist)
            int savedListId = 2;
            SavedList? savedList = null;

            _savedListRepo.Setup(s => s.FindSavedList(savedListId)).Returns(savedList);

            var result = _savedListController.DeleteSavedList(savedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteSavedListViewModel vm = viewResult.Model as DeleteSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_savedListController.ModelState.ContainsKey("NoSavedListFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTDeleteSavedList_GetMethod_SavedListDoesNotBelongToUser()
        {
            //testing the get method to check that the error message is provided if the saved list found does not belong to the logged in user
            SavedList savedList = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(savedList.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _savedListController.DeleteSavedList(savedList.SavedListId);
            var viewResult = Assert.IsType<ViewResult>(result);

            DeleteSavedListViewModel vm = viewResult.Model as DeleteSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.Null(vm.Name);
            Assert.True(_savedListController.ModelState.ContainsKey("SavedListDoesNotBelongToLoggedInUser"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldDeleteSavedList_PostMethod()
        {
            //testing the post method to check that a saved list is being found using the id and belongs to the logged in user
            DeleteSavedListViewModel vm = new DeleteSavedListViewModel
            {
                SavedListId = 1
            };

            SavedList savedList = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(vm.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T1");
            _savedListRepo.Setup(r => r.DeleteSavedList(It.IsAny<SavedList>()));

            var result = _savedListController.DeleteSavedList(vm);

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _savedListRepo.Verify(a => a.DeleteSavedList(It.IsAny<SavedList>()), Times.Once(), "DeleteSavedList was not called once.");
            //verifying the repo methods were called the appropriate number of times

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTDeleteSavedList_PostMethod_SavedListNotFound()
        {
            //testing the post method to check that a saved list is NOT being found using the id (saved list does not exist)
            DeleteSavedListViewModel vm = new DeleteSavedListViewModel
            {
                SavedListId = 1
            };

            SavedList? savedList = null;

            _savedListRepo.Setup(s => s.FindSavedList(vm.SavedListId)).Returns(savedList);

            var result = _savedListController.DeleteSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            vm = viewResult.Model as DeleteSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Never(), "GetLoggedInUserId was called.");
            _savedListRepo.Verify(a => a.DeleteSavedList(It.IsAny<SavedList>()), Times.Never(), "DeleteSavedList was called.");
            _savedListRepo.Verify(a => a.DeleteAttractionOnSavedList(It.IsAny<AttractionSavedList>()), Times.Never(), "DeleteAttractionOnSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_savedListController.ModelState.ContainsKey("NoSavedListFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldNOTDeleteSavedList_PostMethod_SavedListDoesNotBelongToUser()
        {
            //testing the post method to check that the error message is provided if the saved list found does not belong to the logged in user
            DeleteSavedListViewModel vm = new DeleteSavedListViewModel
            {
                SavedListId = 1
            };

            SavedList savedList = CreateMockData()[0];

            _savedListRepo.Setup(s => s.FindSavedList(vm.SavedListId)).Returns(savedList);
            _appUserRepo.Setup(a => a.GetLoggedInUserId()).Returns("T2");

            var result = _savedListController.DeleteSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            vm = viewResult.Model as DeleteSavedListViewModel;

            _savedListRepo.Verify(r => r.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _appUserRepo.Verify(a => a.GetLoggedInUserId(), Times.Once(), "GetLoggedInUserId was not called once.");
            _savedListRepo.Verify(a => a.DeleteSavedList(It.IsAny<SavedList>()), Times.Never(), "DeleteSavedList was called.");
            _savedListRepo.Verify(a => a.DeleteAttractionOnSavedList(It.IsAny<AttractionSavedList>()), Times.Never(), "DeleteAttractionOnSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(viewResult.Model);
            Assert.True(_savedListController.ModelState.ContainsKey("SavedListDoesNotBelongToLoggedInUser"), "Expected database error was not added.");
        }


        [Fact]
        public void ShouldAddAttractionToSavedList_GetMethod()
        {
            //testing the AddAttractionToSavedList get method to ensure an attraction exists before trying to add to a saved list
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
            var result = _savedListController.AddAttractionToSavedList(attraction.AttractionId);
            var viewResult = Assert.IsType<ViewResult>(result);

            AddAttractionToSavedListViewModel vm = viewResult.Model as AddAttractionToSavedListViewModel;

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");

            Assert.NotNull(vm);
            Assert.Equal(attraction.Name, vm.AttractionName);
            Assert.Equal(attraction.FullAddress, vm.AttractionAddress);
            Assert.Equal(attraction.AttractionId, vm.AttractionId);
        }

        [Fact]
        public void ShouldNOTAddAttractionToSavedList_GetMethod_AttractionNotFound()
        {
            //testing the AddAttractionToSavedList get method to check that it returns a model error when an attraction is not found
            //arrange
            Attraction attraction = null;

            _attractionRepo.Setup(a => a.FindAttraction(1)).Returns(attraction);

            //act
            var result = _savedListController.AddAttractionToSavedList(1);
            var viewResult = Assert.IsType<ViewResult>(result);

            AddAttractionToSavedListViewModel vm = viewResult.Model as AddAttractionToSavedListViewModel;

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");

            Assert.NotNull(vm);
            Assert.Null(vm.AttractionName);
            Assert.Null(vm.AttractionAddress);
            Assert.True(_savedListController.ModelState.ContainsKey("NoAttractionFound"), "Expected database error was not added.");
        }

        [Fact]
        public void ShouldAddAttractionToSavedList_PostMethod()
        {
            //testing the AddAttractionToSavedList post method to check that an attraction can be added to a saved list correctly
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

            SavedList savedList = CreateMockData()[0];
            int savedListId = savedList.SavedListId;

            AddAttractionToSavedListViewModel vm = new AddAttractionToSavedListViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                SavedListId = savedListId
            };

            AttractionSavedList mockAttractionSavedList = null;
            int mockAttractionSavedListId = 1;

            _attractionRepo.Setup(a => a.FindAttraction(attraction.AttractionId)).Returns(attraction);
            _savedListRepo.Setup(s => s.FindSavedList(savedListId)).Returns(savedList);
            _savedListRepo.Setup(a => a.AddAttractionToSavedList(It.IsAny<AttractionSavedList>())).Returns(mockAttractionSavedListId).Callback<AttractionSavedList>(addedasl => mockAttractionSavedList = addedasl);
            //setting up the repo methods called

            //act
            var result = _savedListController.AddAttractionToSavedList(vm);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once");
            _savedListRepo.Verify(r => r.AddAttractionToSavedList(It.IsAny<AttractionSavedList>()), Times.Once(), "AddAttractionToSavedList was not called once");
            //verifying the repo methods were called the appropriate number of times

            Assert.NotNull(mockAttractionSavedList);
            Assert.Equal(vm.AttractionId, mockAttractionSavedList.Attraction.AttractionId);
            Assert.Equal(vm.SavedListId, mockAttractionSavedList.SavedList.SavedListId);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ShouldNOTAddAttractionToSavedList_PostMethod_AttractionNotFound()
        {
            //testing the AddAttractionToSavedList post method to check that it returns a model error when an attraction is not found
            //arrange
            Attraction attraction = null;

            AddAttractionToSavedListViewModel vm = new AddAttractionToSavedListViewModel
            {
                AttractionId = 1,
                SavedListId = 1
            };

            _attractionRepo.Setup(a => a.FindAttraction(vm.AttractionId.Value)).Returns(attraction);
            //setting up the repo methods called

            //act
            var result = _savedListController.AddAttractionToSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Never(), "FindSavedList was called.");
            _savedListRepo.Verify(r => r.AddAttractionToSavedList(It.IsAny<AttractionSavedList>()), Times.Never(), "AddAttractionToSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_savedListController.ModelState.ContainsKey("NoAttractionFound"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldNOTAddAttractionToSavedList_PostMethod_SavedListNotFound()
        {
            //testing the AddAttractionToSavedList post method to check that it returns a model error when a saved list is not found
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

            SavedList? savedList = null;

            AddAttractionToSavedListViewModel vm = new AddAttractionToSavedListViewModel
            {
                AttractionId = 1,
                SavedListId = 1
            };

            _attractionRepo.Setup(a => a.FindAttraction(vm.AttractionId.Value)).Returns(attraction);
            _savedListRepo.Setup(s => s.FindSavedList(vm.SavedListId.Value)).Returns(savedList);
            //setting up the repo methods called

            //act
            var result = _savedListController.AddAttractionToSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once.");
            _savedListRepo.Verify(r => r.AddAttractionToSavedList(It.IsAny<AttractionSavedList>()), Times.Never(), "AddAttractionToSavedList was called.");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_savedListController.ModelState.ContainsKey("NoSavedListFound"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }

        [Fact]
        public void ShouldAddAttractionToSavedList_PostMethod_AddAttractionToSavedListError()
        {
            //testing the AddAttractionToSavedList post method to check that it returns a model error when an error occurs when creating the AttractionSavedList
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

            SavedList savedList = CreateMockData()[0];
            int savedListId = savedList.SavedListId;

            AddAttractionToSavedListViewModel vm = new AddAttractionToSavedListViewModel
            {
                AttractionId = attraction.AttractionId,
                AttractionName = attraction.Name,
                AttractionAddress = attraction.FullAddress,
                SavedListId = savedListId
            };

            AttractionSavedList mockAttractionSavedList = null;
            int mockAttractionSavedListId = -1;

            _attractionRepo.Setup(a => a.FindAttraction(attraction.AttractionId)).Returns(attraction);
            _savedListRepo.Setup(s => s.FindSavedList(savedListId)).Returns(savedList);
            _savedListRepo.Setup(a => a.AddAttractionToSavedList(It.IsAny<AttractionSavedList>())).Returns(mockAttractionSavedListId).Callback<AttractionSavedList>(addedasl => mockAttractionSavedList = addedasl);
            //setting up the repo methods called

            //act
            var result = _savedListController.AddAttractionToSavedList(vm);
            var viewResult = Assert.IsType<ViewResult>(result);

            //assert
            _attractionRepo.Verify(a => a.FindAttraction(It.IsAny<int>()), Times.Once(), "FindAttraction was not called once.");
            _savedListRepo.Verify(s => s.FindSavedList(It.IsAny<int>()), Times.Once(), "FindSavedList was not called once");
            _savedListRepo.Verify(r => r.AddAttractionToSavedList(It.IsAny<AttractionSavedList>()), Times.Once(), "AddAttractionToSavedList was not called once");
            //verifying the repo methods were called the appropriate number of times

            Assert.True(_savedListController.ModelState.ContainsKey("ErrorAttractionAlreadyOnList"), "Expected error was not added.");
            Assert.Equal(vm, viewResult.Model);
        }


        [Fact]
        public void ShouldDeleteAttractionSavedList()
        {
            //testing the method to check that an attraction on a saved list (AttractionSavedList) is being found using the id 
            SavedList savedList = CreateMockData()[0];
            AttractionSavedList attractionSavedList = savedList.AttractionList.Last();

            _savedListRepo.Setup(s => s.FindAttractionSavedList(attractionSavedList.AttractionSavedListId)).Returns(attractionSavedList);
            _savedListRepo.Setup(s => s.DeleteAttractionOnSavedList(It.IsAny<AttractionSavedList>()));

            var result = _savedListController.DeleteAttractionOnSavedList(attractionSavedList.AttractionSavedListId);

            _savedListRepo.Verify(r => r.FindAttractionSavedList(It.IsAny<int>()), Times.Once(), "FindAttractionSavedList was not called once.");
            _savedListRepo.Verify(a => a.DeleteAttractionOnSavedList(It.IsAny<AttractionSavedList>()), Times.Once(), "DeleteAttractionOnSavedList was not called once.");
            //verifying the repo methods were called the appropriate number of times

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("GetSavedListDetails", redirectResult.ActionName);
            Assert.Equal(savedList.SavedListId, redirectResult.RouteValues["savedListId"]);
        }


        public List<SavedList> CreateMockData()
        {
            List<SavedList> savedLists = new List<SavedList>();
            Tourist tourist = new Tourist { Id = "T1" };

            List<Attraction> attractions = new List<Attraction>
            {
                new Attraction { AttractionId = 1, Name = "Shop"},
                new Attraction { AttractionId = 2, Name = "Park"},
                new Attraction { AttractionId = 3, Name = "Pool"},
            };


            SavedList sl = new SavedList
            {
                SavedListId = 1,
                Tourist = tourist,
                Name = "list one",
                AttractionList = new List<AttractionSavedList>
                {
                    new AttractionSavedList { Attraction = attractions[0] }
                }
            };

            AttractionSavedList asl = new AttractionSavedList { Attraction = attractions[1], SavedList = sl };
            sl.AttractionList.Add(asl);
            savedLists.Add(sl);

            sl = new SavedList
            {
                SavedListId = 2,
                Tourist = tourist,
                Name = "list two",
                AttractionList = new List<AttractionSavedList>
                {
                    new AttractionSavedList { Attraction = attractions[2] },
                    new AttractionSavedList { Attraction = attractions[1] }
                }
            };
            savedLists.Add(sl);

            return savedLists;
        }
    }
}
