using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OperatorContext db;

        public HomeController(ILogger<HomeController> logger, OperatorContext context)
        {
            _logger = logger;
            db = context;
        }

        [Authorize]
        public IActionResult Index(string nickname)
        {
            List<string> chats = new();
            foreach(Chat chat in db.Chats)
            {
                chats.Add(chat.ChatName);
            }
            return View(chats);
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
