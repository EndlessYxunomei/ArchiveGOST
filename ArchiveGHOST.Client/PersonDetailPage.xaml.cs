using VMLayer;

namespace ArchiveGHOST.Client;

public partial class PersonDetailPage : ContentPage
{
	public PersonDetailPage(EditPersonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}