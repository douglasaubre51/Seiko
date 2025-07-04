using CommunityToolkit.Mvvm.Input;
using Seiko.Models;
using System.Diagnostics;

namespace Seiko.PageModels
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class DetailsPM : BasePM
    {
        [ObservableProperty]
        Monkey monkey;

        IMap _map;

        public DetailsPM(IMap map)
        {
            _map = map;
        }

        [RelayCommand]
        async Task OpenMapAsync()
        {
            try
            {
                await _map.OpenAsync(
                    Monkey.Latitude,
                    Monkey.Longitude,
                    new MapLaunchOptions
                    {
                        Name = Monkey.Name,
                        NavigationMode = NavigationMode.None
                    }
                    );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlertAsync("Map error", "error loading map!", "ok");
                return;
            }

            return;
        }
    }
}
