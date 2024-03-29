﻿using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer
{
    public class EditOriginalViewModel : OriginalDetailViewModel
    {
        //Сервисы
        private readonly IApplicabilityService applicabilityService;
        private readonly ICorrectionService correctionService;
        
        //Приватные поля
        private int id;
        private int oldInventoryNumber;
        private CopyListDto? _selectedCopy;
        private CorrectionListDto? _selectedCorrection;
        private ApplicabilityDto? _selectedApplicability;

        //Свойства
        public CopyListDto? SelectedCopy
        {
            get => _selectedCopy;
            set
            {
                if (SetProperty(ref _selectedCopy, value))
                {
                    EditCopyCommand.NotifyCanExecuteChanged();
                    DeleteCopyCommand.NotifyCanExecuteChanged();
                };
            }
        }
        public CorrectionListDto? SelectedCorrection
        {
            get => _selectedCorrection;
            set
            {
                if (SetProperty(ref _selectedCorrection, value))
                {
                    EditCorrectionCommand.NotifyCanExecuteChanged();
                    DeleteCorrectionCommand.NotifyCanExecuteChanged();
                };
            }
        }
        public ApplicabilityDto? SelectedApplicability
        {
            get => _selectedApplicability;
            set
            {
                if (SetProperty(ref _selectedApplicability, value))
                {
                    EditApplicabilityCommand.NotifyCanExecuteChanged();
                    DeleteApplicabilityCommand.NotifyCanExecuteChanged();
                };
            }
        }
        public ObservableCollection<CopyListDto> CopyList { get; set; } = [];
        public ObservableCollection<CorrectionListDto> CorrectionList { get; set; } = [];
        public ObservableCollection<ApplicabilityDto> ApplicabilityList { get; set; } = [];

        //Переназанчение метода сохранения
        internal override async Task SaveOriginal()
        {
            //МЕТОД СОЗДАНИЯ НОВОГО ДОКУМЕНТА
            if (InventoryNumber == oldInventoryNumber || await originalService.CheckInventoryNumber(InventoryNumber))
            {
                OriginalDetailDto originalDetailDto = new()
                {
                    Id = id,
                    InventoryNumber = InventoryNumber,
                    Name = Name,
                    Caption = Caption,
                    PageFormat = PageFormat,
                    PageCount = PageCount,
                    Company = Company,
                    Document = Document,
                    Person = Person,
                    Notes = Notes,
                };
                //Пока реализовано так: создаем оригинал и в ответ получаем его id
                int newId = await originalService.UpsertOriginal(originalDetailDto);
                //получает по id дтошку для отображения в списке инвентарной книги
                OriginalListDto newDto = await originalService.GetOriginalAsync(newId);
                await originalService.UpdateOriginalsApplicabilities(id, [.. ApplicabilityList]);
                //Передаём обратно дтошку через мессендже
                //WeakReferenceMessenger.Default.Send(new OriginalUpdatedMessage(newDto));
                //await navigationService.GoBack();
                //Навигация назад с передачей параметра
                await base.SaveOriginal();
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.OriginalList, newDto } });
            }
            else
            {
                await dialogService.Notify("Ошибка сохранения", "Нельзя изменить инвентарный номер на уже существующий");
            }
        }

        //Кнопки
        public IAsyncRelayCommand AddCopyCommand { get; }
        public IAsyncRelayCommand EditCopyCommand { get; }
        public IAsyncRelayCommand DeleteCopyCommand { get; }
        public IAsyncRelayCommand AddCorrectionCommand { get; }
        public IAsyncRelayCommand EditCorrectionCommand { get; }
        public IAsyncRelayCommand DeleteCorrectionCommand { get; }
        public IAsyncRelayCommand AddApplicabilityCommand { get; }
        public IAsyncRelayCommand EditApplicabilityCommand { get; }
        public IAsyncRelayCommand DeleteApplicabilityCommand { get; }
        private Task AddCopy()
        {
            return Task.CompletedTask;
        }
        private Task EditCopy()
        {
            return Task.CompletedTask;
        }
        private Task DeleteCopy()
        {
            return Task.CompletedTask;
            /*var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить экземпляр №{SelectedCopy!.Number}?");
            if (result)
            {
                //Удаление оригинала
                //await originalService.DeleteOriginal(SelectedOriginal.Id);

                //обновление списка
                CopyList.Remove(SelectedCopy);
                SelectedCopy = null;

                await dialogService.Notify("Удалено", "Экземпляр удален");
            }*/
        }
        private async Task AddCorrection()
        {
            await navigationService.GoToCorrectionDetails(new OriginalListDto() { Id = id, OriginalName = Name, OriginalCaption = Caption });
        }
        private async Task EditCorrection()
        {
            await navigationService.GoToCorrectionDetails(new OriginalListDto() { Id = id, OriginalName = Name, OriginalCaption = Caption }, SelectedCorrection!.Id);
        }
        private async Task DeleteCorrection()
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить изменение №{SelectedCorrection!.Number}?");
            if (result)
            {
                //Удаление
                await correctionService.DeleteCorrection(SelectedCorrection.Id);

                //обновление списка
                CorrectionList.Remove(SelectedCorrection);
                SelectedCorrection = null;

                await dialogService.Notify("Удалено", "Изменение удалено");
            }
        }
        private async Task AddApplicability()
        {
            var result = await dialogService.ShowApplicabilityPopup(id);
            await UpsertApplicability(result);
        }
        private async Task EditApplicability()
        {
            var result = await dialogService.ShowApplicabilityPopup(id, SelectedApplicability);
            await UpsertApplicability(result);
        }
        private async Task UpsertApplicability(object? result)
        {
            if (result != null && result is ApplicabilityDto dto)
            {
                UtilityService.UpdateList(ApplicabilityList, dto);
                dto.OriginalId = id;
                await applicabilityService.UpsertApplicability(dto);
                //await applicabilityService.AddOriginalToApplicability(dto.Id, dto.OriginalId);
            }
        }
        private async Task DeleteApplicability()
        {
            var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить применимость {SelectedApplicability!.Description}?\nПрименимость будет удалена только из данного чертежа и может быть выбрана вновь.");
            if (result)
            {
                //Удаление оригинала из списка у применимости
                await applicabilityService.DeleteOriginalFromApplicability(SelectedApplicability.Id, id);

                //обновление списка
                ApplicabilityList.Remove(SelectedApplicability!);
                SelectedApplicability = null;

                await dialogService.Notify("Удалено", "Применимость удалена");
            }
        }
        private bool CanEditDeleteCopy() => SelectedCopy != null;
        private bool CanEditDeleteCorrection() => SelectedCorrection != null;
        private bool CanEditDeleteApplicability() => SelectedApplicability != null;


        //Конструктор
        public EditOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService, IApplicabilityService applicabilityService, ICorrectionService correctionService)
            : base(navigationService, documentService, originalService, personService, companyService, dialogService)
        {
            this.applicabilityService = applicabilityService;
            this.correctionService = correctionService;
            
            ErrorExposer = new(this);

            //Создание кнопок
            AddCopyCommand = new AsyncRelayCommand(AddCopy);
            EditCopyCommand = new AsyncRelayCommand(EditCopy, CanEditDeleteCopy);
            DeleteCopyCommand = new AsyncRelayCommand(DeleteCopy, CanEditDeleteCopy);
            AddCorrectionCommand = new AsyncRelayCommand(AddCorrection);
            EditCorrectionCommand = new AsyncRelayCommand(EditCorrection, CanEditDeleteCorrection);
            DeleteCorrectionCommand = new AsyncRelayCommand(DeleteCorrection, CanEditDeleteCorrection);
            AddApplicabilityCommand = new AsyncRelayCommand(AddApplicability);
            EditApplicabilityCommand = new AsyncRelayCommand(EditApplicability, CanEditDeleteApplicability);
            DeleteApplicabilityCommand = new AsyncRelayCommand(DeleteApplicability, CanEditDeleteApplicability);


            //Подписываемся на событие валидации
            //ErrorsChanged += EditOriginalViewModel_ErrorsChanged;
        }

        //Обработка события валидатора
        //private void EditOriginalViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
        public ValidationErrorExposer ErrorExposer { get; }

        //Обработка навигации
        public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue(NavParamConstants.CorrectionList, out object? cor_list) && cor_list is CorrectionListDto correction)
            {
                UtilityService.UpdateList(CorrectionList, correction);
            }
            if (parameters.TryGetValue(NavParamConstants.OriginalDetail, out object? orig_det) && orig_det is int id)
            {
                await LoadData();
                this.id = id;
                OriginalDetailDto original = await originalService.GetOriginalDetailAsync(id);
                InventoryNumber = original.InventoryNumber;
                oldInventoryNumber = original.InventoryNumber;
                Name = original.Name;
                Caption = original.Caption;
                Notes = original.Notes;
                PageFormat = original.PageFormat;
                PageCount = original.PageCount;

                //БЕЗ ЭТОГО ПИКЕРЫ НЕ ХОТЯТ НОРМАЛЬНО ОТОБРАЖАТЬ НУЖНЫЙ ЭЛЕМЕНТ
                if (original.Company != null) Company = Companylist.FirstOrDefault(x => x.Id == original.Company.Id);
                if (original.Document != null) Document = DocumentList.FirstOrDefault(x => x.Id == original.Document.Id);
                if (original.Person != null) Person = PersonList.FirstOrDefault(x => x.Id == original.Person.Id);

                List<CopyListDto> copyList = original.Copies;
                List<ApplicabilityDto> appList = original.Applicabilities;
                List<CorrectionListDto> corList = original.Corrections;

                //CopyList = new ObservableCollection<CopyListDto>(original.Copies);
                //CorrectionList = new ObservableCollection<CorrectionListDto>(original.Corrections);
                //ApplicabilityList = new ObservableCollection<ApplicabilityDto>(original.Applicabilities);
                //copyList.ForEach(CopyList.Add);
                //appList.ForEach(ApplicabilityList.Add);
                //corList.ForEach(CorrectionList.Add);
                CorrectionList.Clear();
                CopyList.Clear();
                ApplicabilityList.Clear();
                foreach (var cor in corList)
                {
                    UtilityService.UpdateList(CorrectionList, cor);
                }
                foreach (var app in appList)
                {
                    UtilityService.UpdateList(ApplicabilityList, app);
                }
                foreach (var copy in copyList)
                {
                    UtilityService.UpdateList(CopyList, copy);
                }
            }
            await base.OnNavigatedTo(parameters);
        }
    }
}
