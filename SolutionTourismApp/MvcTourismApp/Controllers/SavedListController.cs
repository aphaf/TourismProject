using LibraryTourismApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcTourismApp.Models;
using MvcTourismApp.ViewModels;

namespace MvcTourismApp.Controllers
{
    public class SavedListController : Controller
    {
        private ISavedListRepo _savedListRepo;
        private IAppUserRepo _appUserRepo;
        private IAttractionRepo _attractionRepo;

        public SavedListController(ISavedListRepo savedListRepo, IAppUserRepo appUserRepo, IAttractionRepo attractionRepo)
        {
            _savedListRepo = savedListRepo;
            _appUserRepo = appUserRepo;
            _attractionRepo = attractionRepo;
        }

        public void CreateDropDownLists()
        {
            //getting drop down for saved lists of logged in user
            string userId = _appUserRepo.GetLoggedInUserId();
            List<SavedList> allSavedLists = _savedListRepo.GetAllSavedListsForUser(userId);
            ViewData["AllSavedLists"] = new SelectList(allSavedLists, "SavedListId", "Name");
        }

        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult ListAllSavedLists(string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            //find all saved lists for logged in user
            string userId = _appUserRepo.GetLoggedInUserId();

            List<SavedList> allSavedLists = _savedListRepo.GetAllSavedListsForUser(userId);

            return View(allSavedLists);
        }

        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult GetSavedListDetails(int savedListId)
        {
            SavedList savedList = _savedListRepo.FindSavedList(savedListId);

            if (savedList == null)
            {
                return RedirectToAction(nameof(ListAllSavedLists), new { errorMessage = "This saved list no longer exists." });
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != savedList.Tourist.Id)
            {
                return RedirectToAction(nameof(ListAllSavedLists), new { errorMessage = "That saved list does not belong to you." });
            }

            return View(savedList);
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult CreateSavedList()
        {
            CreateSavedListViewModel vm = new CreateSavedListViewModel();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult CreateSavedList(CreateSavedListViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();
            Tourist tourist = _appUserRepo.GetTouristById(loggedInTouristId);

            SavedList savedList = new SavedList(tourist, vm.Name, vm.Description);

            int savedListId = _savedListRepo.AddSavedList(savedList);

            if (savedListId == -1)
            {
                ModelState.AddModelError("ErrorDatabaseError", "Error when adding to the database.");

                return View(vm);
            }

            return RedirectToAction(nameof(GetSavedListDetails), new { savedListId });
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult EditSavedList(int savedListId)
        {
            EditSavedListViewModel vm = new EditSavedListViewModel();

            SavedList? savedList = _savedListRepo.FindSavedList(savedListId);

            if (savedList == null)
            {
                ModelState.AddModelError("NoSavedListFound", "This saved list no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != savedList.Tourist.Id)
            {
                ModelState.AddModelError("SavedListDoesNotBelongToLoggedInUser", "This saved list does not belong to you.");
                return View(vm);
            }

            vm.SavedListId = savedList.SavedListId;
            vm.Name = savedList.Name;
            vm.Description = savedList.Description;
            vm.DateCreated = savedList.DateCreated;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult EditSavedList(EditSavedListViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                CreateDropDownLists();
                return View(vm);
            }

            SavedList? savedList = _savedListRepo.FindSavedList(vm.SavedListId);

            if (savedList == null)
            {
                ModelState.AddModelError("NoSavedListFound", "This saved list no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != savedList.Tourist.Id)
            {
                ModelState.AddModelError("SavedListDoesNotBelongToLoggedInUser", "This saved list does not belong to you.");
                return View(vm);
            }

            savedList.Name = vm.Name;
            savedList.Description = vm.Description;

            _savedListRepo.UpdateSavedList(savedList);
           
            return RedirectToAction(nameof(ListAllSavedLists));            
        }


        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult DeleteSavedList(int savedListId)
        {
            DeleteSavedListViewModel vm = new DeleteSavedListViewModel();

            SavedList savedList = _savedListRepo.FindSavedList(savedListId);

            if (savedList == null)
            {
                ModelState.AddModelError("NoSavedListFound", "This saved list no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != savedList.Tourist.Id)
            {
                ModelState.AddModelError("SavedListDoesNotBelongToLoggedInUser", "This saved list does not belong to you.");
                return View(vm);
            }

            vm.SavedListId = savedList.SavedListId;
            vm.Name = savedList.Name;
            vm.Description = savedList.Description;
            vm.DateCreated = savedList.DateCreated;
            vm.CountOfAttractions = savedList.AttractionList.Count;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult DeleteSavedList(DeleteSavedListViewModel vm)
        {
            SavedList savedList = _savedListRepo.FindSavedList(vm.SavedListId);

            if (savedList == null)
            {
                ModelState.AddModelError("NoSavedListFound", "This saved list no longer exists.");
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();

            if (loggedInTouristId != savedList.Tourist.Id)
            {
                ModelState.AddModelError("SavedListDoesNotBelongToLoggedInUser", "This saved list does not belong to you.");
                return View(vm);
            }

            _savedListRepo.DeleteSavedList(savedList);

            return RedirectToAction(nameof(ListAllSavedLists));
        }




        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult AddAttractionToSavedList(int attractionId)
        {
            CreateDropDownLists();

            AddAttractionToSavedListViewModel vm = new AddAttractionToSavedListViewModel();

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
        public IActionResult AddAttractionToSavedList(AddAttractionToSavedListViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                CreateDropDownLists();
                return View(vm);
            }

            Attraction attraction = _attractionRepo.FindAttraction(vm.AttractionId.Value);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(vm);
            }

            SavedList savedList = _savedListRepo.FindSavedList(vm.SavedListId.Value);

            if (savedList == null)
            {
                ModelState.AddModelError("NoSavedListFound", "This saved list no longer exists.");
                return View(vm);
            }

            AttractionSavedList attractionSavedList = new AttractionSavedList(attraction, savedList);

            int attractionSavedListId = _savedListRepo.AddAttractionToSavedList(attractionSavedList);

            if (attractionSavedListId <= -1)
            {
                ModelState.AddModelError("ErrorAttractionAlreadyOnList", "Cannot add the same attraction twice on the same list.");

                CreateDropDownLists();
                return View(vm);
            }

            return RedirectToAction(nameof(GetSavedListDetails), new { savedListId = vm.SavedListId.Value });
        }


        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult DeleteAttractionOnSavedList(int attractionSavedListId)
        {
            AttractionSavedList? attractionSavedList = _savedListRepo.FindAttractionSavedList(attractionSavedListId);

            int savedListId = attractionSavedList.SavedList.SavedListId;

            _savedListRepo.DeleteAttractionOnSavedList(attractionSavedList);

            return RedirectToAction(nameof(GetSavedListDetails), new { savedListId });
        }
    }
}
