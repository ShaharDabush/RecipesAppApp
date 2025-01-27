using RecipesAppApp.ViewModels;
using RecipesAppApp.Views;

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
            this.BindingContext = vm;
            
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("SignUp", typeof(SignUpView));
            Routing.RegisterRoute("Login", typeof(LoginView));
            Routing.RegisterRoute("HomePage", typeof(HomePageView));
            Routing.RegisterRoute("RecipeDetails", typeof(RecipeDetailsView));
            Routing.RegisterRoute("EditProfile", typeof(EditProfileView));

        }
    }
}
