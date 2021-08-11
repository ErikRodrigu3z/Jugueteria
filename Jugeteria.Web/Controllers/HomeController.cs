using Jugeteria.Web.Models;
using Jugueteria.Models;
using Jugueteria.Models.ViewModels;
using Jugueteria.Service.Repositories.HttpClientService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Jugeteria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClientService _httClient;
        public HomeController(ILogger<HomeController> logger, HttpClientService httClient)
        {
            _logger = logger;
            _httClient = httClient;
        } 

        public async Task<IActionResult> Index()
        {
            try
            {
                ToysViewModel model = new ToysViewModel
                {
                    ListToys = await _httClient.GetListAsync<Toys>("toys", null)
                };
                
                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
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
