using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class User : IdentityUser
    {
        public int PublicationsCount { get; set; }
        public string? Name { get; set; }
    }
}
