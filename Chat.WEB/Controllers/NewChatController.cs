using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using OneChat.BLL.Interfaces;
using OneChat.WEB.Models;
using OneChat.BLL.DTO;
using System.Threading.Tasks;

namespace OneChat.WEB.Controllers
{

    [Authorize]
    public class NewChatController : Controller
    {
        readonly ILogic logic;

        public NewChatController(ILogic logic)
        {
            this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatModel chatModel)
        {
            if (ModelState.IsValid)
            {
                await logic.CreateChat(
                    Int32.Parse(User.Identity.Name),
                    new ChatDTO() { 
                        Id=chatModel.Id, 
                        ChatName=chatModel.ChatName,
                        AdminId=chatModel.AdminId
                    });
            }
            return RedirectToAction("Index", "Home");
        }
    }

}
