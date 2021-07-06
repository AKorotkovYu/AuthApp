using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System;
using System.Threading.Tasks;

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

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DelChat(int chatId)
        {
            await store.RemoveChat(chatId);
            return RedirectToAction("Index", "Home");
        }
    }
}
