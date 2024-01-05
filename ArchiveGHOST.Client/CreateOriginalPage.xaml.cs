using VMLayer;

namespace ArchiveGHOST.Client;

public partial class CreateOriginalPage : ContentPage
{
	public CreateOriginalPage(CreateOriginalViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}