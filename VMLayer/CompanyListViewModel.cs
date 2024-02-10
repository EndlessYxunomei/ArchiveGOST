using AcrhiveModels;
using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer
{
    public class CompanyListViewModel: ObservableObject
    {
        //Сервисы
        private readonly ICompanyService companyService;
        private readonly IOriginalService originalService;
        private readonly IDocumentService documentService;
        private readonly IDialogService dialogService;

        //Поля
        private CompanyDto? _selectedCompany;

        //Свойства
        public CompanyDto? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (SetProperty(ref _selectedCompany, value))
                {
                    DeleteCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                    LoadCompanyDetail();
                }
            }
        }
        public ObservableCollection<CompanyDto> CompanyList { get; set; } = [];
        public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];
        public ObservableCollection<OriginalListDto> OriginalList { get; set; } = [];

        //Кнопки
        public IAsyncRelayCommand CreateCommand { get; }
        public IAsyncRelayCommand EditCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }
        private bool CanEditDeleteCompany() => SelectedCompany != null;
        private async Task CreateCompany()
        {
            var result = await dialogService.ShowCompanyDetailPopup();
            await UpdateCompanyList(result);
        }
        private async Task EditCompany()
        {
            var result = await dialogService.ShowCompanyDetailPopup(SelectedCompany!.Id);
            await UpdateCompanyList(result);
        }
        private async Task UpdateCompanyList(object? result)
        {
            if (result != null && result is CompanyDto dto)
            {
                var newid = await companyService.UpsertCompany(dto);
                var newDto = await companyService.GetCompanyAsync(newid);
                UtilityService.UpdateList(CompanyList, newDto);
            }
        }
        private async Task DeleteCompany()
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить компанию {SelectedCompany!.Name}?");
            if (result)
            {
                await companyService.DeleteCompany(SelectedCompany.Id);

                CompanyList.Remove(SelectedCompany);
                SelectedCompany = null;

                await dialogService.Notify("Удалено", "Компания удалена");
            }
        }

        //Конструктор
        public CompanyListViewModel(IDocumentService documentService, IOriginalService originalService, ICompanyService companyService, IDialogService dialogService)
        {
            this.documentService = documentService;
            this.originalService = originalService;
            this.companyService = companyService;
            this.dialogService = dialogService;

            CreateCommand = new AsyncRelayCommand(CreateCompany);
            DeleteCommand = new AsyncRelayCommand(DeleteCompany, CanEditDeleteCompany);
            EditCommand = new AsyncRelayCommand(EditCompany, CanEditDeleteCompany);

            LoadCompaniesList();
        }
        
        //Загрузка данных
        private async void LoadCompaniesList()
        {
            var complist = await companyService.GetCompanyList();
            //CompanyList = new ObservableCollection<CompanyDto>(await companyService.GetCompanyList());
            complist.ForEach(CompanyList.Add);
        }
        private async void LoadCompanyDetail()
        {
            if (SelectedCompany != null)
            {
                var doclist = await documentService.GetDocumentsByCompany(SelectedCompany.Id);
                var origlist = await originalService.GetOriginalsByCompany(SelectedCompany.Id);
                DocumentList.Clear();
                doclist.ForEach(DocumentList.Add);
                OriginalList.Clear();
                origlist.ForEach(OriginalList.Add);

                //DocumentList = new ObservableCollection<DocumentListDto>(await documentService.GetDocumentsByCompany(SelectedCompany.Id));
                //OriginalList = new ObservableCollection<OriginalListDto>(await originalService.GetOriginalsByCompany(SelectedCompany.Id));
            }
           
        }
    }
}
