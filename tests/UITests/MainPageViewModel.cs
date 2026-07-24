using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jamjardavies.Icon.Maui.FontAwesome;

namespace UITests;

internal partial class MainPageViewModel : ObservableObject
{
    #region Properties

    [ObservableProperty] public partial bool Triggered { get; set; }

    [ObservableProperty] public partial FontAwesomeIcon Test { get; set; } = FontAwesomeIcon.FontAwesome;

    #endregion

    #region Methods

    #region Private

    [RelayCommand]
    private void ToggleTriggered()
    {
        Triggered = !Triggered;

        Test = Triggered switch
        {
            true => FontAwesomeIcon.AddressBook,
            false => FontAwesomeIcon.FontAwesome
        };
    }

    #endregion

    #endregion
}