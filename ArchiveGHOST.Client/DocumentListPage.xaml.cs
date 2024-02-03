using VMLayer;

namespace ArchiveGHOST.Client;

public partial class DocumentListPage : ContentPage
{
	public DocumentListPage(DocumentListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}