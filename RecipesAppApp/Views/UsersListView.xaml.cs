using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class UsersListView : ContentPage
{
	public UsersListView(UsersViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}