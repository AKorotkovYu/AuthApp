using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System;

namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IStore store;

        public HomeController(IStore serv)
        {
            this.store = serv;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(store.GetAllUserChats(Int32.Parse(User.Identity.Name)));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [HttpPost]
        public IActionResult DelChat(int chatId)
        {
            store.RemoveChat(chatId);
            store.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
