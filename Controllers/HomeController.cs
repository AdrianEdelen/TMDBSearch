using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TMDBSearch.Models;
using TMDB;
using Newtonsoft.Json;

namespace TMDBSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TMDBClient _tmdbClient;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //create tmdb client
            _tmdbClient = new TMDB.TMDBClient();
        }

        public async Task<IActionResult> Index()
        {
            var trendingResponse = await _tmdbClient.GetTrendingMoviesAsync();
            TempData["trendingResults"] = JsonConvert.SerializeObject(trendingResponse.results);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchAction(IFormCollection form)
        {
            var queryContent = form["searchQuery"].ToString();
            var searchResponse = await _tmdbClient.SearchMovieByTitleAsync(queryContent);
            if (searchResponse != null)
            {
                TempData["searchCount"] = searchResponse.total_results;
                TempData["searchResults"] = JsonConvert.SerializeObject(searchResponse.results);
            }
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
