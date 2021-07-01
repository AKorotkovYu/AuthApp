using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AuthApp.Services;
using System;

namespace AuthApp.Controllers
{
    public class AddUserToChatController : Controller
    { 
        IStore store;

        public AddUserToChatController( OperatorContext context, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            store.SetDb(context);
        }

        public IActionResult Index(int chatId)
        {
            Dictionary<User, int> users = store.AllAnotherUsers(chatId);
            return View(users);
        }

        [Authorize]
        public IActionResult AddUser(int userId, int chatId)
        {
            store.AddUserToChat(userId, chatId);
            store.SaveChanges();

            return RedirectToAction("Index","AddUserToChat", new { chatId });
        }
    }
}
