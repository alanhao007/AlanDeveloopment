using System.ComponentModel.DataAnnotations;

namespace AdminPanel.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name ="Введите Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null!;

        [Display(Name ="Запомнить меня")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
