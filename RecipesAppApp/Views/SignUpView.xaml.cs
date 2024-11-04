using RecipesAppApp.ViewModels;

namespace RecipesAppApp.Views;

public partial class SignUpView : ContentPage
{
	public SignUpView(SignUpViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        //THe code below is a workarround for a bug in the Image control in MAUI
        //https://github.com/dotnet/maui/issues/18656
        base.OnAppearing();
        var bc = theImageBug.BindingContext;
        theImageBug.BindingContext = null;
        theImageBug.BindingContext = bc;

    }
}