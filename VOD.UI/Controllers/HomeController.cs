using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VOD.UI.Models;

using Microsoft.AspNetCore.Identity;
using VOD.Common.Entities;
//using VOD.UI.Models;
using VOD.Database.Services;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SignInManager<VODUser> _signInManager;

        //private IDbReadService _db;
        //private readonly IUIReadService _db;

        public HomeController(ILogger<HomeController> logger, SignInManager<VODUser> signInMgr
            /*, IUIReadService db*/)
        {
            _logger = logger;
            _signInManager = signInMgr;
            //_db = db;
        }

        public IActionResult Index()
        {
            //IDbReadService
            //_db.Include<Download>();
            //_db.Include<Module, Course>();
            //var result1 = await _db.SingleAsync<Download>(d => d.Id.Equals(3));

            //var result2 = await _db.GetAsync<Download>(); //Fetch all

            ////Fetch all that match the lambda expression
            //var result3 = await _db.GetAsync<Download>(d => d.ModuleId.Equals(1));

            //var result4 = await _db.AnyAsync<Download>(d => d.ModuleId.Equals(1)); // True if a record is found
            
            //IUIReadService
            //var courses = (await _db.GetCourses("b58d7c95-39cf-4050-bd07-8aa9e8aef701")).ToList();
            //var course = _db.GetCourse("b58d7c95-39cf-4050-bd07-8aa9e8aef701", 1);
            //var video = await _db.GetVideo("b58d7c95-39cf-4050-bd07-8aa9e8aef701", 1);
            //var videos = (await _db.GetVideos("b58d7c95-39cf-4050-bd07-8aa9e8aef701", 1)).ToList();

            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/Login", new { Area = "Identity" });

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
