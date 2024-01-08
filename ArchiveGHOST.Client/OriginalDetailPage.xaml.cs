using VMLayer;

namespace ArchiveGHOST.Client;

public partial class OriginalDetailPage : ContentPage
{
	public OriginalDetailPage(EditOriginalViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}