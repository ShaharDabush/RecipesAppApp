using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Classes
{
    public class LevelList
    {
        public string LevelText { get; set; }
        public int LevelPlace { get; set; }
        public LevelList(string LevelText, int LevelPlace)
        {
            this.LevelText = LevelText;
            this.LevelPlace = LevelPlace;
        }
    }
}
