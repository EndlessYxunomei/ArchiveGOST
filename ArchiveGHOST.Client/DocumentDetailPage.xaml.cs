using VMLayer;

namespace ArchiveGHOST.Client;

public partial class DocumentDetailPage : ContentPage
{
	public DocumentDetailPage(DocumentDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}