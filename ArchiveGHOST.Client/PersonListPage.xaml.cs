using VMLayer;

namespace ArchiveGHOST.Client;

public partial class PersonListPage : ContentPage
{
	public PersonListPage(PersonListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}