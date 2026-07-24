namespace UITests;

public partial class MainPage
{
    public MainPage()
    {
        BindingContext = new MainPageViewModel();

        InitializeComponent();
    }
}