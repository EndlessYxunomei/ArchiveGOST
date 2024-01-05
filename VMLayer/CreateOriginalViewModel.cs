using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Messages;
using VMLayer.Navigation;

namespace VMLayer
{
    public class CreateOriginalViewModel: ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly IDocumentService documentService;
        private readonly IOriginalService originalService;
        private readonly ICompanyService companyService;
        private readonly IPersonService personService;
        
        private int _inventoryNumber;
        public int InventoryNumber
        {
            get => _inventoryNumber;
            set => SetProperty(ref _inventoryNumber, value);
        }
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _caption = string.Empty;
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }
        private string? _pageFormat;
        public string? PageFormat
        {
            get => _pageFormat;
            set => SetProperty(ref _pageFormat, value);
        }
        private int _pageCount;
        public int PageCount
        {
            get => _pageCount;
            set => SetProperty(ref _pageCount, value);
        }
        private CompanyListDto? _company;
        public CompanyListDto? Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }
        public ObservableCollection<CompanyListDto> Companylist { get; set; } = [];
        private DocumentListDto? _document;
        public DocumentListDto? Document
        {
            get => _document;
            set => SetProperty(ref _document, value);
        }
        public ObservableCollection<DocumentListDto> DocumentList {get; set;} = [];
        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }
        private PersonListDto? _person;
        public PersonListDto? Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }
        public ObservableCollection<PersonListDto> PersonList { get; set; } = [];



        //Кнопки
        public IAsyncRelayCommand AcseptCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }
        //Кнопки добавления
        public IAsyncRelayCommand AddDocumentCommand { get; }
        public IAsyncRelayCommand AddCompanyCommand { get; }
        public IAsyncRelayCommand AddPersonCommand { get; }
        private async Task CreateOriginal()
        {
            //МЕТОД СОЗДАНИЯ НОВОГО ДОКУМЕНТА

            //ИСПРАВИТЬ НА ПРАВИЛЬНУЮ ДТОШКУ
            WeakReferenceMessenger.Default.Send(new OriginalUpdatedMessage(new OriginalListDto()));
            //НАВИГАЦИЯ НАЗАД
            await navigationService.GoBack();
            //ВОЗМОЖНО ИСПОЛЬЗОВАТЬ GoBackAndReturn
        }
        private async Task CancelCreate()
        {
            await navigationService.GoBack();
            //return Task.CompletedTask;//ВРОДЕ ТАК
        }
        private bool CanCreate()
        {
            //НУЖНА ВАЛИДАЦИЯ
            return false;
        }
        private async Task AddDocument()
        {
            await Task.Delay(10);//ЗАглушка
            DocumentList.Add(new() { DocumentType = AcrhiveModels.DocumentType.AddOriginal, Id = 33, Name = "Test3" });

        }
        private async Task AddCompany()
        {
            await Task.Delay(10);//ЗАглушка
        }
        private async Task AddPerson()
        {
            await Task.Delay(10);//ЗАглушка
        }
        public CreateOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService)
        {
            this.navigationService = navigationService;
            this.documentService = documentService;
            this.originalService = originalService;
            this.personService = personService;
            this.companyService = companyService;
            
            AcseptCommand = new AsyncRelayCommand(CreateOriginal, CanCreate);
            CancelCommand = new AsyncRelayCommand(CancelCreate);

            AddCompanyCommand = new AsyncRelayCommand(AddCompany);
            AddDocumentCommand = new AsyncRelayCommand(AddDocument);
            AddPersonCommand = new AsyncRelayCommand(AddPerson);

            _ = LoadData();
        }
        private async Task LoadData()
        {
            var newInventoryNumber = await originalService.GetLustInventoryNumber();
            var newDocumentList = await documentService.GetDocumentList(AcrhiveModels.DocumentType.AddOriginal);
            var newCompanyList = await companyService.GetCompanyList();
            var newPersonList = await personService.GetPersonList();

            InventoryNumber = newInventoryNumber + 1;
            newDocumentList.ForEach(DocumentList.Add);
            newCompanyList.ForEach(Companylist.Add);
            newPersonList.ForEach(PersonList.Add);

        }

    }
}
