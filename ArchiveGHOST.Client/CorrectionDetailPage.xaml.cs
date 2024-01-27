using VMLayer;

namespace ArchiveGHOST.Client;

public partial class CorrectionDetailPage : ContentPage
{
	public CorrectionDetailPage(CorrectionDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}