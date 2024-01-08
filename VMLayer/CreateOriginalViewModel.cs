using AcrhiveModels.DTOs;
using ServiceLayer;
using System.ComponentModel;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer;

public class CreateOriginalViewModel: OriginalDetailViewModel
{
    //Переназанчение метода сохранения
    internal override async Task SaveOriginal()
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

    //Конструктор
    public CreateOriginalViewModel(INavigationService navigationService, IDocumentService documentService, IOriginalService originalService, IPersonService personService, ICompanyService companyService, IDialogService dialogService)
        :base(navigationService,documentService, originalService, personService, companyService,dialogService)
    {
        ErrorExposer = new(this);

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
}
