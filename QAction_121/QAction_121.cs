using QuickType;
using Skyline.DataMiner.Net.Apps.UserDefinableApis;
using Skyline.DataMiner.Scripting;
using System;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.categorydetailstatuscode_120));
            string json = Convert.ToString(protocol.GetParameter(Parameter.categorydetailresponse_121));
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

            CategoryDetailResponse response = CategoryDetailResponse.FromJson(json);
            CategoryItem data = response?.Data;
            if (data == null)
            {
                protocol.Log($"QA121|'data' not found or deserialization failed.", LogType.Error, LogLevel.NoLogging);
                return;
            }

            new CoinMarketCapService(protocol).UpdateCategoryRow(response.Data);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|Exception: {ex.Message}", LogType.Error, LogLevel.NoLogging);
        }
    }
}