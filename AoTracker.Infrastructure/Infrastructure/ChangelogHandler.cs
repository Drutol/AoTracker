using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Dialogs.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models.DialogParameters;
using AoTracker.Interfaces;
using Xamarin.Essentials;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class ChangelogHandler : IInitializable
    {
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ICustomDialogsManager<DialogIndex> _dialogsManager;

        public ChangelogHandler(
            IDispatcherAdapter dispatcherAdapter,
            ICustomDialogsManager<DialogIndex> dialogsManager)
        {
            _dispatcherAdapter = dispatcherAdapter;
            _dialogsManager = dialogsManager;
        }

        public Task Initialize()
        {
            VersionTracking.Track();

            if (VersionTracking.IsFirstLaunchForCurrentVersion)
            {
                _dispatcherAdapter.Run(() =>
                {
                    _dialogsManager[DialogIndex.ChangelogDialog].Show(new ChangelogDialogParameter
                    {
                        Changelog = _changelog,
                        Date = _date,
                        Note = _note
                    });
                });
            }

            return Task.CompletedTask;
        }

        private string _date = "31.08.2019";
        private string _note = "Test changelog.";
        private List<string> _changelog = new List<string>
        {
            "Initial release."
        };
    }
}
