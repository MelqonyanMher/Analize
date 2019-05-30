using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Analizer.NetCore.Models;
using Analizer.NetCore.Services;
using Newtonsoft.Json;

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
            List<FireRiskItam> model = _meneger.GetToDayItams().OrderBy(itm=>itm.City.Name).ToList();
            return View(model);
        }

        public IActionResult HistoryData()
        {
            return View(new HistoryItam());
        }
        
        public IActionResult Graph(string city)
        {
            List<GraphModel> dataPoints1 = new List<GraphModel>();
            List<FireRiskItam> itams =_meneger.GetCityItam(city).ToList();

            foreach(var itam in itams)
            {
                dataPoints1.Add(new GraphModel($"{itam.Day.Day}/{itam.Day.Month}", itam.ClassOfFireRisk));
            }

            ViewBag.GraphModel = JsonConvert.SerializeObject(dataPoints1);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public IActionResult History(HistoryItam itam)
        {
            ViewData["Message"] = "Your contact page.";
            List<FireRiskItam> model = null;
            if (itam.Year == DateTime.Now.Year)
            {
                model = _meneger.GetCityItam(itam.City).ToList();
            }
            else
            {
                model = _meneger.GetCityItam(itam.City, itam.Year).ToList();
            }

            return View(model);
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
