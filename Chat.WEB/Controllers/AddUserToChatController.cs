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
        public async Task<IActionResult> Index(int chatId)
        {
            if (chatId != 0)
            {
                var allAnotherUsers = await store.AllAnotherUsersAsync(chatId);
                return View(allAnotherUsers);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> AddUser(int userId, int chatId)
        {
            await store.AddUserToChatAsync(userId, chatId);

            return RedirectToAction("Index", "AddUserToChat", new { chatId });
        }
    }
}
