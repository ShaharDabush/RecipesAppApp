using CommunityToolkit.Maui.Views;
using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class EditProfileView : ContentPage
{
    private readonly IServiceProvider serviceProvider;
    public EditProfileView(EditProfileViewModel vm, IServiceProvider sp)
	{
        this.serviceProvider = sp;
        BindingContext = vm;
        vm.OpenPopup += DisplayPopup;
        InitializeComponent();
	}
    public void DisplayPopup(List<string> l)
    {
        var popup = new EditProfilePopUp((EditProfileViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }
}