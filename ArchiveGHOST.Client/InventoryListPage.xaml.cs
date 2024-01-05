using VMLayer;
namespace ArchiveGHOST.Client;


public partial class InventoryListPage : ContentPage
{
	public InventoryListPage(OriginalListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}