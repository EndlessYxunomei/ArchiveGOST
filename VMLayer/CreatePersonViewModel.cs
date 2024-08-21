using AcrhiveModels;
using AcrhiveModels.DTOs;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;
using VMLayer.Validation;

namespace VMLayer
{
	public class CreatePersonViewModel : PersonDetailViewModel
	{
		//переопределение сохранения
		internal override async Task SavePerson()
		{
			if (await personService.CheckPersonFullName(LastName, FirstName))
			{
				PersonDetailDto personDetailDto = new()
				{ 
					LastName = LastName,
					FirstName = FirstName,
					Department = Department
				};

				int newId = await personService.UpsertPerson(personDetailDto);

				PersonListDto newDto = await personService.GetPerson(newId);

				await base.SavePerson();
				await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.PersonList, newDto } });
			}
			else
			{
				await dialogService.Notify("Ошибка добавления", "Данный пользователь уже существует");
			}
		}

		//конструктор
		public CreatePersonViewModel(IPersonService personService, IDialogService dialogService, INavigationService navigationService) : base(personService, dialogService, navigationService)
		{
			ErrorExposer = new(this);
		}

		//валидация
		public ValidationErrorExposer ErrorExposer { get; }
	}
}
