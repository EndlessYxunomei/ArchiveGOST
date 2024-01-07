using AcrhiveModels;
using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class OriginalDetailViewModel: ObservableValidator, INavigationParameterReceiver
{
    //Сервисы
    private protected readonly INavigationService navigationService;
    private protected readonly IDocumentService documentService;
    private protected readonly IOriginalService originalService;
    private protected readonly ICompanyService companyService;
    private protected readonly IPersonService personService;
    private protected readonly IDialogService dialogService;

    //Приватные поля
    private protected int _inventoryNumber;
    private protected string _name = string.Empty;
    private protected string _caption = string.Empty;
    private protected string? _pageFormat;
    private protected string? _notes;
    private protected int _pageCount = 1;
    private protected CompanyListDto? _company;
    private protected DocumentListDto? _document;
    private protected PersonListDto? _person;

    //Свойства
    [Required]
    [Range(1, int.MaxValue)]
    public int InventoryNumber
    {
        get => _inventoryNumber;
        set => SetProperty(ref _inventoryNumber, value, true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_NAME_LENGTH)]
    [MinLength(1)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }
    [Required]
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_CAPTION_LENGTH)]
    [MinLength(1)]
    public string Caption
    {
        get => _caption;
        set => SetProperty(ref _caption, value, true);
    }
    [MaxLength(ArchiveConstants.MAX_ORIGINAL_PAGES_FORMAT_LENGTH)]
    public string? PageFormat
    {
        get => _pageFormat;
        set => SetProperty(ref _pageFormat, value, true);
    }
    [Range(1, int.MaxValue)]
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

    //Кнопки добавления в списки
    public IAsyncRelayCommand AddDocumentCommand { get; }
    public IAsyncRelayCommand AddCompanyCommand { get; }
    public IAsyncRelayCommand AddPersonCommand { get; }
    private protected async Task AddDocument()
    {
        await Task.Delay(10);//ЗАглушка
        DocumentList.Add(new() { DocumentType = DocumentType.AddOriginal, Id = 33, Name = "Test3", Date = DateTime.Today });
    }
    private protected async Task AddCompany()
    {
        await Task.Delay(10);//ЗАглушка
    }
    private protected async Task AddPerson()
    {
        await Task.Delay(10);//ЗАглушка
    }

    //Конструктор
    public OriginalDetailViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService)
    {
        this.navigationService = navigationService;
        this.documentService = documentService;
        this.originalService = originalService;
        this.personService = personService;
        this.companyService = companyService;
        this.dialogService = dialogService;

        AddCompanyCommand = new AsyncRelayCommand(AddCompany);
        AddDocumentCommand = new AsyncRelayCommand(AddDocument);
        AddPersonCommand = new AsyncRelayCommand(AddPerson);

        ValidateAllProperties();//пришлось принудительно запускать валидацию, иначе не работало
    }

    //Пероначальная загрузка данных в списки
    internal virtual async Task LoadData()
    {
        var newDocumentList = await documentService.GetDocumentList(DocumentType.AddOriginal);
        var newCompanyList = await companyService.GetCompanyList();
        var newPersonList = await personService.GetPersonList();

        newDocumentList.ForEach(DocumentList.Add);
        newCompanyList.ForEach(Companylist.Add);
        newPersonList.ForEach(PersonList.Add);
    }
    //Заглушка для навигации
    public virtual Task OnNavigatedTo(Dictionary<string, object> parameters) => Task.CompletedTask;
}
