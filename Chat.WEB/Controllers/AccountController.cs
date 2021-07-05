﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OneChat.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.DTO;
using OneChat.WEB.Models;

namespace OneChat.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IStore store;

        public AccountController(IStore store)
        {
            this.store = store;
        }

        public UserDTO userObject;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                userObject = store.GetUser(model.Email, model.Password);
                if (userObject != null)
                {
                    await Authenticate(model.Email); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                userObject = store.GetUser(model.Email, model.Password);
                if (userObject == null)
                {
                    store.AddNewUser(new UserDTO { Email = model.Email, Nickname = model.Nickname, Password = model.Password });
                    store.SaveChanges();

                    await Authenticate(model.Nickname); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult UserPage()
        {
            userObject = store.GetUser(HttpContext.User.Identity.Name);
            return View(userObject);
        }
    }
}