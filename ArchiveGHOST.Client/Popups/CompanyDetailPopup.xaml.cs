using CommunityToolkit.Maui.Views;
using VMLayer;

namespace ArchiveGHOST.Client.Popups;

public partial class CompanyDetailPopup : Popup
{
	public CompanyDetailPopup(CompanyDetailViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}