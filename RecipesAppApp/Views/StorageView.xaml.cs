using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class StorageView : ContentPage
{
	public StorageView(StorageViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}