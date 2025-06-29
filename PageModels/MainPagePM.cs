using CommunityToolkit.Mvvm.Input;
using Seiko.Models;
using Seiko.Pages;
using Seiko.Services;
using System.Collections.ObjectModel;

namespace Seiko.PageModels
{
    public partial class MainPagePM : BasePM
    {
        MonkeyDetailsService _monkeyService;

        public ObservableCollection<Monkey> Monkeys { get; } = new();

        public MainPagePM(MonkeyDetailsService monkeyService)
        {
            Title = "Details";
            _monkeyService = monkeyService;
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
            }
        }
    }
}
