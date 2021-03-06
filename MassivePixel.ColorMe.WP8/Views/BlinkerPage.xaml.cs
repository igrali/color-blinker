﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Phone.Tasks;

namespace MassivePixel.ColorMe.WP8.Views
{
    public partial class BlinkerPage
    {
        private Timer _timer;
        private const double Interval = 175;

        public BlinkerPage()
        {
            InitializeComponent();
            DataContext = App.SelectedColor;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StartTimer();

            ExtendedVisualStateManager.GoToState(this, "Entering", true);
            await Task.Delay(TimeSpan.FromSeconds(3));
            ExtendedVisualStateManager.GoToState(this, "Exiting", true);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _timer.Dispose();
        }

        private void OnTimer(object state)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ColoredRectangle.Visibility = ColoredRectangle.Visibility != Visibility.Visible
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            });
        }

        private void Sms_Click(object sender, EventArgs e)
        {
            try
            {
                var task = new SmsComposeTask
                {
                    Body = string.Format("I am blinking {0}! Can't miss me :)", App.SelectedColor.Name)
                };
                task.Show();
            }
            catch { }
        }

        private void Mail_Click(object sender, EventArgs e)
        {
            try
            {
                var task = new EmailComposeTask
                {
                    Body = string.Format("I am blinking {0}! Can't miss me :)", App.SelectedColor.Name)
                };
                task.Show();
            }
            catch { }
        }

        private void ColoredRectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            else
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            _timer = new Timer(OnTimer, null, TimeSpan.FromMilliseconds(Interval), TimeSpan.FromMilliseconds(Interval));
        }
    }
}