using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using elraee.Models;
using elraee.ViewModels;
using elraee.IRepository;

namespace elraee.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    IContactInfoRepo contactRepo;

    public HomeController(ILogger<HomeController> logger , IContactInfoRepo contact)
    {
        _logger = logger;
        contactRepo = contact;
    }

    public IActionResult Index()
    {
         ViewBag.img = contactRepo.GetContact().EventCover;
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
