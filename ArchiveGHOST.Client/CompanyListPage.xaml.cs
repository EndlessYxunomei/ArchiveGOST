using VMLayer;

namespace ArchiveGHOST.Client;

public partial class CompanyListPage : ContentPage
{
	public CompanyListPage(CompanyListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}