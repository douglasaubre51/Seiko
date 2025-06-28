using Seiko.PageModels;

namespace Seiko.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPagePM mainPagePM)
    {
        InitializeComponent();

        BindingContext = mainPagePM;
    }
}