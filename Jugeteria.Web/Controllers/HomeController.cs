using Jugeteria.Web.Models;
using Jugueteria.Models;
using Jugueteria.Models.ViewModels;
using Jugueteria.Service.Repositories.HttpClientService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jugeteria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClientService _httClient;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, HttpClientService httClient, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _httClient = httClient;
            _hostEnvironment = hostEnvironment;
        } 

        public async Task<IActionResult> Index()
        {
            try
            {
                ToysViewModel model = new ToysViewModel
                {
                    ListToys = await _httClient.GetListAsync<Toys>("toys", null) ?? new List<Toys>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {                
                var res = await _httClient.DeleteAsync<Toys>($"toys?id={id}", null, null);
                return Json("");
            }
            catch (Exception  ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Toys toys) 
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                string idToy = "";

                if (Request.Form["txtTipoTrans"].ToString() == "ADD")
                {
                    idToy = await _httClient.PostAsync<Toys>($"toys", toys, null);
                }

                if (Request.Form["txtTipoTrans"].ToString() == "EDIT")
                {
                    var res = await _httClient.PutAsync<Toys>($"toys", toys, null);
                }

                if (files.Count() > 0)
                {
                    foreach (var item in files)
                    {
                        string webRootPath = _hostEnvironment.WebRootPath;
                        var uploads = Path.Combine(webRootPath, @"img\toys\");
                        item.OpenReadStream();
                        using (var fileStreams = new FileStream(Path.Combine(uploads, toys.Id == 0 ? idToy + ".jpg" : toys.Id + ".jpg"), FileMode.Create))
                        {
                            item.CopyTo(fileStreams);
                        }
                    }
                   
                }
                
                return RedirectToAction("Index");
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
