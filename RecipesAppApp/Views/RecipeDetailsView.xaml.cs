using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class RecipeDetailsView : ContentPage
{
    private readonly IServiceProvider serviceProvider;
    public RecipeDetailsView(RecipeDetailsViewModel vm, IServiceProvider sp)
	{
        this.serviceProvider = sp;
        BindingContext = vm;
        vm.OpenPopup += DisplayPopup;
        InitializeComponent();
	}
    public void DisplayPopup(List<string> l)
    {
        var popup = new RemoveIngredientsView((RecipeDetailsViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }
}