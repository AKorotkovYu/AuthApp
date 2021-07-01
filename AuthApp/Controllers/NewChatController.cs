using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AuthApp.Models;
using AuthApp.Services;
using System;

namespace AuthApp.Controllers
{

    [Authorize]
    public class NewChatController : Controller
    {
        IStore store;

        public NewChatController( OperatorContext context, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            store.SetDb(context);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost] 
        public async Task<IActionResult> CreateChat(ChatModel model)
        {
            if (ModelState.IsValid)
            {
                    Chat chat = new Chat
                    {
                        ChatName = model.ChatName
                    };

                    User user = store.FindUser(User.Identity.Name);
                    
                    chat.ChatUsers.Add(user);
                    
                    ChatMessage message = new ChatMessage() {ChatId=chat.Id, ChatName = chat.ChatName, Message = "чат создан", Nickname = "system", TimeOfPosting = System.DateTime.Now, isOld=true};
                    chat.ChatMessages.Add(message);
                    user.Chats.Add(chat);
                    
                    
                    store.Add(chat);
                    store.SaveMessage(message);

                    await store.SaveChanges();

                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home"); 
        }
    }

}
