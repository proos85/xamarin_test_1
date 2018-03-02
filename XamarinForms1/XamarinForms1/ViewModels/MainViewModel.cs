using System.Windows.Input;
using Plugin.Settings;
using Xamarin.Forms;

namespace XamarinForms1.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainViewModel: BaseViewModel
    {
        public MainViewModel()
        {
            DeviceToken = CrossSettings.Current.GetValueOrDefault("DeviceToken", string.Empty);

            UpdateDeviceTokenCommand = new Command<string>(UpdateDeviceTokenCommandAction);
            MessagingCenter.Subscribe<object, string>(this, App.NotificationKey, (sender, msg) =>
            {
                UpdateDeviceTokenCommand.Execute(msg);
            });
        }

        private string _deviceToken;
        public string DeviceToken
        {
            get => _deviceToken;
            set => SetProperty(ref _deviceToken, value);
        }

        public ICommand UpdateDeviceTokenCommand { get; set; }

        private void UpdateDeviceTokenCommandAction(string deviceToken)
        {
            DeviceToken = $"Device Token: {deviceToken}";
        }
    }
}