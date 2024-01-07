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
        private readonly IOriginalService originalService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        
        public OriginalListViewModel(IOriginalService originalService, IDialogService dialogService, INavigationService navigationService)
        {
            this.originalService = originalService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            //Создание команд
            CreateCommand = new AsyncRelayCommand(CreateOriginal);
            DeleteCommand = new AsyncRelayCommand(DeleteOriginal, CanDeleteOriginal);
            EditCommand = new AsyncRelayCommand(EditOriginal, CanEditOriginal);
            //Загружаем перовначальный список
            _ = LoadOriginalListAsync();

            //подписываемся на сообщения для обновления списка придобавлении или изменении
            //WeakReferenceMessenger.Default.Register<OriginalUpdatedMessage>(this, (r, m) =>
            //((OriginalListViewModel)r).UpdateOrignalList(m.Value));

            //TEST
            //OriginalsList.Add(new() { OriginalId = 9998, OriginalInventoryNumber = 101, OriginalName = "test1", OriginalCaption = "cap1", DocumentName = "doc1", OriginalDate = DateTime.Today });
            //OriginalsList.Add(new() { OriginalId = 9998, OriginalInventoryNumber = 101, OriginalName = "test1", OriginalCaption = "cap1", DocumentName = "doc1", OriginalDate = DateTime.Today });
            //OriginalsList.Add(new() { OriginalId = 9996, OriginalInventoryNumber = 103, OriginalName = "test3", OriginalCaption = "cap3", DocumentName = "doc3", OriginalDate = DateTime.Today });
        }
        //Список оригиналов
        private OriginalListDto? _selectedOriginal;
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


        /*private List<OriginalListDto> _originalsList = 
            [
                //Test
                new OriginalListDto() { OriginalId = 9998, OriginalInventoryNumber = 101, OriginalName = "test1", OriginalCaption = "cap1", DocumentName = "doc1", OriginalDate = DateTime.Today },
                new OriginalListDto() { OriginalId = 9997, OriginalInventoryNumber = 102, OriginalName = "test2", OriginalCaption = "cap2", DocumentName = "doc2", OriginalDate = DateTime.Today },
                new OriginalListDto() { OriginalId = 9996, OriginalInventoryNumber = 103, OriginalName = "test3", OriginalCaption = "cap3", DocumentName = "doc3", OriginalDate = DateTime.Today }
            ];

        public List<OriginalListDto> OriginalsList1
        {
            get => _originalsList;
            set => SetProperty(ref _originalsList, value);
        }*/
        public ObservableCollection<OriginalListDto> OriginalsList { get; set; } = [];
        //Кнопки работы с коллекицей
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
                await originalService.DeleteOriginal(SelectedOriginal.OriginalId);

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
                await navigationService.GoToOriginalDetails(SelectedOriginal.OriginalId);
            }
        }
        private bool CanDeleteOriginal()
        {
            if (SelectedOriginal == null) { return false; }
            return true;
        }
        private bool CanEditOriginal()
        {
            if (SelectedOriginal == null) { return false; }
            return true;
        }
        //Первоначальная загрузка листа оригиналов
        private async Task LoadOriginalListAsync()
        {
            var originallist = await originalService.GetOriginalListAsync();
            originallist.ForEach(OriginalsList.Add);
        }

        //Обновление листа оригиналов при добавлении или изменении
        private void UpdateOrignalList(OriginalListDto originalDto)
        {
            OriginalListDto? res = OriginalsList.FirstOrDefault(x => x?.OriginalId == originalDto.OriginalId, null);
            if (res != null)
            {
                OriginalsList[OriginalsList.IndexOf(res)] = originalDto;
            }
            else
            {
                OriginalsList.Add(originalDto);
            }
        }

        public Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters[NavParamConstants.OrginalList] is OriginalListDto originalListDto)
            {
                UpdateOrignalList(originalListDto);
            }
            return Task.CompletedTask;
        }
    }
}
