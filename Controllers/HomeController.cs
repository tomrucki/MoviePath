using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoviePath.Models;

namespace MoviePath.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MovieSearch _movieSearch;

    // todo: load from config
    private readonly string _defaultTargetActor = "Tom Cruise";

    public HomeController(ILogger<HomeController> logger, MovieSearch movieSearch)
    {
        _logger = logger;
        _movieSearch = movieSearch;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var vm = new IndexViewModel { Actors = _movieSearch.GetAllActors() };
        return View(vm);
    }

    [HttpPost]
    public IActionResult Index(string actorFrom)
    {
        var searchResult = _movieSearch.FindPath(actorFrom, _defaultTargetActor);
        var vm = new IndexViewModel
        {
            Actors = _movieSearch.GetAllActors(),
            SearchSteps = searchResult,
        };

        return View(vm);
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
