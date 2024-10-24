using System;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager
{
    private bool showDebugs = false;

    public void LogAds(string eventString, AdAnalyticData adData) => LogAds(eventString, adData.ad_type, adData.placement, adData.result, adData.connection);
    public void LogAds(string eventString, string ad_type, string placement, string result, bool connection)
    {
        var sectionString = eventString;
        var data = new Dictionary<string, object>();
        data["ad_type"] = ad_type;
        data["placement"] = placement;
        data["result"] = result;
        data["connection"] = connection;

        ReportEvent(sectionString, data);
        OnAnalyticSent($"{sectionString} | {data}");
    }

    public void Log(AnalyticSectionType section, string eventText)
    {
        AppMetricLog(section.ToString(), eventText);
    }

    private void AppMetricLog(string sectionString, string eventString)
    {
        var data = new Dictionary<string, object>();
        data[eventString] = null;
        ReportEvent(sectionString, data);

        OnAnalyticSent($"{sectionString} | {eventString}");
    }

    private void ReportEvent(string sectionString, Dictionary<string, object> data)
    {
        AppMetrica.Instance.ReportEvent(sectionString, data);
    }

    private void OnAnalyticSent(string debugMessage)
    {
        if (showDebugs) Debug.Log("[analytic] " + debugMessage);
    }
}

[Serializable]
public struct AdAnalyticData
{
    public string ad_type;
    public string placement;
    public string result;
    public bool connection;

    public AdAnalyticData(string ad_type, string placement, string result, bool connection)
    {
        this.ad_type = ad_type;
        this.placement = placement;
        this.result = result;
        this.connection = connection;
    }
}

public enum AnalyticSectionType
{
    // добавить аналитические разделы
}