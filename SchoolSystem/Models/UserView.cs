using System.Web;

namespace SchoolSystem.Models
{
    public class UserView
    {
        public User User { get; set; }
        public HttpPostedFileBase Photo { get; set; }

    }
}