using AcrhiveModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;

namespace VMLayer
{
    public class PersonDetailViewModel: ObservableValidator, INavigationParameterReceiver
	{
		//сервисы
		private protected readonly IPersonService personService;
		private protected readonly IDialogService dialogService;
		private protected readonly INavigationService navigationService;

		//поля
		private protected string? _firstName;
		private protected string _lastName = string.Empty;
		private protected string? _department;

		//свойства
		[StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
		public string? FirstName
		{
			get => _firstName;
			set => SetProperty(ref _firstName, value, true);
		}
		[Required]
		[StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
		[MinLength(1)]
		public required string LastName
		{
			get => _lastName;
			set => SetProperty(ref _lastName, value, true);
		}
		[StringLength(ArchiveConstants.MAX_PERSON_DEPARTMENT_LENGTH)]
		public string? Department
		{
			get => _department;
			set => SetProperty(ref _department, value, true);
		}

		//списки

		//кнопки
		public IAsyncRelayCommand AcseptCommand { get; }
		public IAsyncRelayCommand CancelCommand { get; }
		internal virtual Task SavePerson()
		{
			ErrorsChanged -= PersonViewModel_ErrorsChanged;
			return Task.CompletedTask;//Заглушка
		}
		private async Task CancelPerson()
		{
			ErrorsChanged -= PersonViewModel_ErrorsChanged;
			await navigationService.GoBack();
		}

		//конструктор
		public PersonDetailViewModel(IPersonService personService, IDialogService dialogService, INavigationService navigationService)
        { 
			this.personService = personService;
			this.dialogService = dialogService;
			this.navigationService = navigationService;

			AcseptCommand = new AsyncRelayCommand(SavePerson, () => !HasErrors);
			CancelCommand = new AsyncRelayCommand(CancelPerson);

			ErrorsChanged += PersonViewModel_ErrorsChanged;

			ValidateAllProperties();//пришлось принудительно запускать валидацию, иначе не работало
		}

		//навигация
		public virtual Task OnNavigatedTo(Dictionary<string, object> parameters)
		{
			return Task.CompletedTask;
		}

		//обработка валидатора
		private void PersonViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
	}
}
