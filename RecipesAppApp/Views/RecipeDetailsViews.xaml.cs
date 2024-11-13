using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class RecipeDetailsViews : ContentPage
{
	public RecipeDetailsViews(RemoveIngredientsViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}