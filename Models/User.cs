using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Models
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
    }
}
