using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Key]
        public string? Id {  get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is missing")]

        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        [Required(ErrorMessage = "Old password is missing")]

        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is missing")]
        public string NewPassword { get; set; }

        public string NewPasswordConfirm { get; set; }
    }
}
