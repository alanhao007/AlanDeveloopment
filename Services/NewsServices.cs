using AdminPanel.Models;

namespace AdminPanel.Services
{
    public class NewsService
    {
        ApplicationContext db;
        public NewsService(ApplicationContext context)
        {
            db = context;
        }
        public List<News> GetAll()
        {
            return db.News.ToList();
        }
        public News GetDetails(int NewsId)
        {
            return db.News.Where(x => x.Id == NewsId).FirstOrDefault();
        }

        public int Create(News news)
        {
            db.News.Add(news);
            db.SaveChanges();
            return news.Id;
        }

        public void Delete(News news)
        {
            db.News.Remove(news);
            db.SaveChanges();
        }
        public int Edit(News news)
        {
            //получить новость с таким же ID из базы. 
            var OldNews = GetDetails(news.Id);
            //Обновляю все поля из новых полей которые пришли
            db.Entry(OldNews).CurrentValues.SetValues(news);
            //сохраняю все в базе
            db.SaveChanges();
            //вернуть айди
            return news.Id;
        }
    }
}
