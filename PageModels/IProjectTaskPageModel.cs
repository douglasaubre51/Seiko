using CommunityToolkit.Mvvm.Input;
using Seiko.Models;

namespace Seiko.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}