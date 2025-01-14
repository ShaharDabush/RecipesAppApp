using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class RecipeDetailsView : ContentPage
{
	public RecipeDetailsView(RecipeDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}