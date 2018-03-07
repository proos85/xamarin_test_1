using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinForms1.ViewModels;

namespace XamarinForms1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage
	{
		public MainPage ()
		{
			InitializeComponent ();

		    BindingContext = DependencyService.Get<MainViewModel>();
		}

	    private async void Button_OnClicked(object sender, EventArgs e)
	    {
	        DependencyService.Get<MainViewModel>().NotificationMessage = $"Notification message: '{newNotificationMessageEntry.Text}'";
	        await DisplayAlert("Notification message sent", "Notification message was sent", "OK");
	    }
	}
}