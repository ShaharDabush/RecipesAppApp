using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.ViewModels
{
    public class LoadingPageViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;

        public LoadingPageViewModel (IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            NewShell();
        }

        public void NewShell()
        {
            AppShell shell = serviceProvider.GetService<AppShell>();
            Application.Current.MainPage = shell;
            shell.FlyoutIsPresented = false;
        }
    }
}
