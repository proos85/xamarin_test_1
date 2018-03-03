using XamarinForms1.Views;
using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using XamarinForms1.ViewModels;

namespace XamarinForms1
{
	public partial class App : Application
	{
	    public const string NotificationKey = "PNKey";
	    public const string MessageReceived = "MessageReceived";
	    public const string MobileServiceUrl = "https://xamarinforms1backend.azurewebsites.net";

		public App ()
		{
			InitializeComponent();

            DependencyService.Register<MainViewModel>();

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
