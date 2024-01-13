using CommunityToolkit.Mvvm.ComponentModel;
using AcrhiveModels.DTOs;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using Azure.Identity;
using CommunityToolkit.Mvvm.Messaging;
using VMLayer.Messages;
using VMLayer.Navigation;

namespace VMLayer
{
    public class OriginalListViewModel: ObservableObject, INavigationParameterReceiver
    {
        //Сервисы
        private readonly IOriginalService originalService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        //Приватные поля
        private OriginalListDto? _selectedOriginal;

        //Свойства
        public OriginalListDto? SelectedOriginal
        {
            get => _selectedOriginal;
            set
            {
                if (SetProperty(ref _selectedOriginal, value))
                {
                    DeleteCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<OriginalListDto> OriginalsList { get; set; } = [];

        //Кнопки
        public IAsyncRelayCommand CreateCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }
        public IAsyncRelayCommand EditCommand { get; }
        private async Task CreateOriginal() => await navigationService.GoToCreateOriginal();
        private async Task DeleteOriginal()
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить {SelectedOriginal!.OriginalName} {SelectedOriginal.OriginalCaption}?");
            if (result)
            {
                //Удаление оригинала
                await originalService.DeleteOriginal(SelectedOriginal.Id);

                //обновление списка
                OriginalsList.Remove(SelectedOriginal);
                SelectedOriginal = null;
                
                await dialogService.Notify("Удалено", "Документ удалён");
            }
        }
        private async Task EditOriginal()
        {
            if (SelectedOriginal != null)
            {
                await navigationService.GoToOriginalDetails(SelectedOriginal.Id);
            }
        }
        private bool CanEditDeleteOriginal() => SelectedOriginal != null;

        //Конструктор
        public OriginalListViewModel(IOriginalService originalService, IDialogService dialogService, INavigationService navigationService)
        {
            this.originalService = originalService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            //Кнопки
            CreateCommand = new AsyncRelayCommand(CreateOriginal);
            DeleteCommand = new AsyncRelayCommand(DeleteOriginal, CanEditDeleteOriginal);
            EditCommand = new AsyncRelayCommand(EditOriginal, CanEditDeleteOriginal);

            //Загружаем перовначальный список
            _ = LoadOriginalListAsync();

            //подписываемся на сообщения для обновления списка придобавлении или изменении
            //WeakReferenceMessenger.Default.Register<OriginalUpdatedMessage>(this, (r, m) =>
            //((OriginalListViewModel)r).UpdateOrignalList(m.Value));
        }

        //Первоначальная загрузка данных
        private async Task LoadOriginalListAsync()
        {
            var originallist = await originalService.GetOriginalListAsync();
            originallist.ForEach(OriginalsList.Add);
        }
        
        //Обработка навигации
        public Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue(NavParamConstants.OrginalList, out object? orig_list) && orig_list is OriginalListDto originalListDto)
            {
                UtilityService.UpdateList(OriginalsList, originalListDto);
            }
            return Task.CompletedTask;
        }
    }
}
