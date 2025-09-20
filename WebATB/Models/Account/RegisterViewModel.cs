using System.ComponentModel.DataAnnotations;

namespace WebATB.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }

        [Display(Name = "Фото аватару")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Display(Name = "Рік народження")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        public string PasswordConfirm { get; set; }
    }
}
