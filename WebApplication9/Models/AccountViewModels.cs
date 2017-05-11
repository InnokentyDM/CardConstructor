using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "Введите старый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(100, ErrorMessage = "Минимальная длина пароля {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class ManageUserData
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле Email не должно быть пустым")]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле пароль не должно быть пустым")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Поле email не должно быть пустым")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле пароль не должно быть пустым")]
        [StringLength(100, ErrorMessage = "Минимальная длина пароля {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Поле имя не должно быть пустым")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле фамилия не должно быть пустым")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Поле номер телефона не должно быть пустым")]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

    }
}
