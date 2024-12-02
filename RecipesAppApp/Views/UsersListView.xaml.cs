using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class UsersListView : ContentPage
{
	public UsersListView(UsersListViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}