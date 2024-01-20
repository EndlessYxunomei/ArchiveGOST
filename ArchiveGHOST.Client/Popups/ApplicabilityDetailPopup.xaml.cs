using CommunityToolkit.Maui.Views;
using VMLayer;

namespace ArchiveGHOST.Client.Popups;

public partial class ApplicabilityDetailPopup : Popup
{
	public ApplicabilityDetailPopup(ApplicabilityDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}