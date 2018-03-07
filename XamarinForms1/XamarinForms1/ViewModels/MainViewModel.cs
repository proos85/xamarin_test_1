using System.Threading.Tasks;
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
            DeviceToken = $"Device token: '{CrossSettings.Current.GetValueOrDefault("DeviceToken", string.Empty)}'";
            NotificationMessage = "NotificationMessage: '-'";

            MessagingCenter.Subscribe<object, string>(this, App.NotificationKey, (sender, deviceToken) =>
            {
                DeviceToken = $"Device Token: {deviceToken}";
            });

            MessagingCenter.Subscribe<object, string>(this, App.MessageReceived, (sender, msg) =>
            {
                NotificationMessage = msg;
            });

            SentMessageCommand = new Command(SentMessageCommandAction);
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

        private string _sentMessage;
        public string SentMessage
        {
            get => _sentMessage;
            set => SetProperty(ref _sentMessage, value);
        }

        public ICommand SentMessageCommand { get; set; }

        public void SentMessageCommandAction()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                SentMessage = string.Empty;
            });
        }
    }
}