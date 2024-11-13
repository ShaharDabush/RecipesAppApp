using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class CreateRecipeView : ContentPage
{
	public CreateRecipeView(CreateRecipeViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}