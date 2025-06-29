using Seiko.PageModels;

namespace Seiko.Pages;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(DetailsPM detailsPM)
    {
        InitializeComponent();
        BindingContext = detailsPM;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}