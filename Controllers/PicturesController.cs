using AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class PicturesController : Controller
    {
        PicturesService PicturesService;
        public PicturesController(ApplicationContext context, IWebHostEnvironment env)
        {
            PicturesService = new PicturesService(context);
        }
        public IActionResult PicturesEdit()
        {
            var listPictures = PicturesService.GetAll();
            return View(listPictures);
        }
        public IActionResult DeletePicture([FromForm] Pictures picturesID)
        {
            //Получить айди фото для удаления
            PicturesService.Delete(picturesID);
            return RedirectToAction("Index", "News");
        }
    }
}
