using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
/*
        [Authorize]*/
       public IActionResult Index()
        
        {
            return View();
        }
    }
}