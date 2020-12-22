using Xamarin.Plugin.Calendar.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace Xamarin.Plugin.Calendar.Controls
{
    /// <summary>
    /// Internal class used by Xamarin.Plugin.Calendar
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayView : ContentView
    {
        #region Bindable Properties
        public static readonly BindableProperty DayViewSizeProperty =
            BindableProperty.Create(nameof(DayViewSize), typeof(double), typeof(Calendar), 40.0);

        public double DayViewSize
        {
            get => (double)GetValue(DayViewSizeProperty);
            set => SetValue(DayViewSizeProperty, value);
        }

        public static readonly BindableProperty DayViewCornerRadiusProperty =
            BindableProperty.Create(nameof(DayViewCornerRadius), typeof(float), typeof(Calendar), 20f);

        public float DayViewCornerRadius
        {
            get => (float)GetValue(DayViewCornerRadiusProperty);
            set => SetValue(DayViewCornerRadiusProperty, value);
        }

        public static readonly BindableProperty DaysLabelStyleProperty =
            BindableProperty.Create(nameof(DaysLabelStyle), typeof(Style), typeof(Calendar), Device.Styles.BodyStyle);

        public Style DaysLabelStyle
        {
            get => (Style)GetValue(DaysLabelStyleProperty);
            set => SetValue(DaysLabelStyleProperty, value);
        }
        #endregion


        public DayView()
        {
            InitializeComponent();
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (BindingContext is DayModel dayModel && !dayModel.IsDisabled && dayModel.IsVisible)
            {
                dayModel.IsSelected = true;
                dayModel.DayTappedCommand?.Execute(dayModel.Date);
            }
        }
    }
}