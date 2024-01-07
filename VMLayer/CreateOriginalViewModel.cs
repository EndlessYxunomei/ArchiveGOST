using AcrhiveModels.DTOs;
using AcrhiveModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Messages;
using VMLayer.Navigation;
using System.ComponentModel.DataAnnotations;
using VMLayer.Validation;

namespace VMLayer;

public class CreateOriginalViewModel: ObservableValidator, INavigationParameterReceiver
{
    //Сервисы
    private readonly INavigationService navigationService;
    private readonly IDocumentService documentService;
    private readonly IOriginalService originalService;
    private readonly ICompanyService companyService;
    private readonly IPersonService personService;
    private readonly IDialogService dialogService;

    //Приватные поля
    private int _inventoryNumber;
    private string _name = string.Empty;
    private string _caption = string.Empty;
    private string? _pageFormat;
    private string? _notes;
    private int _pageCount = 1;
    private CompanyListDto? _company;
    private DocumentListDto? _document;
    private PersonListDto? _person;

    //Свойства
    [Required]
    [Range(1,int.MaxValue)]
    public int InventoryNumber
    {
        get => _inventoryNumber;
        set => SetProperty(ref _inventoryNumber, value,true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_NAME_LENGTH)]
    [MinLength(1)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value,true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_CAPTION_LENGTH)]
    [MinLength(1)]
    public string Caption
    {
        get => _caption;
        set => SetProperty(ref _caption, value,true);
    }
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_PAGES_FORMAT_LENGTH)]
    public string? PageFormat
    {
        get => _pageFormat;
        set => SetProperty(ref _pageFormat, value, true);
    }
    [Range(1,int.MaxValue)]
    public int PageCount
    {
        get => _pageCount;
        set => SetProperty(ref _pageCount, value, true);
    }
    public CompanyListDto? Company
    {
        get => _company;
        set => SetProperty(ref _company, value);
    }
    public DocumentListDto? Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_NOTES_LENGTH)]
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value, true);
    }
    public PersonListDto? Person
    {
        get => _person;
        set => SetProperty(ref _person, value);
    }

    //Списки для выбора
    public ObservableCollection<PersonListDto> PersonList { get; set; } = [];
    public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];
    public ObservableCollection<CompanyListDto> Companylist { get; set; } = [];

    //Кнопки
    public IAsyncRelayCommand AcseptCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand AddDocumentCommand { get; }
    public IAsyncRelayCommand AddCompanyCommand { get; }
    public IAsyncRelayCommand AddPersonCommand { get; }

    //Методы кнопок
    private async Task CreateOriginal()
    {
        //МЕТОД СОЗДАНИЯ НОВОГО ДОКУМЕНТА
        if (await originalService.CheckInventoryNumber(InventoryNumber))
        {
            OriginalDetailDto originalDetailDto = new()
            {
                InventoryNumber = InventoryNumber,
                Name = Name,
                Caption = Caption,
                PageFormat = PageFormat,
                PageCount = PageCount,
                Company = Company,
                Document = Document,
                Person = Person,
                Notes = Notes
            };
            //Пока реализовано так: создаем оригинал и в ответ получаем его id
            int newId = await originalService.UpsertOriginal(originalDetailDto);
            //получает по id дтошку для отображения в списке инвентарной книги
            OriginalListDto newDto = await originalService.GetOriginalAsync(newId);
            //Передаём обратно дтошку через мессендже
            //WeakReferenceMessenger.Default.Send(new OriginalUpdatedMessage(newDto));
            //await navigationService.GoBack();
            //Навигация назад с передачей параметра
            //orginal_list
            await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.OrginalList, newDto } });
        }
        else
        {
            await dialogService.Notify("Ошибка добавления", "Данный инвентарный номер уже существует");
        }
    }
    private async Task CancelCreate()
    {
        await navigationService.GoBack();
        //return Task.CompletedTask;//ВРОДЕ ТАК
    }
    private async Task AddDocument()
    {
        await Task.Delay(10);//ЗАглушка
        DocumentList.Add(new() { DocumentType = AcrhiveModels.DocumentType.AddOriginal, Id = 33, Name = "Test3", Date = DateTime.Today });
    }
    private async Task AddCompany()
    {
        await Task.Delay(10);//ЗАглушка
    }
    private async Task AddPerson()
    {
        await Task.Delay(10);//ЗАглушка
    }

    //Конструктор
    public CreateOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService)
    {
        this.navigationService = navigationService;
        this.documentService = documentService;
        this.originalService = originalService;
        this.personService = personService;
        this.companyService = companyService;
        this.dialogService = dialogService;

        ErrorExposer = new(this);
        
        AcseptCommand = new AsyncRelayCommand(CreateOriginal, () => !HasErrors);
        CancelCommand = new AsyncRelayCommand(CancelCreate);

        
        //Подписываемся на событие валидации
        ErrorsChanged += CreateOriginalViewModel_ErrorsChanged;

        AddCompanyCommand = new AsyncRelayCommand(AddCompany);
        AddDocumentCommand = new AsyncRelayCommand(AddDocument);
        AddPersonCommand = new AsyncRelayCommand(AddPerson);

        _ = LoadData();
        ValidateAllProperties();//пришлось принудительно запускать валидацию, иначе не работало
    }
    
    //Пероначальная загрузка данных
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

    //Обработка события валидатора
    private void CreateOriginalViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) =>AcseptCommand.NotifyCanExecuteChanged();
    public ValidationErrorExposer ErrorExposer { get; }

    //Обработка навигации
    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters[NavParamConstants.DocumentList] is DocumentListDto document)
        {
            DocumentList.Add(document);
        }
        if (parameters[NavParamConstants.CompanyList] is CompanyListDto company)
        {
            Companylist.Add(company);
        }
        if (parameters[NavParamConstants.PersonList] is PersonListDto person)
        {
            PersonList.Add(person);
        }
        return Task.CompletedTask;
    }

}
