using AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AdminPanel.Controllers
{
    public class NewsController : Controller
    {
        IWebHostEnvironment _environment;
        NewsService NewsService;
        PicturesService PicturesService;
        public NewsController(ApplicationContext context, IWebHostEnvironment env)
        {
            _environment = env;
            NewsService = new NewsService(context);
            PicturesService = new PicturesService(context);
        }
        public async Task<IActionResult> Index()
        {
            var listNews = NewsService.GetAll();
            return View(listNews);
        }

        public IActionResult Edit(int NewsId)
        {
            //получить из сервиса 1 новость по айди
            var OneNews = NewsService.GetDetails(NewsId);
            //передать во вьюшку
            return View(OneNews);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] News News)
        {
            NewsService.Edit(News);
            return RedirectToAction("Index");
        }
        //1. Зачем FromForm? Уберите его. Ниже (пункт 2) правильное написание
        //2. public void Delete(int id)
        //вообще, public - плохая практика. Из любого класса разраб может обратиться к методу удаления поля. Это небезопасно.
        //используй interface с методами, его имплементируй в контроллере.
        public IActionResult DeleteNews([FromForm] News News)
        {
            //Получить айди новости для удаления
            NewsService.Delete(News);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(string newsName, string newsDescription, IFormFileCollection uploadedFiles)
        {
            //создаем новость через экземпляр класса news
            var news = new News { Name = newsName, Description = newsDescription };
            var NewsID = NewsService.Create(news);


            if (uploadedFiles != null)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    var fileName = Guid.NewGuid() + "." + (uploadedFile.FileName.Split('.').Last());
                    //путь для хранени файла
                    var path = "/img/" + fileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        //копируем изображения в папку wwrooot + path
                        uploadedFile.CopyTo(fileStream);
                    }
                    Pictures pic = new Pictures() { Name = uploadedFile.FileName, FilePath = path, NewsId = NewsID };
                    PicturesService.Create(pic);
                    //получаем созданную новость
                    var CreatedNews = NewsService.GetDetails(NewsID);
                    //добавляем в новость ссылку на главную картинку
                    CreatedNews.MainPicturePath = path;
                    //сохраняем новость
                    NewsService.Edit(CreatedNews);

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
