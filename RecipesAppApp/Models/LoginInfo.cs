using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Models
{
    public class LoginInfo
    {
        public string UserEmail { get; set; } = null!;

        public string Password { get; set; } = null!;

        public LoginInfo(string UserEmail,string Password)
        {
            this.UserEmail = UserEmail;
            this.Password = Password;
        }
    }
}
