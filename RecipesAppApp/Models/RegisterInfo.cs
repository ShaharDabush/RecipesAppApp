using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Models
{
    public class RegisterInfo
    {
        public User UserInfo { get; set; }
        public Storage StorageInfo { get; set; }
        public string StorageCodeInfo { get; set; }
    }
}
