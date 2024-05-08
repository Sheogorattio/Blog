using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите email")]
        [Display(Name = "Email")]
        public string? Email { get; set; } = "";

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; } = "";

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; } = "";
    }
}
