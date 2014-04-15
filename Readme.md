This demo implements a simple Weather Warning service using Azure Notification hubs. 
It consists of a Windows 8 app which allows to register for specific weather warning events within a geographical region. Such events can be logged using the html app that simply just inserts a new weather warning to a table hosted in Azure Mobile Services. This insert operation triggers a script that posts the notification to the notofcation hub. It is the hub that notifies the Windows 8 clients about the new weather warning.

To get this demo working you have to configure the following steps:


1.) associate the Windows 8 app with the store

2.) create a new service bus namespace and create a new notificaton hub

3.) configure the Windows push notification service within the notification hub portal

4.) wire up the Windows 8 app with the notification hub: In Notifications.cs replace the notification HUB_NAME and the Default**Listen**SharedAccessSignature:
 
        public Notifications()
        {
            hub = new NotificationHub("[[HUB_NAME]]", "[[DefaultListenSharedAccessSignature]]");
        }
5.) create a new mobile service
6.) add a table called **warning** to the mobile service 
7.) add the following script to the insert operation of the warning table and replace the notification HUB_NAME and the Default**Full**SharedAccessSignature:

    function insert(item, user, request) {
           sendNotification(item.afrikaansWarningText, item.englishWarningText, item.regions, item.warnings);
       request.execute();
    }

    function sendNotification(AF_text, EN_text, regions, warnings) {
       var regionsArray = regions.split(",");
       var warningsArray = warnings.split(",");
        
       var azure = require('azure');
       var notificationHubService = azure.createNotificationHubService('[[HUB_NAME]]', '[[FullSharedAccessSignature]]');
   
       regionsArray.forEach(function (region) {
        var EN_topMsg = "Weather warning for " + region;
        var AF_topMsg = "Weer waarskuwing vir " + region;
        var notification = {
            "Region_EN" : EN_topMsg,
            "Warning_EN" : EN_text,
            "Region_AF" : AF_topMsg ,
            "Warning_AF" : AF_text,                
        }
        var tagExpression = "((" + warningsArray.join("||") + ") && " + region + ")";
        console.info("Send notification with tag expression: " + tagExpression);
        notificationHubService.send(tagExpression, notification, function(error) {
            if (!error) {
                console.info("Notification successful");
            } 
            else {
                console.error(error);
            }         
        });
      }); 
    }
8.) Wire up the html app with the mobile services. In page.js replace the MOBILE_SERVICES_URL and the SHARED_SECRET of your mobile service

    var client = new WindowsAzure.MobileServiceClient('[[MOBILE_SERVICES_URL]]', '[[SHARED_SECRET]]'),

Finished - now you're good to go: just subscribe to the weather warning using the Windows 8 App and fire weather events using the webpage.


