using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using UI.App.Models;
using UI.App.Service;

namespace UI.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService apiService;
        private string token => User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Thumbprint)?.Value ?? "";

        //private static readonly List<ResponseEmployment> _listEmployments=new List<ResponseEmployment>();


        public HomeController(ILogger<HomeController> logger, IApiService apiService)
        {
            _logger = logger;
            this.apiService = apiService;
        }

        public IActionResult Index()
        {
            //ViewBag.RecentSearches = _listEmployments;
            return View();
        }


        public async Task<IActionResult> SearchByIdCard(string idcard)
        {
            var _listEmployments = new List<ResponseEmployment>();
            try
            {
                _listEmployments = await apiService.SearchById(token, idcard);
            }
            catch (Exception ex)
            {
                Response.Clear();
                var erMessage = string.Join("\n", apiService.GetErrors());
                ViewBag.ErrMessage = erMessage;
            }


            return PartialView("_ListEmployments", _listEmployments);
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