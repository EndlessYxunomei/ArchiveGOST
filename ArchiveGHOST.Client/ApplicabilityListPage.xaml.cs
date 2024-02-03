using VMLayer;

namespace ArchiveGHOST.Client;

public partial class ApplicabilityListPage : ContentPage
{
	public ApplicabilityListPage(ApplicabilityListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}