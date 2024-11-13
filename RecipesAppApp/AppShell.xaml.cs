using RecipesAppApp.ViewModels;

namespace RecipesAppApp
{
    public partial class AppShell : Shell
    {
        //public AppShell()
        //{
        //    InitializeComponent();
        //}
        public AppShell(ShellViewModel vm)
        {

            InitializeComponent();
            this.BindingContext = vm;
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
        }
    }
}
