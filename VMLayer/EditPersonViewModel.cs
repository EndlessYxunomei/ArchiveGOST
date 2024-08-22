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
	public class EditPersonViewModel : PersonDetailViewModel
	{
		//сервисы

		//поля
		private int id;
		private string oldLastName = string.Empty;
		private string? oldFistName;

		//свойства
		//выдачи копий

		//кнопки
		internal override async Task SavePerson()
		{
			//ПЕРЕДЕЛАТЬ НА РЕДАКТИРОВАНИЕ СУЩЕСТВУЮЩЕГО
			if ((LastName == oldLastName && FirstName == oldFistName) || await personService.CheckPersonFullName(LastName, FirstName))
			{
				PersonDetailDto personDetailDto = new()
				{
					LastName = LastName,
					FirstName = FirstName,
					Department = Department,
					Id = id
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
		public EditPersonViewModel(IPersonService personService, IDialogService dialogService, INavigationService navigationService) : base(personService, dialogService, navigationService)
		{
			ErrorExposer = new(this);
		}

		//навигация
		public override async Task OnNavigatedTo(Dictionary<string, object> parameters)
		{
			if (parameters.TryGetValue(NavParamConstants.PersonDetail, out object? per_det) && per_det is int id)
			{
				this.id = id;
				var person = await personService.GetPersonDetail(id);
				oldLastName = person.LastName;
				oldFistName = person.FirstName;
				LastName = person.LastName;
				FirstName = person.FirstName;
				Department = person.Department;
			}
			//await base.OnNavigatedTo(parameters);
		}

		//валидация
		public ValidationErrorExposer ErrorExposer { get; }
	}
}
