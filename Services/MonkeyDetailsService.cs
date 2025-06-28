using Seiko.Models;
using System.Net.Http.Json;

namespace Seiko.Services
{
    public class MonkeyDetailsService
    {
        HttpClient httpClient;
        List<Monkey> monkeyList = new();

        public MonkeyDetailsService() =>
            httpClient = new HttpClient();

        public async Task<List<Monkey>> GetDetails()
        {
            if (monkeyList?.Count > 0)
                return monkeyList;

            var url = "https://montemagno.com/monkeys.json";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            }

            return monkeyList;
        }
    }
}
