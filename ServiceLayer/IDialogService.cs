using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IDialogService
    {
        Task Notify (string title, string message, string buttonText = "ОК");
        Task<bool> AskYesNo(string title, string message, string trueButtonText = "Да", string falseButtonText = "Нет");
        Task<string?> Ask(string title, string message, string accepButtonText = "ОК", string cancelButtonText = "Отмена");
        Task ShowApplicabilityPopup(int originalId, object? parameters = null);
        Task ClosePopup(object? popupView, object? parameters = null);
    }
}
