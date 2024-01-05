using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;

namespace ArchiveGHOST.Client.Services
{
    public class NavigationService : INavigationService
    {
        private async Task Navigate(string pageName, Dictionary<string,object> parameters)
        {
            await Shell.Current.GoToAsync(pageName);
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }
        private async Task Navigate(string pageName) => await Shell.Current.GoToAsync(pageName);

        public async Task GoBack() => await Shell.Current.GoToAsync("..");
        public async Task GoBackAndReturn(Dictionary<string, object> parameters)
        {
            await GoBack();
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }

        public Task GoToCreateOriginal() => Navigate("CreateOriginal");
        public Task GoToOriginalDetails(int id) => Navigate("OriginalDetail", new() { { "id", id } });
    }
}
