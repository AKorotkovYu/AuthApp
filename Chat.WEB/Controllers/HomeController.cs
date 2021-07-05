using OneChat.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using OneChat.BLL.DTO;
using System.Threading.Tasks;
using AutoMapper;
using OneChat.BLL.Infrastructure;
using OneChat.BLL.Interfaces;

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
            return View(store.GetAllUserChats(1));//TODO: Взять ID
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
            store.RemoveChat(chatId);//Remove(store.FindChat(chatId).Result);
            store.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
