using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveGHOST.Client.Services
{
    public class DialogService : IDialogService
    {
        public Task<string?> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена")
            => Application.Current.MainPage.DisplayPromptAsync(title, message, accepButtonText, cancelButtonText);
        public Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет")
            => Application.Current.MainPage.DisplayAlert(title, message, trueButtonText, falseButtonText);
        public Task Notify(string title, string message, string buttonText = "ОК")
            => Application.Current.MainPage.DisplayAlert(title, message, buttonText);
    }
}
