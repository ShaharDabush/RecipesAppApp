using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Classes
{
    public class UserAllergyWithIsChecked
    {
        public int AllergyId { get; set; }

        public string AllergyName { get; set; } = null!;

        public bool IsChecked { get; set; }

        public UserAllergyWithIsChecked() { }

        public UserAllergyWithIsChecked(int allergyId, string allergyName, bool isChecked)
        {
            AllergyId = allergyId;
            AllergyName = allergyName;
            IsChecked = isChecked;
        }

    }
}
