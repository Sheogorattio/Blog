using Blog.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Data
{
	public class RoleInitializer
	{
		public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				if (await roleManager.FindByNameAsync("Admin") == null)
				{
					await roleManager.CreateAsync(new IdentityRole("Admin"));
				}
				if (await roleManager.FindByNameAsync("Editor") == null)
				{
					await roleManager.CreateAsync(new IdentityRole("Editor"));
				}
			}
			if (!userManager.Users.Any())
			{
				var users = new[]
				{
					new { Email = "admin@gmail.com", Name = "Admin", Password = "qwerty" },
					new { Email = "alex@gmail.com", Name = "Alex", Password = "123456" },
					new { Email = "marry@gmail.com", Name = "Marry", Password = "123456" },
					new { Email = "tom@gmail.com", Name = "Tom", Password = "123456" },
					new { Email = "john@gmail.com", Name = "John", Password = "123456" }
				};

				foreach (var user in users)
				{
					if (await userManager.FindByEmailAsync(user.Email) == null)
					{
						User currentUser = new User { Email = user.Email, UserName = user.Email, Name = user.Name };
						IdentityResult result = await userManager.CreateAsync(currentUser, user.Password);

						if (result.Succeeded)
						{
							if (currentUser.Email.Equals("admin@gmail.com"))
							{
								await userManager.AddToRoleAsync(currentUser, "Admin");
							}
							else
							{
								await userManager.AddToRoleAsync(currentUser, "Editor");
							}
						}
					}
				}
			}
		}
	}
}
