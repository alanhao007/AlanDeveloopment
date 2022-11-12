using AdminPanel.Models;
using AdminPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        //мы получаем сервис по управлению пользователями - UserManager и сервис SignInManager,
        //который позволяет аутентифицировать пользователя и устанавливать или удалять его куки.
        private readonly UserManager<User> _userManager;// управление пользователем
        private readonly SignInManager<User> _signInManager; //управление логином (куки)


        //создаем констркутор AccountController
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //метод будет отображать форму для регистрации
        public IActionResult Register()
        {
            return View();
        }

        //метод Post
        //из формы(Views) будут приходить сюда данные, которые будт обрабатывать сервер
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            //проверяем, все ли поля формы заполнены коректно 
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя в БД
                var result = await _userManager.CreateAsync(user, model.Password);//В качестве параметра передается
                                                                                  //сам пользователь и его пароль.
                //если ползователь добавился в БД, то временно(до первого закрытия вкладки) сохраняем его данные в куки
                //и автоматически попадаем на старницу в качечтве зарегистрированного пользователя
                if (result.Succeeded)
                {
                    // установка куки
/*                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");*/
//генерация токена
                 var code =await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callback = Url.Action(
                        "ConfirmEmail",
                         "Account",
                         new { userId = user.Id, code = code },
                         protocol:HttpContext.Request.Scheme);
                        EmailService emailService= new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Подтверждение вашего аккаунта",
                        $"Подтвердите вашу регистрацую, перейдя по ссылке " + $" <a href='{callback}'>link</a>");
                    return Content("Для завершения регистрации проверьте вашу почту");
                }
                //если по какой-то причине не удалось произвести запись в БД, то выводим ошибку
                else
                {
                    foreach (var error in result.Errors)//result.Errors - реестр ошибок в Identity
                    {
                        //добавляет модель ошибки на страницу Identity для отобрадения ошибко
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        // Когда пользователь запрашивает URL-адрес с ограниченным доступом, он перенаправляется
        // по адресу /Account/Login со строкой запроса, содержащей адрес страницы с ограниченным доступом
        public IActionResult Login(string? returnUrl=null)
        {
            return View(new LoginViewModel { ReturnUrl=returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,model.RememberMe, false); 
                if(result.Succeeded)
                {
                    //проверить принадлежит  ли url приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl) ) {
                        //тут
    /*                    return RedirectToAction("Login");*/
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильно введены логин или ппроль");
                }
            }

            return View(model);
        }

        [HttpPost]
       public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
