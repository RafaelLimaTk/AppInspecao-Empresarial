using CommunityToolkit.Maui.Views;
using Inspeção_Empresarial.ViewModels.Inspection;
using Microsoft.Maui.Platform;

namespace Inspeção_Empresarial.Views.Inspection;

public partial class CreateAndEditPage : ContentPage
{
	public CreateAndEditPage()
	{
		InitializeComponent();
	}

    private void OnFirstExpanderTapped(object sender, EventArgs e)
    {
        OnExpanderTapped(sender, e, 1);
    }

    private void OnSecondExpanderTapped(object sender, EventArgs e)
    {
        OnExpanderTapped(sender, e, 2);
    }

    private void OnThirdExpanderTapped(object sender, EventArgs e)
    {
        OnExpanderTapped(sender, e, 3);
    }

    private void OnFourthExpanderTapped(object sender, EventArgs e)
    {
        OnExpanderTapped(sender, e, 4);
    }

    private void OnExpanderTapped(object sender, EventArgs e, int expanderId)
    {
        Image arrowImage;
        switch (expanderId)
        {
            case 1: arrowImage = ArrowFirstImage; break;
            case 2: arrowImage = ArrowSecondImage; break;
            case 3: arrowImage = ArrowThirdImage; break;
            case 4: arrowImage = ArrowFourthImage; break;
            default: throw new ArgumentOutOfRangeException(nameof(expanderId));
        }
        ToggleExpander(expanderId, arrowImage);
    }

    private async void ToggleExpander(int expanderId, Image arrowImage)
    {
        var newRotation = arrowImage.Rotation == 0 ? 180 : 0;
        await arrowImage.RotateTo(newRotation, 250);

        var viewModel = BindingContext as CreateAndEditPageViewModel;
        if (viewModel == null) return;

        switch (expanderId)
        {
            case 1: viewModel.IsFirstExpanderOpen = !viewModel.IsFirstExpanderOpen; break;
            case 2: viewModel.IsSecondExpanderOpen = !viewModel.IsSecondExpanderOpen; break;
            case 3: viewModel.IsThirdExpanderOpen = !viewModel.IsThirdExpanderOpen; break;
            case 4: viewModel.IsFourthExpanderOpen = !viewModel.IsFourthExpanderOpen; break;
            default: throw new ArgumentOutOfRangeException(nameof(expanderId));
        }
    }

    private async void OnExpanderExpandedChanged(object sender, EventArgs e)
    {
        if (!(sender is Expander expander)) return;

        var content = expander.Content as VisualElement;
        if (content == null) return;

        if (expander.IsExpanded)
        {
            content.Opacity = 0;
            await content.FadeTo(1, 500); 
        }
        else
        {
            await content.FadeTo(0, 500);                                      
        }
    }
}