using AdminPanel.Models;

namespace AdminPanel.Services
{
        public class PicturesService
        {
            ApplicationContext db;
            public PicturesService(ApplicationContext context)
            {
                db = context;
            }
            public List<Pictures> GetAll()
            {
                return db.Pictures.ToList();
            }
            public List<Pictures> GetListForNews(int NewsId)
            {
                //вывести картинки для данной новости
                // новость определяется по полю News Id
                return db.Pictures.Where(x => x.NewsId == NewsId).ToList();
            }
            public Pictures GetDetails(int PicturesId)
            {
                return db.Pictures.Where(x => x.Id == PicturesId).FirstOrDefault();
            }
            public string? Create(Pictures pictures)
            {   
                db.Pictures.Add(pictures);
                db.SaveChanges();
                return pictures.FilePath;
            }
            public void Delete(Pictures pictures)
            {
                db.Pictures.Remove(pictures);
                db.SaveChanges();
            }
            public int SetMain(int PictureId)
            {

                //получить картинку из базы
                var MyPicture = db.Pictures.Where(x => x.Id == PictureId).FirstOrDefault();
                //проверяю является ли наша картинка главной. если да, то оставляю ее ID
                if (MyPicture.IsMain == true)
                {
                    return MyPicture.Id;
                }

                //Просматриваю есть ли главная картинка
                //Пробегаю по всем картинкам новости и ищу главную
                //получаю айди новости
                var NewsId = MyPicture.NewsId;
                //найти все картинки от этой новости
                var AllPictures = GetListForNews(NewsId);
                //найти старую главную картинку
                var OldMainPicture = AllPictures.Where(x => x.IsMain==true).FirstOrDefault();
                //если есть нахожу ее и снимаю главность
                if(OldMainPicture != null)
                {
                    OldMainPicture.IsMain = false;
                }
                //делаю мою картинку главной
                MyPicture.IsMain=true;
                //получить новость
                var news = db.News.Where(x => x.Id == NewsId).FirstOrDefault();
                //сохраняем мссылку на главную картинку в новости
                news.MainPicturePath = MyPicture.FilePath;

            db.SaveChanges();
                return MyPicture.Id;

            }
        }
}
