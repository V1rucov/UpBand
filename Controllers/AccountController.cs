using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using upband.Models;
using upband.Data;
using upband.Data.Entities;

namespace upband.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<user> userManager;
        private readonly SignInManager<user> signInManager;
        private readonly IEmailService emailService;
        private readonly IPasswordHasher<user> passwordHasher;
        private readonly DataBaseContext dataBaseContext;

        public AccountController(UserManager<user> _userManager, SignInManager<user> _signInManager,
            IEmailService _emailService, IPasswordHasher<user> _passwordHasher,
            DataBaseContext _dataBaseContext) {
            userManager = _userManager;
            signInManager = _signInManager;
            emailService = _emailService;
            passwordHasher = _passwordHasher;
            dataBaseContext = _dataBaseContext;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                userManager.PasswordHasher = passwordHasher;
                var result1 = userManager.FindByEmailAsync(model.Email).Result;
                var result2 = userManager.FindByNameAsync(model.UserName).Result;
                if (result1 == null && result2 == null)
                {
                    user User = new user() { UserName = model.UserName, Email = model.Email, UserProfile = new profile()};
                    var result = await userManager.CreateAsync(User, model.Password + model.Email.Length);
                    if (result.Succeeded)
                    {
                        var token = userManager.GenerateEmailConfirmationTokenAsync(User).Result;
                        var UrlCallBack = Url.Action("ConfirmRegistration", "Account", new { Email = User.Email, Token = token });
                        await emailService.SendAsync(User.Email, "UpBand", $"Здравствуйте! Перейдите по ссылке ниже для окончания процесса регистрации в сервисе UpBand <a href=https://{HttpContext.Request.Host}{UrlCallBack}>ссылка</a>");
                        ViewData["result"] = "Проверьте вашу почту!";
                        return Redirect("/Account/Login");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Что-то пошло не так");
                        return View(model);
                    }
                }
                else ModelState.AddModelError("Email", "Этот адрес/логин уже занят");
                return View(model);
            }
            else return View(model);
        }
        public async Task<IActionResult> ConfirmRegistration(string Email, string Token)
        {
            user User = userManager.FindByEmailAsync(Email).Result;
            if (Email != null && Token != null)
            {
                var result = await userManager.ConfirmEmailAsync(User, Token);
                if (result.Succeeded)
                {
                    ViewData["result"] = "Теперь вы можете войти!";
                    return Redirect("/Account/Login");
                }
                return Redirect("/");
            }
            return Redirect("/");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                userManager.PasswordHasher = passwordHasher;
                var User = userManager.FindByEmailAsync(model.Email);
                if (User.Result.EmailConfirmed == false)
                {
                    ModelState.AddModelError("Email", "Вы не подтвердили электронную почту.");
                    return View(model);
                }
                var cc = User.Result;
                if (cc.PasswordHash == passwordHasher.HashPassword(cc, model.Password + model.Email.Length))
                {
                    await signInManager.SignInAsync(cc, false);
                    return Redirect("/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("Email", "Неправильный адрес почты или пароль.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Что-то пошло не так");
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings() {
            user User = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(User); 
        }
    }
}