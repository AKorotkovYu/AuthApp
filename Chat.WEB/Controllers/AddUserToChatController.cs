using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    public class AddUserToChatController : Controller
    {
        readonly IStore store;

        public AddUserToChatController(IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [Authorize]
        public IActionResult Index(int chatId)
        {
            return View(store.AllAnotherUsers(chatId));
        }

        [Authorize]
        public async Task<IActionResult> AddUser(int userId, int chatId)
        {
            await store.AddUserToChat(userId, chatId);

            return RedirectToAction("Index", "AddUserToChat", new { chatId });
        }
    }
}
