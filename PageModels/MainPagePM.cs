using CommunityToolkit.Mvvm.Input;
using Seiko.Models;
using Seiko.Pages;
using Seiko.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Seiko.PageModels
{
    public partial class MainPagePM : BasePM
    {
        // common api
        IConnectivity _connectivity;
        IGeolocation _gelocation;

        MonkeyDetailsService _monkeyService;
        public ObservableCollection<Monkey> Monkeys { get; } = new();

        [ObservableProperty]
        bool isRefreshing;

        public MainPagePM(MonkeyDetailsService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkeys";

            _monkeyService = monkeyService;
            _connectivity = connectivity;
            _gelocation = geolocation;
        }

        [RelayCommand]
        async Task GetClosestMonkeyAsync()
        {
            if (IsBusy || Monkeys.Count == 0) return;

            try
            {
                IsBusy = true;

                var location = await _gelocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await _gelocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Best,
                            Timeout = TimeSpan.FromSeconds(30)
                        }
                        );
                }

                if (location == null) return;

                var first = Monkeys.OrderBy(
                    m => location.CalculateDistance(
                        m.Latitude,
                        m.Longitude,
                        DistanceUnits.Miles
                        )
                    ).FirstOrDefault();

                if (first is null) return;

                IsBusy = false;
                await Shell.Current.DisplayAlertAsync("found", $"closest monkey is {first.Name} at {first.Location}", "ok");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlertAsync("Gelocation error", "unable to fetch nearest monkey!", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToMonkeyDetailsAsync(Monkey monkey)
        {
            if (monkey is null) return;

            await Shell.Current.GoToAsync(
                $"{nameof(DetailsPage)}",
                true,
                new Dictionary<string, object>
                {
                    {"Monkey",monkey}
                }
                );
        }

        [RelayCommand]
        async Task GetMonkeyDetailsAsync()
        {
            // handle button mashing
            if (IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlertAsync("Connection error", "internet disconnected!", "ok");
                    return;
                }

                IsBusy = true;
                var monkeyDetails = await _monkeyService.GetDetails();

                // refresh list
                if (Monkeys.Count != 0)
                    Monkeys.Clear();

                foreach (var monkey in monkeyDetails)
                    Monkeys.Add(monkey);
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlertAsync("error", "error fetching monkeys!", "ok");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
