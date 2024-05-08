using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
	public class EditUserViewModel
	{
		[Key]
		public string? Id { get; set; }

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
	}
}
