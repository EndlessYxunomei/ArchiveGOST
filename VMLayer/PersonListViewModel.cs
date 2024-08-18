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
using VMLayer.Navigation;

namespace VMLayer
{
	public class PersonListViewModel : ObservableObject, INavigationParameterReceiver
	{
		//серивисы
		private readonly IDialogService dialogService;
		private readonly INavigationService navigationService;
		private readonly IPersonService personService;

		//поля
		private PersonListDto? _selectedPerson;

		//свойства
		public PersonListDto? SelectedPerson
		{
			get => _selectedPerson;
			set
			{
				if (SetProperty(ref _selectedPerson, value))
				{
					DeleteCommand.NotifyCanExecuteChanged();
					EditCommand.NotifyCanExecuteChanged();
				}
			}
		}
		public ObservableCollection<PersonListDto> PersonList { get; set; } = [];

		//кнопки
		public IAsyncRelayCommand CreateCommand { get; }
		public IAsyncRelayCommand DeleteCommand { get; }
		public IAsyncRelayCommand EditCommand { get; }
		private bool CanEditDeletePerson() => SelectedPerson != null;
		private async Task CreatePerson() => await navigationService.GoToCreatePerson();

		private async Task EditPerson()
		{
			if (SelectedPerson != null)
			{
				await navigationService.GoToPersonDetails(SelectedPerson.Id);
			}
		}
		private async Task DeletePerson()
		{
			if (SelectedPerson != null)
			{
				var result = await dialogService.AskYesNo("Удаление данных", $"Вы действительно хотите удалить пользователя {SelectedPerson.FullName}?");
				if (result)
				{
					//Удаление оригинала
					await personService.DeletePerson(SelectedPerson.Id);

					//обновление списка
					PersonList.Remove(SelectedPerson);
					SelectedPerson = null;

					await dialogService.Notify("Удалено", "Пользователь удалён");
				}
			}
		}

		//конструктор
		public PersonListViewModel(IPersonService personService, IDialogService dialogService, INavigationService navigationService)
		{
			this.personService = personService;
			this.dialogService = dialogService;
			this.navigationService = navigationService;

			CreateCommand = new AsyncRelayCommand(CreatePerson);
			EditCommand = new AsyncRelayCommand(EditPerson, CanEditDeletePerson);
			DeleteCommand = new AsyncRelayCommand(DeletePerson, CanEditDeletePerson);

			LoadPersonListAsync();
		}

		//загрузка данных
		private async void LoadPersonListAsync()
		{
			var perList = await personService.GetPersonList();
			perList.ForEach(PersonList.Add);
		}

		//обработка навигации
		public Task OnNavigatedTo(Dictionary<string, object> parameters)
		{
			if (parameters.TryGetValue(NavParamConstants.PersonList, out object? per_list) && per_list is PersonListDto personListDto)
			{
				UtilityService.UpdateList(PersonList, personListDto);
			}
			return Task.CompletedTask;
		}
	}
}
