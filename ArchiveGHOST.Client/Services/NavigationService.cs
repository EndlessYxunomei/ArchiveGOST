using AcrhiveModels.DTOs;
using VMLayer.Navigation;

namespace ArchiveGHOST.Client.Services
{
    public class NavigationService : INavigationService
    {
        private static async Task Navigate(string pageName, Dictionary<string,object> parameters)
        {
            await Shell.Current.GoToAsync(pageName);
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }
        private static async Task Navigate(string pageName) => await Shell.Current.GoToAsync(pageName);

        public async Task GoBack() => await Shell.Current.GoToAsync("..");
        public async Task GoBackAndReturn(Dictionary<string, object> parameters)
        {
            await GoBack();
            if (Shell.Current.CurrentPage.BindingContext is INavigationParameterReceiver receiver)
            {
                await receiver.OnNavigatedTo(parameters);
            }
        }

        public Task GoToCreateOriginal() => Navigate(NavigationConstants.CreateOriginal);
        public Task GoToOriginalDetails(int id) => Navigate(NavigationConstants.OriginalDetail, new() { { "original_detail", id } });
        public Task GoToCorrectionDetails(OriginalListDto original, int id = 0) => Navigate(NavigationConstants.CorrectionDetail, new() { { "original_list", original }, { "correction_detail", id } });
    }
}
