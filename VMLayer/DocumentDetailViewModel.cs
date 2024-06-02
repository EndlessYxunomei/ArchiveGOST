using AcrhiveModels;
using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer
{
    public class DocumentDetailViewModel : ObservableValidator, INavigationParameterReceiver
    {
        //сервисы
        private readonly IDocumentService documentService;
        private readonly IDialogService dialogService;
        private readonly ICompanyService companyService;
        private readonly INavigationService navigationService;

        //поля
        private int id;
        private string _name = string.Empty;
        private string? _description;
        private DateTime _date;
        private CompanyDto? _company;
        private DocumentType _documentType;
        private string oldName = string.Empty;
        private DateTime oldDate;

        public IReadOnlyList<DocumentType> DocumentTypes { get;} = (IReadOnlyList<DocumentType>)Enum.GetValues(typeof(DocumentType));
        //public IReadOnlyList<string> Types { get; } = Enum.GetNames(typeof(DocumentType));

        //свойства
        public ObservableCollection<CompanyDto> Companylist { get; set; } = [];
        [Required]
        [MaxLength(ArchiveConstants.MAX_NAME_LENGTH)]
        [MinLength(1)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, true);
        }
        [EmptyOrWithinRange(MinLength = 1, MaxLength = ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value, true);
        }
        [Required]
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value, true);
        }
        [Required]
        public CompanyDto? Company
        {
            get => _company;
            set => SetProperty(ref _company, value, true);
        }
        [Required]
        public DocumentType DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value, true);
        }

        //кнопки
        public IAsyncRelayCommand AcseptCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }
        public IAsyncRelayCommand AddCompanyCommand { get; }
        private async Task AddCompany()
        {
            var result = await dialogService.ShowCompanyDetailPopup();
            if (result != null && result is CompanyDto company)
            {
                var newid = await companyService.UpsertCompany(company);
                var newDto = await companyService.GetCompanyAsync(newid);
                UtilityService.UpdateList(Companylist, newDto);
            }
        }
        private async Task SaveDocument()
        {
            bool documentIsNotExists = await documentService.CheckDocument(Name, new(Date.Year, Date.Month, Date.Day));
            bool canSave = false;
            if (id == 0 && documentIsNotExists)
            {
                canSave = true;
            }
            else if ((Name == oldName && Date == oldDate) || documentIsNotExists)
            {
                canSave = true;
            }
            
            if (canSave)
            //if (await documentService.CheckDocument(Name, new(Date.Year, Date.Month, Date.Day)))
            {
                //сождаем ДТОшку
                DocumentDetailDto newDto = new()
                {
                    Name = Name,
                    DocumentType = DocumentType,
                    Description = Description,
                    Date = Date,
                    Company = Company,
                    Id = id
                };
                //закидываем её в базу и получаем обратно списочную дТО
                int newId = await documentService.UpsertDocument(newDto);
                DocumentListDto listDto = await documentService.GetDocumentAsync(newId);

                ErrorsChanged -= DocumentDetailViewModel_ErrorsChanged;

                //Возвращеамся по навигации с передачей занчения
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.DocumentList, listDto } });
            }
            else
            {
                await dialogService.Notify("Ошибка сохранения", "Документ с таким номером и датой уже существует");
            }
        }
        private async Task CancelDocument()
        {
            ErrorsChanged -= DocumentDetailViewModel_ErrorsChanged;
            await navigationService.GoBack();
        }

        //конструктор
        public DocumentDetailViewModel(IDocumentService documentService, ICompanyService companyService, IDialogService dialogService, INavigationService navigationService)
        {
            this.documentService = documentService;
            this.companyService = companyService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            AddCompanyCommand = new AsyncRelayCommand(AddCompany);
            AcseptCommand = new AsyncRelayCommand(SaveDocument, () => !HasErrors);
            CancelCommand = new AsyncRelayCommand(CancelDocument);

            _ = LoadData();

            ErrorsChanged += DocumentDetailViewModel_ErrorsChanged;
            ErrorExposer = new(this);
            ValidateAllProperties();
        }
        
        //Загрузка данных
        private async Task LoadData()
        {
            var newCompanyList = await companyService.GetCompanyList();
            newCompanyList.ForEach(Companylist.Add);
        }

        //Навигация
        public async Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue(NavParamConstants.DocumentDetail, out object? doc_det) && doc_det is int id)
            {
                this.id = id;
                DocumentDetailDto detailDto = await documentService.GetDocumentDetail(id);
                Name = detailDto.Name;
                Description = detailDto.Description;
                DocumentType = detailDto.DocumentType;
                Date = detailDto.Date;
                if (detailDto.Company != null) Company = Companylist.FirstOrDefault(x => x.Id == detailDto.Company.Id);

                oldDate = detailDto.Date;
                oldName = detailDto.Name;
            }
        }

        //Валидация
        private void DocumentDetailViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
        public ValidationErrorExposer ErrorExposer { get; }
    }
}
