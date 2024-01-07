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

public class CreateOriginalViewModel: OriginalDetailViewModel
{
    //Кнопки
    public IAsyncRelayCommand AcseptCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

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
    }

    //Конструктор
    public CreateOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService)
        :base(navigationService,documentService, originalService, personService, companyService,dialogService)
    {
        ErrorExposer = new(this);
        
        AcseptCommand = new AsyncRelayCommand(CreateOriginal, () => !HasErrors);
        CancelCommand = new AsyncRelayCommand(CancelCreate);

        //Подписываемся на событие валидации
        ErrorsChanged += CreateOriginalViewModel_ErrorsChanged;

        _ = LoadData();
        //ValidateAllProperties();//пришлось принудительно запускать валидацию, иначе не работало
    }
    
    //Пероначальная загрузка данных
    internal override  async Task LoadData()
    {
        var newInventoryNumber = await originalService.GetLustInventoryNumber();
        InventoryNumber = newInventoryNumber + 1;

        await base.LoadData();
    }

    //Обработка события валидатора
    private void CreateOriginalViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) =>AcseptCommand.NotifyCanExecuteChanged();
    public ValidationErrorExposer ErrorExposer { get; }

    //Обработка навигации
    public override Task OnNavigatedTo(Dictionary<string, object> parameters)
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
