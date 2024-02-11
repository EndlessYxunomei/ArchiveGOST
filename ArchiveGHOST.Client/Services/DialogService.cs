using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using ServiceLayer;
using VMLayer;

namespace ArchiveGHOST.Client.Services
{
    public class DialogService(IPopupService popupService) : IDialogService
    {
        private readonly IPopupService popupService = popupService;

        public Task<string?> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена")
            => Application.Current?.MainPage?.DisplayPromptAsync(title, message, accepButtonText, cancelButtonText) ?? throw new NullReferenceException();
        public Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет")
            => Application.Current?.MainPage?.DisplayAlert(title, message, trueButtonText, falseButtonText) ?? throw new NullReferenceException();
        public Task Notify(string title, string message, string buttonText = "ОК")
            => Application.Current?.MainPage?.DisplayAlert(title, message, buttonText) ?? throw new NullReferenceException();

        public async Task<object?> ShowApplicabilityPopup(int originalId = 0, object? parameters = null)
            => await popupService.ShowPopupAsync<ApplicabilityDetailViewModel>(onPresenting: x => x.LoadData(originalId, parameters));
        public async Task<object?> ShowCompanyDetailPopup(int id = 0)
            => await popupService.ShowPopupAsync<CompanyDetailViewModel>(onPresenting: x => x.LoadData(id));
        public async Task ClosePopup(object? popupView, object? parameters = null)
        {
            if (popupView != null && popupView is Popup popup)
            {
                await popup.CloseAsync(parameters);
            }
        }
    }
}
