using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.Storage;

namespace WeatherWarningW8
{
    public class Notifications
    {
        private NotificationHub hub;

        public Notifications()
        {
            hub = new NotificationHub("weatherwarning", "Endpoint=sb://weatherwarning.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=XjbdiSHFLIU2tHXiFQVMrLNlosqvMxOlHs0EpWZVEx4=");
        }

        public async Task StoreCategoriesAndSubscribe(IEnumerable<string> regions, IEnumerable<string> categories, string language)
        {
            ApplicationData.Current.LocalSettings.Values["categories"] = string.Join(",", categories);
            ApplicationData.Current.LocalSettings.Values["regions"] = string.Join(",", regions);
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            await hub.UnregisterAllAsync(channel.Uri);

            var tags= new HashSet<string>(categories);
            tags.UnionWith(regions);

            var template = String.Format(@"<toast><visual><binding template=""ToastText02""><text id=""1"">$(Region_{0})</text><text id=""2"">$(Warning_{0})</text></binding></visual></toast>", language);
            await hub.RegisterTemplateAsync(channel.Uri, template, "WarningTemplate", tags);
        }
    }
}
