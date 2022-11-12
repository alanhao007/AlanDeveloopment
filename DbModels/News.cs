namespace AdminPanel.Models
{
    public class News
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? MainPicturePath { get; set; }
        public ICollection<Pictures> Pictures { get; set; }
        public News()
        {
            Pictures = new List<Pictures>();
        }

    }
}
