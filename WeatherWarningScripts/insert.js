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