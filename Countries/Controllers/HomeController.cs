using Countries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Countries.Services;

namespace Countries.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICountryService _countryService;

        public HomeController(ILogger<HomeController> logger, ICountryService countryService) {
            _logger = logger;
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };

            _countryService = countryService;
        }
        
        async public Task<IActionResult> Index(string? countryName)
        {
            if (countryName == null) {
                var countryList = await _countryService.GetCountryList();
                if (countryList == null) {
                    return Error();
                }
                return View(countryList.ToList());
            }
            var foundCountry = await _countryService.GetCountryByName(countryName);
            if (foundCountry == null) {
                return Error();
            }
            return View(foundCountry.ToList());
        }
        async public Task<IActionResult> Details(int countryId)
        {
            var resCountry = await _countryService.GetCountryById(countryId);
            if (resCountry == null) {
                return Error();
            }
            return View(resCountry);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}