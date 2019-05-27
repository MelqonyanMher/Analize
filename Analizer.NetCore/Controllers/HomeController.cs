using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Analizer.NetCore.Models;
using Analizer.NetCore.Services;

namespace Analizer.NetCore.Controllers
{
    public class HomeController : Controller
    {
        private IFireRiskMeneger _meneger;
        public HomeController(IFireRiskMeneger meneger)
        {
            _meneger = meneger;
        }

        public IActionResult Index()
        {
            _meneger.DeleteHistory();
            List<FireRiskItam> model = _meneger.GetToDayItams().ToList();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult History(int year = 2019, string city = "Yerevan")
        {
            ViewData["Message"] = "Your contact page.";
            List<FireRiskItam> model;
            if(year == DateTime.Now.Year)
            {
                model = _meneger.GetCityItam(city).ToList();
            }
            else
            {
                model = _meneger.GetCityItam(city, year).ToList();
            }
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            _meneger.DeleteHistory();
            List<FireRiskItam> model = _meneger.GetToDayItams().ToList();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
