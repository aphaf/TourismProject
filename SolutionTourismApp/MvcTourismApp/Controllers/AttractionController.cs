using LibraryTourismApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcTourismApp.Models;
using MvcTourismApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MvcTourismApp.Controllers
{
    public class AttractionController : Controller
    {
        private IAttractionRepo _attractionRepo;
        private IAppUserRepo _appUserRepo;
        private IReviewRepo _reviewRepo;
        //private IAttractionEmailSender _emailSender;

        public AttractionController(IAttractionRepo attractionRepo, IAppUserRepo appUserRepo, IReviewRepo reviewRepo
             // IAttractionEmailSender emailSender
             )
        {
            _attractionRepo = attractionRepo;
            _appUserRepo = appUserRepo;
            // _emailSender = emailSender;
            _reviewRepo = reviewRepo;
        }

        public void CreateDropDownLists()
        {
            //Drop down list for theme names
            List<Theme> allThemes = _attractionRepo.GetAllThemes();
            ViewData["AllThemes"] = new SelectList(allThemes, "ThemeId", "Name");

            //Drop down list for attractions
            List<Attraction> allAttractions = _attractionRepo.GetAllAttractions();
            ViewData["AllAttractions"] = new SelectList(allAttractions, "AttractionId", "Name");

            //Drop down list for cities
            var allCities = allAttractions.Select(a => a.City).Distinct().ToList();
            List<SelectListItem> listOfAllCities = allCities.Select(c => new SelectListItem { Text = c, Value = c }).ToList();
            ViewData["AllCities"] = listOfAllCities;

            //dropdown for county
            var allCounties = allAttractions.Select(a => a.County).Distinct().ToList();
            List<SelectListItem> listOfAllCounties = allCounties.Select(c => new SelectListItem { Text = c, Value = c }).ToList();
            ViewData["AllCounties"] = listOfAllCounties;

            //dropdown for state
            var allStates = allAttractions.Select(a => a.State).Distinct().ToList();
            List<SelectListItem> listOfAllStates = allStates.Select(s => new SelectListItem { Text = s, Value = s }).ToList();
            ViewData["AllStates"] = listOfAllStates;
        }

        //tourist actions
        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public IActionResult AddAttraction()
        {
            CreateDropDownLists();
            AddAttractionViewModel vm = new AddAttractionViewModel();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public IActionResult AddAttraction(AddAttractionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                CreateDropDownLists();
                return View(vm);
            }

            string loggedInTouristId = _appUserRepo.GetLoggedInUserId();
            Tourist loggedInUser = _appUserRepo.GetTouristById(loggedInTouristId);

            Attraction attraction = new Attraction(
                vm.Name, vm.Description, vm.TypeOfAttraction.Value,
                vm.Street, vm.City, vm.ZipCode, vm.State, vm.County,
                AttractionStatus.Pending,
                loggedInUser, vm.Website);

            int attractionId = _attractionRepo.AddAttraction(attraction);

            if (vm.ThemeIds != null && vm.ThemeIds.Any())
            {
                foreach (int eachThemeId in vm.ThemeIds)
                {
                    Theme theme = _attractionRepo.FindTheme(eachThemeId);
                    AttractionTheme attractionTheme = new AttractionTheme(attraction, theme);
                    int attractionThemeId = _attractionRepo.AddAttractionTheme(attractionTheme);

                    if (attractionThemeId == -1)
                    {
                        ModelState.AddModelError("ErrorDatabaseError", "Error when adding to database.");
                        CreateDropDownLists();
                        return View(vm);
                    }
                }
            }

            if (attractionId == -1)
            {
                ModelState.AddModelError("ErrorNameAndStreetRepeated", "Name and Street cannot be repeated.");
                CreateDropDownLists();
                return View(vm);
            }

            /*This logic is for sending email notifications, the implementation is commented out
            string subject = "New attraction has been added! Make a decision.";
            string message = $"View the attraction";

            List<Moderator> allModerators = _appUserRepo.GetAllModerators().Where(m => !m.UserName.Contains("@test.com")).ToList();
            //find all moderators who dont have @test.com in their username (all real moderators)

            foreach (Moderator moderator in allModerators)
            {
                _emailSender.SendAddAttractionEmail("Attraction/GetAllPendingAttractions", moderator.Email, subject, message);
            }
            */

            return RedirectToAction(nameof(SearchAttractions));

        }



        //all user actions
        [HttpGet]
        public IActionResult SearchAttractions()
        {
            CreateDropDownLists();

            SearchAttractionsViewModel vm = new SearchAttractionsViewModel();

            return View(vm);
        }

        [HttpPost]
        public IActionResult SearchAttractions(SearchAttractionsViewModel vm)
        {
            CreateDropDownLists();

            List<Attraction> allAttractions = _attractionRepo.GetAllAttractions();

            vm.SearchResult = Attraction.SearchAttractions(allAttractions, vm.SearchNameById, vm.SearchCity, vm.SearchCounty, vm.SearchThemeId, vm.SearchAttractionType);

            return View(vm);
        }


        [HttpGet]
        public IActionResult GetAttractionDetails(int attractionId)
        {
            Attraction? attraction = _attractionRepo.FindAttraction(attractionId);
            return View(attraction);
        }



        //moderator actions
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult EditAttraction(int attractionId)
        {
            CreateDropDownLists();

            EditAttractionViewModel? evm = new EditAttractionViewModel();

            Attraction attraction = _attractionRepo.FindAttraction(attractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(evm);
            }

            //setting up the view model to hold all of the attraction's info
            evm = new EditAttractionViewModel
            {
                AttractionId = attraction.AttractionId,
                Name = attraction.Name,
                Description = attraction.Description,
                TypeOfAttraction = attraction.TypeOfAttraction,
                Status = attraction.Status,
                Street = attraction.Street,
                City = attraction.City,
                ZipCode = attraction.ZipCode,
                State = attraction.State,
                County = attraction.County,
                Website = attraction.Website,
                TouristWhoAdded = attraction.TouristWhoAdded,
                ModeratorWhoMadeDecision = attraction.ModeratorWhoMadeDecision,
                DateDecisionMade = attraction.DateDecisionMade
            };

            return View(evm);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult EditAttraction(EditAttractionViewModel evm)
        {
            if (!ModelState.IsValid)
            {
                CreateDropDownLists();
                return View(evm);
            }

            Attraction attraction = _attractionRepo.FindAttraction(evm.AttractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(null);
            }

            if (evm.Status != attraction.Status
                && (evm.Status == AttractionStatus.Approved || evm.Status == AttractionStatus.Denied)
                && (attraction.DateDecisionMade == null || attraction.DateDecisionMade <= DateTime.Now))
            {
                string loggedInModeratorId = _appUserRepo.GetLoggedInUserId();
                Moderator moderator = _appUserRepo.GetModeratorById(loggedInModeratorId);
                attraction.ModeratorWhoMadeDecision = moderator;
                attraction.DateDecisionMade = DateTime.Now;
            }
            else if (evm.Status == AttractionStatus.Pending)
            {
                attraction.ModeratorWhoMadeDecision = null;
                attraction.DateDecisionMade = null;
            }

            attraction.Name = evm.Name;
            attraction.Description = evm.Description;
            attraction.TypeOfAttraction = evm.TypeOfAttraction.Value;
            attraction.Status = evm.Status.Value;
            attraction.Street = evm.Street;
            attraction.City = evm.City;
            attraction.ZipCode = evm.ZipCode;
            attraction.State = evm.State;
            attraction.County = evm.County;
            attraction.Website = evm.Website;

            _attractionRepo.UpdateAttraction(attraction);

            return RedirectToAction(nameof(SearchAttractions));
        }


        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult DeleteAttraction(int attractionId)
        {
            DeleteAttractionViewModel vm = new DeleteAttractionViewModel();

            Attraction? attraction = _attractionRepo.FindAttraction(attractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(vm);
            }

            vm = new DeleteAttractionViewModel 
            { 
                AttractionId = attractionId,
                Name = attraction.Name,
                Description = attraction.Description,
                FullAddress = attraction.FullAddress
            };
            
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult DeleteAttraction(DeleteAttractionViewModel vm)
        {
            Attraction? attraction = _attractionRepo.FindAttraction(vm.AttractionId);

            if (attraction == null)
            {
                ModelState.AddModelError("NoAttractionFound", "The attraction no longer exists.");
                return View(vm);
            }

            /* manual deletions not required with cascade in migration file
             while (attraction.Themes.Count > 0)
            {
                AttractionTheme at = attraction.Themes[0];
                _attractionRepo.DeleteAttractionTheme(at);
            }

            while (attraction.Reviews.Count > 0)
            {
                Review r = attraction.Reviews[0];
                _reviewRepo.DeleteReview(r);
            }*/

            _attractionRepo.DeleteAttraction(attraction);
            return RedirectToAction(nameof(SearchAttractions));
        }


        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetAllDeniedAttractions()
        {
            List<Attraction> allDeniedAttractions = _attractionRepo.GetAllDeniedAttractions();
            return View(allDeniedAttractions);
        }

        
        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult GetAllPendingAttractions()
        {
            List<Attraction> allPendingAttractions = _attractionRepo.GetAllPendingAttractions();

            MakeDecisionsOnAttractionsViewModel vm = new MakeDecisionsOnAttractionsViewModel { PendingAttractions = allPendingAttractions };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult MakeDecisionsOnAttractions(MakeDecisionsOnAttractionsViewModel vm)
        {
            int numberOfDecisions = vm.AttractionIds.Count;

            for (int i = 0; i < numberOfDecisions; i++)
            {
                if (vm.Decisions[i] != null && (AttractionStatus)Convert.ToInt32(vm.Decisions[i]) != AttractionStatus.Pending)
                {
                    AttractionStatus decisionStatus = (AttractionStatus)Convert.ToInt32(vm.Decisions[i]);

                    int attractionId = vm.AttractionIds[i];

                    MakeDecisionOnOneAttraction(attractionId, decisionStatus);
                }
            }

            return RedirectToAction(nameof(SearchAttractions));
        }

        public void MakeDecisionOnOneAttraction(int attractionId, AttractionStatus status)
        {
            Attraction attraction = _attractionRepo.FindAttraction(attractionId);

            attraction.Status = status;
            attraction.DateDecisionMade = DateTime.Now;
            string loggedInModeratorId = _appUserRepo.GetLoggedInUserId();
            attraction.ModeratorWhoMadeDecision = _appUserRepo.GetModeratorById(loggedInModeratorId);

            _attractionRepo.UpdateAttraction(attraction);

            /*string subject = "A decision has been made on your requested attraction!";
            string message = $"View the attraction ";

            _emailSender.SendAddAttractionEmail($"Attraction/GetAttractionDetails?attractionId={attractionId}", attraction.TouristWhoAdded.Email, subject, message);
            */
        }

    }
}
