using System.Diagnostics;
using LibraryTourismApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcTourismApp.Models;

namespace MvcTourismApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IAttractionRepo _attractionRepo;

        public HomeController(ILogger<HomeController> logger, IAttractionRepo attractionRepo)
        {
            _logger = logger;
            _attractionRepo = attractionRepo;
        }

        public void CreateDropDownLists()
        {
            List<Attraction> allAttractions = _attractionRepo.GetAllAttractionsWithReviews();

            var allAttractionsForDDL = allAttractions.Select(a => new
            {
                AttractionId = a.AttractionId,
                AttractionDetails = $"{a.Name} - {a.FullAddress}"
            });

            ViewData["AllAttractions"] = new SelectList(allAttractionsForDDL, "AttractionId", "AttractionDetails");

        }

        public IActionResult Index()
        {
            CreateDropDownLists();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
