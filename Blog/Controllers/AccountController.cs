﻿using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Interfaces;
using Blog.Controllers;
using Blog.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [Route("login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("register")]
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register([FromServices] IMembership membership, RegisterUserViewModel model)
        {
            if(ModelState.IsValid){
                var userCheck = await _userManager.FindByEmailAsync(model.Email);
                if(userCheck != null)
                {
                    ModelState.AddModelError("Email", "This email is already used!");
                    return View(model);
                }
                User user = new User { Email = model.Email, UserName = model.Name, PhoneNumber = model.Phone };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Editor");
                    await membership.DisableMembershipCodeAsync(model.Code);
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(model);
        }

        [Route("settings")]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(currentUser);
        }

        [Route("change-password")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [Route("change-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync (model.Id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Settings");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                if (error.Description.Equals("Incorrect password."))
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid password");
                                    continue;
                                }
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User was not found");
                }
            }
            return View(model);

        }
    }
}
