using Inspeção_Empresarial.ViewModels.Inspection;

namespace Inspeção_Empresarial.Views.Inspection;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Search.Focus();
    }
}