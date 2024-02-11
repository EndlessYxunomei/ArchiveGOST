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
    public class ApplicabilityListViewModel: ObservableValidator
    {
        //Сервисы
        private readonly IApplicabilityService applicabilityService;
        private readonly IOriginalService originalService;
        private readonly IDialogService dialogService;

        //Поля
        private ApplicabilityDto? _selectedApplicability;

        //Свойства
        public ApplicabilityDto? SelectedApplicability
        {
            get => _selectedApplicability;
            set
            {
                if (SetProperty(ref _selectedApplicability, value))
                {
                    DeleteCommand.NotifyCanExecuteChanged();
                    EditCommand.NotifyCanExecuteChanged();
                    LoadApplicabilityDetail();
                }
            }
        }
        public ObservableCollection<ApplicabilityDto> ApplicabilityList { get; set; } = [];
        public ObservableCollection<OriginalListDto> OriginalList { get; set; } = [];

        //Кнопки
        public IAsyncRelayCommand CreateCommand { get; }
        public IAsyncRelayCommand EditCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }
        private bool CanEditDeleteApplicability() => SelectedApplicability != null;
        private async Task CreateApplicability()
        {
            var result = await dialogService.ShowApplicabilityPopup();
            await UpdateApplicabilityList(result);
        }
        private async Task EditApplicability()
        {
            var result = await dialogService.ShowApplicabilityPopup(0,SelectedApplicability);
            await UpdateApplicabilityList(result);
        }
        private async Task UpdateApplicabilityList(object? result)
        {
            if (result != null && result is ApplicabilityDto dto)
            {
                var newid = await applicabilityService.UpsertApplicability(dto);
                var newDto = await applicabilityService.GetApplicabilityAsync(newid);
                if (newDto != null)
                {
                    UtilityService.UpdateList(ApplicabilityList, newDto);
                }
            }
        }
        private async Task DeleteApplicability()
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить применимость {SelectedApplicability!.Description}?");
            if (result)
            {
                await applicabilityService.DeleteApplicability(SelectedApplicability!.Id);

                ApplicabilityList.Remove(SelectedApplicability);
                SelectedApplicability = null;

                await dialogService.Notify("Удалено", "Применимость удалена");
            }
        }

        //Конструктор
        public ApplicabilityListViewModel(IApplicabilityService applicabilityService, IOriginalService originalService, IDialogService dialogService)
        {
            this.applicabilityService = applicabilityService;
            this.originalService = originalService;
            this.dialogService = dialogService;

            CreateCommand = new AsyncRelayCommand(CreateApplicability);
            DeleteCommand = new AsyncRelayCommand(DeleteApplicability, CanEditDeleteApplicability);
            EditCommand = new AsyncRelayCommand(EditApplicability, CanEditDeleteApplicability);

            LoadApplicabilities();
        }

        //Загрузка данных
        private async void LoadApplicabilities()
        {
            var list = await applicabilityService.GetApplicabilityListAsync();
            list.ForEach(ApplicabilityList.Add);
        }
        private async void LoadApplicabilityDetail()
        {
            if (SelectedApplicability != null)
            {
                var origlist = await originalService.GetOriginalsByCompany(SelectedApplicability.Id);
                OriginalList.Clear();
                origlist.ForEach(OriginalList.Add);
            }
        }
    }
}
