using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AdminPanel.ViewModels
{
    public class RegisterViewModel
    {
        [Required] //аннотация, которая гвоорит, что это поле обязательно для заполнения со стороны пользователя
        [Display(Name = "Email")]//На странице регистрации пользователь увидит надпись "Email" над заполняемым полем
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Год рождения")]//На странице регистрации пользователь увидит надпись "Год рождения" над заполняемым полем
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]  //На странице регистрации пользователь увидит надпись "Пароль" над заполняемым полем
        public string Password { get; set; }=null!;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]//аннотаця для сравнения введенного пароля с паролем
                                                                   //из поля "Password". Если они не равны, то выпадет ошибка 
                                                                   //на странице в виде надписи "Пароли не совпадают"
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]//На странице регистрации пользователь увидит надпись "Подтвердить пароль"
                                              //над заполняемым полем
        public string PasswordConfirm { get; set; }=null !;
    }
}

