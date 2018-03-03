using System;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Plugin.Settings;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace XamarinForms1.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once UnusedMember.Global
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private NSDictionary _launchOptions;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());

            Task.Run(async () =>
            {
                await RequestPushNotificationAuthorizationFromUser();
            });

            _launchOptions = options;

            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            if (_launchOptions != null && _launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
            {
                if (_launchOptions[UIApplication.LaunchOptionsRemoteNotificationKey] is NSDictionary notification)
                {
                    PresentNotification(notification);
                }

                _launchOptions = null;
            }
        }

        private static async Task RequestPushNotificationAuthorizationFromUser()
        {
            var requestResult = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound)
                .ConfigureAwait(false);

            bool approved = requestResult.Item1;
            NSError error = requestResult.Item2;

            if (error == null)
            {
                if (!approved)
                {
                    Console.WriteLine("The user did not approved");
                    return;
                }

                var currentSettings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync()
                    .ConfigureAwait(false);

                if (currentSettings.AuthorizationStatus != UNAuthorizationStatus.Authorized)
                {
                    Console.WriteLine("Permissions has been requested in the past, but have been revoked by the user");
                    return;
                }

                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                Console.WriteLine($"Something went wrong on the device: {error}");
            }
        }

        public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            if (deviceToken == null)
            {
                return;
            }
            
            Console.WriteLine($"Device token received: {deviceToken}");
            CrossSettings.Current.AddOrUpdateValue("DeviceToken", deviceToken.ToString());
            MessagingCenter.Send<object, string>(this, App.NotificationKey, deviceToken.ToString());

            await SendRegistrationTokenToServer(deviceToken).ConfigureAwait(false);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Console.WriteLine($"Failed to register for push notifications: {error}");
        }

        public override void DidReceiveRemoteNotification(
            UIApplication application, 
            NSDictionary userInfo, 
            Action<UIBackgroundFetchResult> completionHandler)
        {
            var currentBadgeNumber = UIApplication.SharedApplication.ApplicationIconBadgeNumber;
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = currentBadgeNumber + 1;

            PresentNotification(userInfo);

            completionHandler(UIBackgroundFetchResult.NoData);
        }

        private void PresentNotification(NSDictionary userInfo)
        {
            if (userInfo.ObjectForKey(new NSString("aps")) is NSDictionary aps)
            {
                var msg = string.Empty;
                if (aps.ContainsKey(new NSString("alert")))
                {
                    msg = aps[new NSString("alert")].ToString();
                }

                if (string.IsNullOrEmpty(msg))
                {
                    msg = "Unable to parse";
                }

                MessagingCenter.Send<object, string>(this, App.MessageReceived, msg);
            }
        }

        private async Task SendRegistrationTokenToServer(NSData deviceToken)
        {
            const string templateBodyApns = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

            var templates = new JObject
            {
                ["genericMessage"] = new JObject
                {
                    {"body", templateBodyApns}
                }
            };

            var client = new MobileServiceClient(App.MobileServiceUrl);
            var push = client.GetPush();
            
            await push.RegisterAsync(deviceToken, templates).ConfigureAwait(false);
            CrossSettings.Current.AddOrUpdateValue("InstallationId", push.InstallationId);
        }
    }
}
