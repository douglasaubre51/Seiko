namespace Seiko.PageModels
{
    public partial class BasePM : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !isBusy;

        [ObservableProperty]
        private string title;

    }
}
