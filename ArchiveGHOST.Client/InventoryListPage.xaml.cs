using VMLayer;
namespace ArchiveGHOST.Client;


public partial class InventoryListPage : ContentPage
{
	public InventoryListPage()
	{
		InitializeComponent();
		BindingContext = new OriginalListViewModel();
	}
}