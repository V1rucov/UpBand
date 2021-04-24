using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using upband.Models;
using upband.Data;
using upband.Data.Entities;

namespace upband.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext dataBaseContext;

        public HomeController(ILogger<HomeController> logger, DataBaseContext _dataBaseContext)
        {
            _logger = logger;
            dataBaseContext = _dataBaseContext;
        }

        public async Task<IActionResult> Index()
        {
            List<artist> artists = await dataBaseContext.artists.ToListAsync();
            return View(artists);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("404")]
        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> Search(string query) {
            List<artist> Artists = await dataBaseContext.artists.Where(a => a.BandName.Contains(query)).ToListAsync();
            if(Artists!=null)
                return View(Artists);
            ViewData["message"] = "Ничего не найдено";
            return View();
        } 
    }
}
