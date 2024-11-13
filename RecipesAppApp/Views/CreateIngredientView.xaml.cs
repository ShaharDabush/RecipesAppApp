using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;


public partial class CreateIngredientView : ContentPage
{
	public CreateIngredientView(CreateIngredientViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}