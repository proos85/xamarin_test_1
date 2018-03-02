using Plugin.Settings;
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

		    var viewModel = DependencyService.Get<MainViewModel>();
		    viewModel.DeviceToken = CrossSettings.Current.GetValueOrDefault("DeviceToken", string.Empty);

		    BindingContext = viewModel;
		}
	}
}