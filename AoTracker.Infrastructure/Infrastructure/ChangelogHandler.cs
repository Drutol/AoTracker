using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Dialogs.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models.DialogParameters;
using AoTracker.Interfaces;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class ChangelogHandler : IInitializable
    {
        private readonly ILogger<ChangelogHandler> _logger;
        private readonly IDispatcherAdapter _dispatcherAdapter;
        private readonly ICustomDialogsManager<DialogIndex> _dialogsManager;

        public ChangelogHandler(
            ILogger<ChangelogHandler> logger,
            IDispatcherAdapter dispatcherAdapter,
            ICustomDialogsManager<DialogIndex> dialogsManager)
        {
            _logger = logger;
            _dispatcherAdapter = dispatcherAdapter;
            _dialogsManager = dialogsManager;
        }

        public Task Initialize()
        {
            VersionTracking.Track();

            if (VersionTracking.IsFirstLaunchForCurrentVersion && !VersionTracking.IsFirstLaunchEver)
            {
                var version = new Version(VersionTracking.CurrentVersion);
                var prevVersion = new Version(VersionTracking.PreviousVersion);

                if (version.Major == prevVersion.Major &&
                    version.Minor == prevVersion.Minor &&
                    version.Build == prevVersion.Build &&
                    version.Revision != prevVersion.Revision)
                    return Task.CompletedTask;

                _dispatcherAdapter.Run(() =>
                {
                    try
                    {
                        _dialogsManager[DialogIndex.ChangelogDialog].Show(new ChangelogDialogParameter
                        {
                            Changelog = _changelog,
                            Date = _date,
                            Note = _note
                        });
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to show changelog dialog. Debug launch?");
                    }

                });
            }

            return Task.CompletedTask;
        }

        private string _date = "04.09.2019";
        private string _note = "Test changelog.";
        private List<string> _changelog = new List<string>
        {
            "Fix initial crashes on non Android P devices.",
            "Various background restoration tweaks."
        };
    }
}
