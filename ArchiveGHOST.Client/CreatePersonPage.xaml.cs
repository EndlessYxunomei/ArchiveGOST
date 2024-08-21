using VMLayer;

namespace ArchiveGHOST.Client;

public partial class CreatePersonPage : ContentPage
{
	public CreatePersonPage(CreatePersonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}