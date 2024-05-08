using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}


		[Route("users")]
		[HttpGet]
		public IActionResult Index()
		{
			return View(_userManager.Users.ToList());
		}

		[Route("edit-user")]
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			User user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			EditUserViewModel model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				Name = user.Name,
				Phone = user.PhoneNumber
			};

			return View(model);
		}


		[Route("edit-user")]
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Edit(EditUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = await _userManager.FindByIdAsync(model.Id);
				if (user != null)
				{
					user.Email = model.Email;
					user.Name = model.Name;
					user.PhoneNumber = model.Phone;

					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(Index));
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
			}

			return View(model);
		}


		[AutoValidateAntiforgeryToken]
		[HttpPost]
		public async Task<ActionResult> Delete(string id)
		{
			User user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}
			return RedirectToAction(nameof(Index));
		}


		[Route("user-roles")]
		[HttpGet]
		public async Task<IActionResult> EditRoles(string userId)
		{
			User user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				var allRoles = _roleManager.Roles.ToList();
				ChangeRoleViewModel model = new ChangeRoleViewModel
				{
					UserId = user.Id,
					UserEmail = user.Email,
					UserRoles = userRoles,
					AllRoles = allRoles
				};
				return View(model);
			}
			return NotFound();
		}


		[Route("user-roles")]
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> EditRoles(string userId, List<string> roles)
		{
			User user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				var addedRoles = roles.Except(userRoles);
				var removedRoles = userRoles.Except(roles);

				await _userManager.AddToRolesAsync(user, addedRoles);
				await _userManager.RemoveFromRolesAsync(user, removedRoles);

				return RedirectToAction(nameof(Index));
			}
			return NotFound();
		}
	}
}
