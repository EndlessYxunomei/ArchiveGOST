using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System.Collections.ObjectModel;
using VMLayer.Navigation;

namespace VMLayer
{
    public class DocumentListViewModel: ObservableObject, INavigationParameterReceiver
    {
        //сервисы
        private readonly IDocumentService documentService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        //поля
        private DocumentListDto? _selectedDocument;

        //свойства
        public DocumentListDto? SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                if (SetProperty(ref _selectedDocument, value))
                {
                    DeleteCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];

        //кнопки
        public IAsyncRelayCommand CreateCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }
        public IAsyncRelayCommand EditCommand { get; }
        private async Task CreateDocument() => await navigationService.GoToCreateDocument();
        private async Task EditDocument()
        {
            if (SelectedDocument != null)
            {
                await navigationService.GoToDocumentDetails(SelectedDocument.Id);
            }
        }
        private async Task DeleteDocument()
        {
            if (SelectedDocument != null)
            {
                var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить документ {SelectedDocument!.Name} от {SelectedDocument.Date:d}?");
                if (result)
                {
                    //Удаление оригинала
                    await documentService.DeleteDocument(SelectedDocument.Id);

                    //обновление списка
                    DocumentList.Remove(SelectedDocument);
                    SelectedDocument = null;

                    await dialogService.Notify("Удалено", "Документ удалён");
                }
            }
        }
        private bool CanEditDeleteDocument() => SelectedDocument != null;

        //конструктор
        public DocumentListViewModel(IDocumentService documentService, IDialogService dialogService, INavigationService navigationService)
        {
            this.documentService = documentService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            CreateCommand = new AsyncRelayCommand(CreateDocument);
            DeleteCommand = new AsyncRelayCommand(DeleteDocument, CanEditDeleteDocument);
            EditCommand = new AsyncRelayCommand(EditDocument, CanEditDeleteDocument);

            LoadDocumentListAsync();
        }

        //загрузка данных
        private async void LoadDocumentListAsync()
        {
            var doclist = await documentService.GetDocumentListAsync();
            doclist.ForEach(DocumentList.Add);
        }

        //обработка навигации
        public Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue(NavParamConstants.DocumentList, out object? doc_list) && doc_list is DocumentListDto documentListDto)
            {
                UtilityService.UpdateList(DocumentList, documentListDto);
            }
            return Task.CompletedTask;
        }
    }
}
