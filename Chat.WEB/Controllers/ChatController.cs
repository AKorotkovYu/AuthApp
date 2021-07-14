﻿using Microsoft.AspNetCore.Mvc;
using OneChat.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System.Linq;
using System;
using OneChat.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OneChat.WEB.Controllers
{
    public class ChatController : Controller
    {
        private readonly IStore store;
        private readonly ILogic logic;
        private int chatId;
        public ChatController(IStore store, ILogic logic)
        {
            this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [Authorize]
        public async Task<IActionResult> Index(int chatId)
        {
            if (chatId != 0)
            {
                this.chatId = chatId;
                return View(await store.GetMessages(chatId));
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(ChatMessageModel model)
        {
            string nickname = User.Claims.Where(c => c.Type == "Nickname").Select(c => c.Value).SingleOrDefault();
            if (model != null)
            {
                await logic.Send(
                new ChatMessageDTO
                {
                    Id = model.Id,
                    isOld = model.IsOld,
                    Nickname = nickname,
                    ChatId = model.ChatId,
                    ChatName = model.ChatName,
                    Message = model.Message,
                    TimeOfPosting = DateTime.Now
                });

                return RedirectToAction("Index", "Chat", new { chatId = model.ChatId });
            }
            return RedirectToAction("Index", "Chat", new { chatId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DelMes(int chatId, int mesId)
        {
            if(mesId!=0)
                await logic.DelMes(chatId, mesId);
            return RedirectToAction("Index", "Chat", new { chatId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Exit(int userId, int chatId)
        {
            if(userId!=0)
                await logic.Exit(userId, chatId);
            return RedirectToAction("Index", "Home");
        }
    }
}


