using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Введите имя")]
		[Display(Name = "Имя")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Введите email")]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Введите номер телефона")]
		[Display(Name = "Номер телефона")]
		public string? Phone { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		[MinLength(5, ErrorMessage = "Пароль должен быть не менее 5 символов")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[Compare("Password", ErrorMessage = "Пароль не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string? ConfirmPassword { get; set; }
	}
}
