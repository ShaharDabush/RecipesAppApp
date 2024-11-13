using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class RecipesAproveView : ContentPage
{
	public RecipesAproveView(RecipesAproveViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}