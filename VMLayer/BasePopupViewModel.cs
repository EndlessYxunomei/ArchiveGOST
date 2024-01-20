using AcrhiveModels.DTOs;
using AcrhiveModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VMLayer
{
    public class BasePopupViewModel : ObservableValidator
    {
        //Сервисы
        private readonly IDialogService dialogService;

        //Список ошибок
        public ObservableCollection<ValidationResult> Errors { get; } = [];

        //Кнопки
        public IAsyncRelayCommand<object?> AcseptCommand { get; }
        public IAsyncRelayCommand<object?> CancelCommand { get; }
        private async Task ClosePopupAndSave(object? popup)
        {
            ErrorsChanged -= BasePopupViewModel_ErrorsChanged;
            await dialogService.ClosePopup(popup, ReturnValue());
        }
        private async Task ClosePopup(object? popup)
        {
            ErrorsChanged -= BasePopupViewModel_ErrorsChanged;
            await dialogService.ClosePopup(popup);
        }
        
        //Конструктор
        public BasePopupViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            CancelCommand = new AsyncRelayCommand<object?>(ClosePopup);
            AcseptCommand = new AsyncRelayCommand<object?>(ClosePopupAndSave, x => !HasErrors);

            ErrorsChanged += BasePopupViewModel_ErrorsChanged;
        }

        //Валидация
        private void BasePopupViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            Errors.Clear();
            GetErrors().ToList().ForEach(Errors.Add);
            AcseptCommand.NotifyCanExecuteChanged();
        }

        //Возвращение данных
        internal virtual object? ReturnValue() => null;
    }
}
