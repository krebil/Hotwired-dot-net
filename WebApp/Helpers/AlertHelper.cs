using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApp.Components;

namespace WebApp.Helpers;

public static class AlertHelper
{
    private const string AlertKey = "alert";
    private const string AlertTypeKey = "alertType";
    public static void SetAlert(this ITempDataDictionary tempData, string message)
    {
        tempData[AlertKey] = message;
    }
    
    public static string? GetAlert(this ITempDataDictionary tempData)
    {
        return tempData[AlertKey] as string;
    }
    
    public static AlertType? GetAlertType(this ITempDataDictionary tempData)
    {
        return tempData[AlertTypeKey] as AlertType?;
    }
    
    public static void SetAlertType(this ITempDataDictionary tempData, AlertType? alertType)
    {
        tempData[AlertTypeKey] = alertType;
    }
}