using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;

namespace VMLayer
{
    //ПОКА ТАК, ПОТОМ ПРЕДЕЛАТЬ НА БАЗОВЫЙ КЛАСС, ОТ КОТРОГО БУДУТ ПЛЯСАТЬ ВСЕ ОСТАЛЬНЫЕ
    
    public class OriginalDetailViewModel: ObservableObject, INavigationParameterReceiver
    {
        //Приватные поля
        private readonly int _id;
        private int _inventoryNumber;
        private string _name = string.Empty;
        private string _caption = string.Empty;
        private string? _pageFormat;
        private int _pageCount;
        private CompanyListDto? _company;
        private List<CompanyListDto> _companyList = [];
        private DocumentListDto? _document;
        private List<DocumentListDto> _documentList = [];
        private string? _notes;
        private PersonListDto? _person;
        private List<PersonListDto> _personList = [];

        //Публичные поля
        public int InventoryNumber
        {
            get => _inventoryNumber;
            set => SetProperty(ref _inventoryNumber, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }
        public string? PageFormat 
        { 
            get => _pageFormat;
            set => SetProperty(ref _pageFormat,value); 
        }
        public int PageCount 
        { 
            get => _pageCount; 
            set => SetProperty(ref _pageCount, value); 
        }
        public CompanyListDto? Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }
        public List<CompanyListDto> Companylist
        {
            get => _companyList;
        }
        public DocumentListDto? Document
        {
            get => _document;
            set => SetProperty(ref _document, value);
        }
        public List<DocumentListDto> DocumentList
        {
            get => _documentList;
        }
        public string? Notes
        { 
            get => _notes; 
            set => SetProperty(ref _notes, value); 
        }
        public PersonListDto? Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }
        public List<PersonListDto> PersonList
        {
            get => _personList;
        }

        //ещё не реализовано
        public List<CopyListDto> Copies { get; set; } = [];
        public List<CorrectionListDto> Corrections { get; set; } = [];
        public List<ApplicabilityListDto> Applicabilities { get; set; } = [];

        //Кнопки

        //Конструктор
        public OriginalDetailViewModel()
        {

        }

        public Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
