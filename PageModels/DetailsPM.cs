using Seiko.Models;

namespace Seiko.PageModels
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class DetailsPM : BasePM
    {
        [ObservableProperty]
        Monkey monkey;
    }
}
