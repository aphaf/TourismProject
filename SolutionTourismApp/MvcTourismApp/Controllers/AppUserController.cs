using LibraryTourismApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcTourismApp.Models;
using MvcTourismApp.ViewModels;

namespace MvcTourismApp.Controllers
{
    public class AppUserController : Controller
    {
        IAppUserRepo _appUserRepo;

        public AppUserController(IAppUserRepo appUserRepo)
        {
            _appUserRepo = appUserRepo;
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult ListAllTourists(ListAllTouristsViewModel vm)
        {
            vm.ListOfTourists = _appUserRepo.GetAllTourists();

            if (vm.TouristId != null)
            {
                Tourist tourist = vm.ListOfTourists.Where(t => t.Id == vm.TouristId).FirstOrDefault();
                vm.RecentReview = tourist.FindMostRecentReview();
            }

            return View(vm);
        }
    }
}
