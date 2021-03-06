﻿using XamarinForms1.Views;
using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace XamarinForms1
{
	public partial class App : Application
	{

		public App ()
		{
			InitializeComponent();
            MainPage = new MainPage();
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
		    AppCenter.Start("ios=86f978f4-e62a-4445-9571-2f2b7617975d;", typeof(Analytics), typeof(Crashes));
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
