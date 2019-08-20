using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Activities;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Settings;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.Settings
{
    [NavigationPage(PageIndex.SettingsGeneral)]
    class SettingsGeneralPageFragment : CustomFragmentBase<SettingsGeneralViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_settings_general;

        private List<ImageButton> _accentButtons;
        private List<RadioButton> _radioButtons;

        private bool _updatingAccent;
        private bool _updatingTheme;
        private bool _initialized;

        private AppTheme _selectedTheme;
        private AppTheme _selectedAccent;

        private Dictionary<int, AppTheme> _radioDictionary = new Dictionary<int, AppTheme>
        {
            {Resource.Id.LightThemeRadioButton, AppTheme.Light},
            {Resource.Id.DarkThemeRadioButton, AppTheme.Dark},
            {Resource.Id.BlackThemeRadioButton, AppTheme.Black},
        };

        private Dictionary<int, AppTheme> _accentDictionary = new Dictionary<int, AppTheme>
        {
            {Resource.Id.ColorOrangeAccentButton, AppTheme.Orange},
            {Resource.Id.ColorPurpleAccentButton, AppTheme.Purple},
            {Resource.Id.ColorCyanAccentButton, AppTheme.Cyan},
            {Resource.Id.ColorRedAccentButton, AppTheme.Red},
            {Resource.Id.ColorPinkAccentButton, AppTheme.Pink},
            {Resource.Id.ColorLimeAccentButton, AppTheme.Lime},
            {Resource.Id.ColorBlueAccentButton, AppTheme.SkyBlue},
        };

        protected override void InitBindings()
        {
            _radioButtons = new List<RadioButton>
            {
                LightThemeRadioButton,
                DarkThemeRadioButton,
                BlackThemeRadioButton
            };

            _accentButtons = new List<ImageButton>
            {
                ColorOrangeAccentButton,
                ColorPurpleAccentButton,
                ColorRedAccentButton,
                ColorBlueAccentButton,
                ColorLimeAccentButton,
                ColorPinkAccentButton,
                ColorCyanAccentButton
            };

            foreach (var accentButton in _accentButtons)
            {
                accentButton.SetOnClickListener(new OnClickListener(OnAccentSelected));
            }

            Bindings.Add(this.SetBinding(() => ViewModel.AppTheme).WhenSourceChanges(() =>
            {
                if(ViewModel.AppTheme == 0)
                    return;

                if (_updatingAccent || !_initialized)
                {
                    foreach (var accentButton in _accentButtons)
                    {
                        accentButton.SetImageResource(0);
                    }

                    if ((ViewModel.AppTheme & AppTheme.Orange) == AppTheme.Orange)
                    {
                        ColorOrangeAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Orange;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Lime) == AppTheme.Lime)
                    {
                        ColorLimeAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Lime;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Cyan) == AppTheme.Cyan)
                    {
                        ColorCyanAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Cyan;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Purple) == AppTheme.Purple)
                    {
                        ColorPurpleAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Purple;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.SkyBlue) == AppTheme.SkyBlue)
                    {
                        ColorBlueAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.SkyBlue;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Red) == AppTheme.Red)
                    {
                        ColorRedAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Red;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Pink) == AppTheme.Pink)
                    {
                        ColorPinkAccentButton.SetImageResource(Resource.Drawable.icon_tick);
                        _selectedAccent = AppTheme.Pink;
                    }
                }

                if (_updatingTheme || !_initialized)
                {
                    foreach (var radioButton in _radioButtons)
                    {
                        radioButton.Checked = false;
                    }

                    if ((ViewModel.AppTheme & AppTheme.Dark) == AppTheme.Dark)
                    {
                        DarkThemeRadioButton.Checked = true;
                        _selectedTheme = AppTheme.Dark;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Black) == AppTheme.Black)
                    {
                        BlackThemeRadioButton.Checked = true;
                        _selectedTheme = AppTheme.Black;
                    }
                    else if ((ViewModel.AppTheme & AppTheme.Light) == AppTheme.Light)
                    {
                        LightThemeRadioButton.Checked = true;
                        _selectedTheme = AppTheme.Light;
                    }
                }

                _initialized = true;
            }));

            Bindings.Add(
                this.SetBinding(() => ViewModel.HasThemeChanged,
                    () => ApplyThemeButton.Visibility).ConvertSourceToTarget(BindingConverters.BoolToVisibility));

            ThemeRadioGroup.CheckedChange += ThemeRadioGroupOnCheckedChange;
            ApplyThemeButton.SetOnClickListener(new OnClickListener(view =>
            {
                ViewModel.ApplyThemeCommand.Execute(null);
                MainActivity.Instance.Recreate();
            }));
        }

        private void OnAccentSelected(View view)
        {
            if(_updatingTheme || _updatingAccent)
                return;
            _updatingAccent = true;
            var accent = _accentDictionary[view.Id];
            ViewModel.AppTheme = _selectedTheme | accent;
            _updatingAccent = false;
        }

        private void ThemeRadioGroupOnCheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (_updatingAccent || _updatingTheme || !_initialized)
                return;
            _updatingTheme = true;
            var theme = _radioDictionary[e.CheckedId];
            ViewModel.AppTheme = theme | _selectedAccent;
            _updatingTheme = false;
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        #region Views

        private RadioButton _lightThemeRadioButton;
        private RadioButton _darkThemeRadioButton;
        private RadioButton _blackThemeRadioButton;
        private RadioGroup _themeRadioGroup;
        private ImageButton _colorOrangeAccentButton;
        private ImageButton _colorPurpleAccentButton;
        private ImageButton _colorBlueAccentButton;
        private ImageButton _colorLimeAccentButton;
        private ImageButton _colorPinkAccentButton;
        private ImageButton _colorCyanAccentButton;
        private ImageButton _colorRedAccentButton;
        private Button _applyThemeButton;


        public RadioButton LightThemeRadioButton => _lightThemeRadioButton ?? (_lightThemeRadioButton = FindViewById<RadioButton>(Resource.Id.LightThemeRadioButton));
        public RadioButton DarkThemeRadioButton => _darkThemeRadioButton ?? (_darkThemeRadioButton = FindViewById<RadioButton>(Resource.Id.DarkThemeRadioButton));
        public RadioButton BlackThemeRadioButton => _blackThemeRadioButton ?? (_blackThemeRadioButton = FindViewById<RadioButton>(Resource.Id.BlackThemeRadioButton));
        public RadioGroup ThemeRadioGroup => _themeRadioGroup ?? (_themeRadioGroup = FindViewById<RadioGroup>(Resource.Id.ThemeRadioGroup));
        public ImageButton ColorOrangeAccentButton => _colorOrangeAccentButton ?? (_colorOrangeAccentButton = FindViewById<ImageButton>(Resource.Id.ColorOrangeAccentButton));
        public ImageButton ColorPurpleAccentButton => _colorPurpleAccentButton ?? (_colorPurpleAccentButton = FindViewById<ImageButton>(Resource.Id.ColorPurpleAccentButton));
        public ImageButton ColorBlueAccentButton => _colorBlueAccentButton ?? (_colorBlueAccentButton = FindViewById<ImageButton>(Resource.Id.ColorBlueAccentButton));
        public ImageButton ColorLimeAccentButton => _colorLimeAccentButton ?? (_colorLimeAccentButton = FindViewById<ImageButton>(Resource.Id.ColorLimeAccentButton));
        public ImageButton ColorPinkAccentButton => _colorPinkAccentButton ?? (_colorPinkAccentButton = FindViewById<ImageButton>(Resource.Id.ColorPinkAccentButton));
        public ImageButton ColorCyanAccentButton => _colorCyanAccentButton ?? (_colorCyanAccentButton = FindViewById<ImageButton>(Resource.Id.ColorCyanAccentButton));
        public ImageButton ColorRedAccentButton => _colorRedAccentButton ?? (_colorRedAccentButton = FindViewById<ImageButton>(Resource.Id.ColorRedAccentButton));
        public Button ApplyThemeButton => _applyThemeButton ?? (_applyThemeButton = FindViewById<Button>(Resource.Id.ApplyThemeButton));

        #endregion
    }
}