using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using AuthApp.Services;
using System.Threading.Tasks;
using AuthApp.ViewModels;


namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {
        IStore store;

        public HomeController(OperatorContext context, IStore store)
        {
            this.store = store;
            store.SetDb(context);
        }

        [Authorize]
        public IActionResult Index()
        {
            List<Chat> chats = new();
            chats=store.GetAllUserChats(User.Identity.Name);
            
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

        [Authorize]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> DelChat(int chatId)
        {
            store.Remove(store.FindChat(chatId).Result);
            await store.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
