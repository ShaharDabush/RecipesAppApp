using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class RemoveIngredientsView : ContentPage
{
	public RemoveIngredientsView(RemoveIngredientsViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}