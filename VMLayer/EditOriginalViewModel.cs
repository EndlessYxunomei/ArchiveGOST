﻿using AcrhiveModels.DTOs;
using AcrhiveModels.Interfaces;
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
        //Приватные поля
        private int _id;
        private CopyListDto? _selectedCopy;
        private CorrectionListDto? _selectedCorrection;
        private ApplicabilityListDto? _selectedApplicability;

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
        public ApplicabilityListDto? SelectedApplicability
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
        public ObservableCollection<ApplicabilityListDto> ApplicabilityList { get; set; } = [];

        //Переназанчение метода сохранения
        internal override Task SaveOriginal()
        {
            //МЕТОД СОЗДАНИЯ НОВОГО ДОКУМЕНТА
            /*if (await originalService.CheckInventoryNumber(InventoryNumber))
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
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.OrginalList, newDto } });
            }
            else
            {
                await dialogService.Notify("Ошибка добавления", "Данный инвентарный номер уже существует");
            }*/
            return Task.CompletedTask;
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
        private Task AddCorrection()
        {
            return Task.CompletedTask;
        }
        private Task EditCorrection()
        {
            return Task.CompletedTask;
        }
        private Task DeleteCorrection()
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
        private Task AddApplicability()
        {
            return Task.CompletedTask;
        }
        private Task EditApplicability()
        {
            return Task.CompletedTask;
        }
        private Task DeleteApplicability()
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
        private bool CanEditDeleteCopy() => SelectedCopy != null;
        private bool CanEditDeleteCorrection() => SelectedCorrection != null;
        private bool CanEditDeleteApplicability() => SelectedApplicability != null;


        //Конструктор
        public EditOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService)
            : base(navigationService, documentService, originalService, personService, companyService, dialogService)
        {
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
            ErrorsChanged += EditOriginalViewModel_ErrorsChanged;
        }

        //Обработка события валидатора
        private void EditOriginalViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
        public ValidationErrorExposer ErrorExposer { get; }

        //Обработка навигации
        public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue(NavParamConstants.OriginalDetail, out object? orig_det) && orig_det is int id)
            {
                await LoadData();
                _id = id;
                OriginalDetailDto original = await originalService.GetOriginalDetailAsync(id);
                InventoryNumber = original.InventoryNumber;
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
                List<ApplicabilityListDto> appList = original.Applicabilities;
                List<CorrectionListDto> corList = original.Corrections;
                copyList.ForEach(CopyList.Add);
                appList.ForEach(ApplicabilityList.Add);
                corList.ForEach(CorrectionList.Add);
            }
            await base.OnNavigatedTo(parameters);
        }

        //Обновление коллекций
        /*private static void UpdateList<T>(ObservableCollection<T> list, T updateItem) where T:IIdentityModel
        {
            T? exist = list.FirstOrDefault(x => x?.Id == updateItem.Id);
            if (exist != null)
            {
                list[list.IndexOf(exist)] = updateItem;
            }
            else
            {
                list.Add(updateItem);
            }
        }*/
    }
}
