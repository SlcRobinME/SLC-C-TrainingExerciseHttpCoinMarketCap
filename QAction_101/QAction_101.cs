using System;
using System.Collections.Generic;
using QuickType;
using Skyline.DataMiner.Scripting;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.latestlistingsstatuscode_100));
            string json = Convert.ToString(protocol.GetParameter(Parameter.latestlistingsresponse_101));
            if (!statusCode.Contains("200"))
            {
                protocol.Log($"QA{protocol.QActionID}|Unexpected status: {statusCode}, Response: {json}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("{"))
            {
                protocol.Log($"QA{protocol.QActionID}|Response is not valid JSON: {json?.Substring(0, Math.Min(50, json?.Length ?? 0))}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            Welcome welcome = Welcome.FromJson(json);
            if (welcome?.Data == null)
            {
                protocol.Log($"QA{protocol.QActionID}|'data' array not found or deserialization failed.", LogType.Error, LogLevel.NoLogging);
                return;
            }

            new CoinMarketCapService(protocol).FillLatestListings(welcome);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|Exception: {ex.Message}", LogType.Error, LogLevel.NoLogging);
        }
    }
}