using VMLayer;

namespace ArchiveGHOST.Client;

public partial class OriginalDetailPage : ContentPage
{
	public OriginalDetailPage(OriginalDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}