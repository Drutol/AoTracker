using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Text;
using Android.Util;

namespace AoTracker.Android.Utils
{
    /// <summary>
    /// Ported from https://github.com/xabaras/RecyclerViewSwipeDecorator
    /// </summary>
    public class RecyclerViewSwipeDecorator
    {
        private readonly Canvas _canvas;
        private readonly RecyclerView _recyclerView;
        private readonly RecyclerView.ViewHolder _viewHolder;
        private readonly float _dX;
        private float _dY;
        private readonly int _actionState;
        private bool _isCurrentlyActive;

        private Color? _swipeLeftBackgroundColor;
        private int _swipeLeftActionIconId;
        private Color? _swipeLeftActionIconTint;

        private Color? _swipeRightBackgroundColor;
        private int _swipeRightActionIconId;
        private Color? _swipeRightActionIconTint;

        private int _iconHorizontalMargin;

        private string _mSwipeLeftText;
        private float _mSwipeLeftTextSize = 14;
        private ComplexUnitType _mSwipeLeftTextUnit = ComplexUnitType.Sp;
        private Color _mSwipeLeftTextColor = Color.DarkGray;
        private Typeface _mSwipeLeftTypeface = Typeface.SansSerif;

        private string _mSwipeRightText;
        private float _mSwipeRightTextSize = 14;
        private ComplexUnitType _mSwipeRightTextUnit = ComplexUnitType.Sp;
        private Color _mSwipeRightTextColor = Color.DarkGray;
        private Typeface _mSwipeRightTypeface = Typeface.SansSerif;

        private RecyclerViewSwipeDecorator()
        {
            _swipeLeftBackgroundColor = null;
            _swipeRightBackgroundColor = null;
            _swipeLeftActionIconId = 0;
            _swipeRightActionIconId = 0;
            _swipeLeftActionIconTint = null;
            _swipeRightActionIconTint = null;
        }

        /**
         * Create a @RecyclerViewSwipeDecorator
         * @param canvas The canvas which RecyclerView is drawing its children
         * @param recyclerView The RecyclerView to which ItemTouchHelper is attached to
         * @param viewHolder The ViewHolder which is being interacted by the User or it was interacted and simply animating to its original position
         * @param dX The amount of horizontal displacement caused by user's action
         * @param dY The amount of vertical displacement caused by user's action
         * @param actionState The type of interaction on the View. Is either ACTION_STATE_DRAG or ACTION_STATE_SWIPE.
         * @param isCurrentlyActive True if this view is currently being controlled by the user or false it is simply animating back to its original state
         */
        public RecyclerViewSwipeDecorator(Canvas canvas, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder,
            float dX, float dY, int actionState, bool isCurrentlyActive) : this()
        {
            this._canvas = canvas;
            this._recyclerView = recyclerView;
            this._viewHolder = viewHolder;
            this._dX = dX;
            this._dY = dY;
            this._actionState = actionState;
            this._isCurrentlyActive = isCurrentlyActive;
            _iconHorizontalMargin = (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, 16,
                recyclerView.Context.Resources.DisplayMetrics);
        }

        /**
         * Set the background color for either (left/right) swipe directions
         * @param backgroundColor The resource id of the background color to be set
         */
        public void SetBackgroundColor(Color backgroundColor)
        {
            _swipeLeftBackgroundColor = backgroundColor;
            _swipeRightBackgroundColor = backgroundColor;
        }

        /**
         * Set the action icon for either (left/right) swipe directions
         * @param actionIconId The resource id of the icon to be set
         */
        public void SetActionIconId(int actionIconId)
        {
            _swipeLeftActionIconId = actionIconId;
            _swipeRightActionIconId = actionIconId;
        }

        /**
         * Set the tColor color for either (left/right) action icons
         * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
         */
        public void SetActionIconTint(Color color)
        {
            SetSwipeLeftActionIconTint(color);
            SetSwipeRightActionIconTint(color);
        }

        /**
         * Set the background color for left swipe direction
         * @param swipeLeftBackgroundColor The resource id of the background color to be set
         */
        public void SetSwipeLeftBackgroundColor(Color swipeLeftBackgroundColor)
        {
            this._swipeLeftBackgroundColor = swipeLeftBackgroundColor;
        }

        /**
         * Set the action icon for left swipe direction
         * @param swipeLeftActionIconId The resource id of the icon to be set
         */
        public void SetSwipeLeftActionIconId(int swipeLeftActionIconId)
        {
            this._swipeLeftActionIconId = swipeLeftActionIconId;
        }

        /**
         * Set the tColor color for action icon drawn while swiping left
         * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
         */
        public void SetSwipeLeftActionIconTint(Color color)
        {
            _swipeLeftActionIconTint = color;
        }

        /**
         * Set the background color for right swipe direction
         * @param swipeRightBackgroundColor The resource id of the background color to be set
         */
        public void SetSwipeRightBackgroundColor(Color swipeRightBackgroundColor)
        {
            this._swipeRightBackgroundColor = swipeRightBackgroundColor;
        }

        /**
         * Set the action icon for right swipe direction
         * @param swipeRightActionIconId The resource id of the icon to be set
         */
        public void SetSwipeRightActionIconId(int swipeRightActionIconId)
        {
            this._swipeRightActionIconId = swipeRightActionIconId;
        }

        /**
         * Set the tColor color for action icon drawn while swiping right
         * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
         */
        public void SetSwipeRightActionIconTint(Color color)
        {
            _swipeRightActionIconTint = color;
        }

        /**
         * Set the label shown when swiping right
         * @param label a String
         */
        public void SetSwipeRightLabel(string label)
        {
            _mSwipeRightText = label;
        }

        /**
         * Set the size of the text shown when swiping right
         * @param unit TypedValue (default is COMPLEX_UNIT_SP)
         * @param size the size value
         */
        public void SetSwipeRightTextSize(ComplexUnitType unit, float size)
        {
            _mSwipeRightTextUnit = unit;
            _mSwipeRightTextSize = size;
        }

        /**
         * Set the color of the text shown when swiping right
         * @param color the color to be set
         */
        public void SetSwipeRightTextColor(Color color)
        {
            _mSwipeRightTextColor = color;
        }

        /**
         * Set the Typeface of the text shown when swiping right
         * @param typeface the Typeface to be set
         */
        public void SetSwipeRightTypeface(Typeface typeface)
        {
            _mSwipeRightTypeface = typeface;
        }

        /**
         * Set the horizontal margin of the icon in the given unit (default is 16dp)
         * @param unit TypedValue
         * @param iconHorizontalMargin the margin in the given unit
         */
        public void SetIconHorizontalMargin(ComplexUnitType unit, int iconHorizontalMargin)
        {
            this._iconHorizontalMargin = (int) TypedValue.ApplyDimension(unit, iconHorizontalMargin,
                _recyclerView.Context.Resources.DisplayMetrics);
        }

        /**
         * Set the label shown when swiping left
         * @param label a String
         */
        public void SetSwipeLeftLabel(string label)
        {
            _mSwipeLeftText = label;
        }

        /**
         * Set the size of the text shown when swiping left
         * @param unit TypedValue (default is COMPLEX_UNIT_SP)
         * @param size the size value
         */
        public void SetSwipeLeftTextSize(ComplexUnitType unit, float size)
        {
            _mSwipeLeftTextUnit = unit;
            _mSwipeLeftTextSize = size;
        }

        /**
         * Set the color of the text shown when swiping left
         * @param color the color to be set
         */
        public void SetSwipeLeftTextColor(Color color)
        {
            _mSwipeLeftTextColor = color;
        }

        /**
         * Set the Typeface of the text shown when swiping left
         * @param typeface the Typeface to be set
         */
        public void SetSwipeLeftTypeface(Typeface typeface)
        {
            _mSwipeLeftTypeface = typeface;
        }

        /**
         * Decorate the RecyclerView item with the chosen backgrounds and icons
         */
        public void Decorate()
        {
            try
            {
                if (_actionState != ItemTouchHelper.ActionStateSwipe) return;

                if (_dX > 0)
                {
                    // Swiping Right
                    if (_swipeRightBackgroundColor != null)
                    {
                        var background = new ColorDrawable(_swipeRightBackgroundColor.Value);
                        background.SetBounds(_viewHolder.ItemView.Left, _viewHolder.ItemView.Top,
                            _viewHolder.ItemView.Left + (int) _dX, _viewHolder.ItemView.Bottom);
                        background.Draw(_canvas);
                    }

                    var iconSize = 0;
                    if (_swipeRightActionIconId != 0 && _dX > _iconHorizontalMargin)
                    {
                        var icon = ContextCompat.GetDrawable(_recyclerView.Context, _swipeRightActionIconId);
                        if (icon != null)
                        {
                            iconSize = icon.IntrinsicHeight;
                            var halfIcon = iconSize / 2;
                            var top = _viewHolder.ItemView.Top +
                                      ((_viewHolder.ItemView.Bottom - _viewHolder.ItemView.Top) / 2 - halfIcon);
                            icon.SetBounds(_viewHolder.ItemView.Left + _iconHorizontalMargin, top,
                                _viewHolder.ItemView.Left + _iconHorizontalMargin + icon.IntrinsicWidth,
                                top + icon.IntrinsicHeight);
                            if (_swipeRightActionIconTint != null)
                                icon.SetColorFilter(_swipeRightActionIconTint.Value, PorterDuff.Mode.SrcIn);
                            icon.Draw(_canvas);
                        }
                    }

                    if (!string.IsNullOrEmpty(_mSwipeRightText) && _dX > _iconHorizontalMargin + iconSize)
                    {
                        var textPaint = new TextPaint
                        {
                            AntiAlias = true,
                            TextSize = TypedValue.ApplyDimension(_mSwipeRightTextUnit, _mSwipeRightTextSize,
                                _recyclerView.Context.Resources.DisplayMetrics),
                            Color = _mSwipeRightTextColor
                        };
                        textPaint.SetTypeface(_mSwipeRightTypeface);

                        var textTop = (int) (_viewHolder.ItemView.Top +
                                             (_viewHolder.ItemView.Bottom - _viewHolder.ItemView.Top) / 2.0 +
                                             textPaint.TextSize / 2);
                        _canvas.DrawText(_mSwipeRightText,
                            _viewHolder.ItemView.Left + _iconHorizontalMargin + iconSize +
                            (iconSize > 0 ? _iconHorizontalMargin / 2 : 0), textTop, textPaint);
                    }
                }
                else if (_dX < 0)
                {
                    // Swiping Left
                    if (_swipeLeftBackgroundColor != null)
                    {
                        var background = new ColorDrawable(_swipeLeftBackgroundColor.Value);
                        background.SetBounds(_viewHolder.ItemView.Right + (int) _dX, _viewHolder.ItemView.Top,
                            _viewHolder.ItemView.Right, _viewHolder.ItemView.Bottom);
                        background.Draw(_canvas);
                    }

                    var iconSize = 0;
                    var imgLeft = _viewHolder.ItemView.Right;
                    if (_swipeLeftActionIconId != 0 && _dX < -_iconHorizontalMargin)
                    {
                        var icon = ContextCompat.GetDrawable(_recyclerView.Context, _swipeLeftActionIconId);
                        if (icon != null)
                        {
                            iconSize = icon.IntrinsicHeight;
                            var halfIcon = iconSize / 2;
                            var top = _viewHolder.ItemView.Top +
                                      ((_viewHolder.ItemView.Bottom - _viewHolder.ItemView.Top) / 2 - halfIcon);
                            imgLeft = _viewHolder.ItemView.Right - _iconHorizontalMargin - halfIcon * 2;
                            icon.SetBounds(imgLeft, top, _viewHolder.ItemView.Right - _iconHorizontalMargin,
                                top + icon.IntrinsicHeight);
                            if (_swipeLeftActionIconTint != null)
                                icon.SetColorFilter(_swipeLeftActionIconTint.Value, PorterDuff.Mode.SrcIn);
                            icon.Draw(_canvas);
                        }
                    }

                    if (!string.IsNullOrEmpty(_mSwipeLeftText) && _dX < -_iconHorizontalMargin - iconSize)
                    {
                        var textPaint = new TextPaint
                        {
                            AntiAlias = true,
                            TextSize = TypedValue.ApplyDimension(_mSwipeLeftTextUnit, _mSwipeLeftTextSize,
                                _recyclerView.Context.Resources.DisplayMetrics),
                            Color = _mSwipeLeftTextColor
                        };
                        textPaint.SetTypeface(_mSwipeLeftTypeface);

                        var width = textPaint.MeasureText(_mSwipeLeftText);
                        var textTop = (int) (_viewHolder.ItemView.Top +
                                             (_viewHolder.ItemView.Bottom - _viewHolder.ItemView.Top) / 2.0 +
                                             textPaint.TextSize / 2);
                        _canvas.DrawText(_mSwipeLeftText,
                            imgLeft - width - (imgLeft == _viewHolder.ItemView.Right
                                ? _iconHorizontalMargin
                                : _iconHorizontalMargin / 2), textTop, textPaint);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /**
         * A Builder for the RecyclerViewSwipeDecorator class
         */
        public class Builder
        {
            private readonly RecyclerViewSwipeDecorator _mDecorator;

            /**
             * Create a builder for a RecyclerViewsSwipeDecorator
             * @param canvas The canvas which RecyclerView is drawing its children
             * @param recyclerView The RecyclerView to which ItemTouchHelper is attached to
             * @param viewHolder The ViewHolder which is being interacted by the User or it was interacted and simply animating to its original position
             * @param dX The amount of horizontal displacement caused by user's action
             * @param dY The amount of vertical displacement caused by user's action
             * @param actionState The type of interaction on the View. Is either ACTION_STATE_DRAG or ACTION_STATE_SWIPE.
             * @param isCurrentlyActive True if this view is currently being controlled by the user or false it is simply animating back to its original state
             */
            public Builder(Canvas canvas, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                _mDecorator = new RecyclerViewSwipeDecorator(
                    canvas,
                    recyclerView,
                    viewHolder,
                    dX,
                    dY,
                    actionState,
                    isCurrentlyActive
                );
            }

            /**
             * Add a background color to both swiping directions
             * @param color A single color value in the form 0xAARRGGBB
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddBackgroundColor(Color color)
            {
                _mDecorator.SetBackgroundColor(color);
                return this;
            }

            /**
             * Add an action icon to both swiping directions
             * @param drawableId The resource id of the icon to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddActionIcon(int drawableId)
            {
                _mDecorator.SetActionIconId(drawableId);
                return this;
            }

            /**
             * Set the tColor color for either (left/right) action icons
             * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetActionIconTint(Color color)
            {
                _mDecorator.SetActionIconTint(color);
                return this;
            }

            /**
             * Add a background color while swiping right
             * @param color A single color value in the form 0xAARRGGBB
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeRightBackgroundColor(Color color)
            {
                _mDecorator.SetSwipeRightBackgroundColor(color);
                return this;
            }

            /**
             * Add an action icon while swiping right
             * @param drawableId The resource id of the icon to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeRightActionIcon(int drawableId)
            {
                _mDecorator.SetSwipeRightActionIconId(drawableId);
                return this;
            }

            /**
             * Set the tColor color for action icon shown while swiping right
             * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeRightActionIconTint(Color color)
            {
                _mDecorator.SetSwipeRightActionIconTint(color);
                return this;
            }

            /**
             * Add a label to be shown while swiping right
             * @param label The string to be shown as label
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeRightLabel(string label)
            {
                _mDecorator.SetSwipeRightLabel(label);
                return this;
            }

            /**
             * Set the color of the label to be shown while swiping right
             * @param color the color to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeRightLabelColor(Color color)
            {
                _mDecorator.SetSwipeRightTextColor(color);
                return this;
            }

            /**
             * Set the size of the label to be shown while swiping right
             * @param unit the unit to convert from
             * @param size the size to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeRightLabelTextSize(ComplexUnitType unit, float size)
            {
                _mDecorator.SetSwipeRightTextSize(unit, size);
                return this;
            }

            /**
             * Set the Typeface of the label to be shown while swiping right
             * @param typeface the Typeface to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeRightLabelTypeface(Typeface typeface)
            {
                _mDecorator.SetSwipeRightTypeface(typeface);
                return this;
            }

            /**
             * Adds a background color while swiping left
             * @param color A single color value in the form 0xAARRGGBB
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeLeftBackgroundColor(Color color)
            {
                _mDecorator.SetSwipeLeftBackgroundColor(color);
                return this;
            }

            /**
             * Add an action icon while swiping left
             * @param drawableId The resource id of the icon to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeLeftActionIcon(int drawableId)
            {
                _mDecorator.SetSwipeLeftActionIconId(drawableId);
                return this;
            }

            /**
             * Set the tColor color for action icon shown while swiping left
             * @param color a color in ARGB format (e.g. 0xFF0000FF for blue)
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeLeftActionIconTint(Color color)
            {
                _mDecorator.SetSwipeLeftActionIconTint(color);
                return this;
            }

            /**
             * Add a label to be shown while swiping left
             * @param label The string to be shown as label
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder AddSwipeLeftLabel(string label)
            {
                _mDecorator.SetSwipeLeftLabel(label);
                return this;
            }

            /**
             * Set the color of the label to be shown while swiping left
             * @param color the color to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeLeftLabelColor(Color color)
            {
                _mDecorator.SetSwipeLeftTextColor(color);
                return this;
            }

            /**
             * Set the size of the label to be shown while swiping left
             * @param unit the unit to convert from
             * @param size the size to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeLeftLabelTextSize(ComplexUnitType unit, float size)
            {
                _mDecorator.SetSwipeLeftTextSize(unit, size);
                return this;
            }

            /**
             * Set the Typeface of the label to be shown while swiping left
             * @param typeface the Typeface to be set
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetSwipeLeftLabelTypeface(Typeface typeface)
            {
                _mDecorator.SetSwipeLeftTypeface(typeface);
                return this;
            }

            /**
             * Set the horizontal margin of the icon in the given unit (default is 16dp)
             * @param unit TypedValue
             * @param iconHorizontalMargin the margin in the given unit
             *
             * @return This instance of @RecyclerViewSwipeDecorator.Builder
             */
            public Builder SetIconHorizontalMargin(ComplexUnitType unit, int iconHorizontalMargin)
            {
                _mDecorator.SetIconHorizontalMargin(unit, iconHorizontalMargin);
                return this;
            }

            /**
             * Create a RecyclerViewSwipeDecorator
             * @return The created @RecyclerViewSwipeDecorator
             */
            public RecyclerViewSwipeDecorator Create()
            {
                return _mDecorator;
            }
        }
    }
}