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

            MessagingCenter.Subscribe<object, string>(this, App.NotificationKey, (sender, deviceToken) =>
            {
                DeviceToken = $"Device Token: {deviceToken}";
            });

            MessagingCenter.Subscribe<object, string>(this, App.MessageReceived, (sender, msg) =>
            {
                NotificationMessage = msg;
            });
        }

        private string _deviceToken;
        public string DeviceToken
        {
            get => _deviceToken;
            set => SetProperty(ref _deviceToken, value);
        }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get => _notificationMessage;
            set => SetProperty(ref _notificationMessage, value);
        }
    }
}