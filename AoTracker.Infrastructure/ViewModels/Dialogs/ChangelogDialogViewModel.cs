using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Dialogs.Core;
using AoTracker.Infrastructure.Models.DialogParameters;

namespace AoTracker.Infrastructure.ViewModels.Dialogs
{
    public class ChangelogDialogViewModel : CustomDialogViewModelWithParameterBase<ChangelogDialogParameter>
    {
        private ChangelogDialogParameter _parameter;

        public ChangelogDialogParameter Parameter
        {
            get => _parameter;
            set => Set(ref _parameter, value);
        }

        public override CustomDialogConfig CustomDialogConfig { get; } = new CustomDialogConfig
        {
            Gravity = CustomDialogConfig.DialogGravity.Right | CustomDialogConfig.DialogGravity.Top,
            IsCancellable = true
        };

        protected override void OnDialogAppeared(ChangelogDialogParameter parameter)
        {
            Parameter = parameter;
        }
    }
}
