using System;
using QuickType;
using Skyline.DataMiner.Scripting;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.categoriesstatuscode_112));
            string json = Convert.ToString(protocol.GetParameter(Parameter.categoriesresponse_111));

            protocol.SetParameter(Parameter.categoriescommunicationstatus_110, CoinMarketCapService.GetCommunicationStatus(statusCode));
            if (CoinMarketCapService.GetCommunicationStatus(statusCode) != 1)
            {
                protocol.Log($"QA{protocol.QActionID}|Unexpected status: {statusCode}, Response: {json}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("{"))
            {
                protocol.Log($"QA{protocol.QActionID}|Response is not valid JSON: {json?.Substring(0, Math.Min(50, json?.Length ?? 0))}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            CategoriesResponse response = CategoriesResponse.FromJson(json);
            if (response?.Data == null)
            {
                protocol.Log($"QA{protocol.QActionID}|'data' array not found or deserialization failed.", LogType.Error, LogLevel.NoLogging);
                return;
            }

            new CoinMarketCapService(protocol).FillCategories(response);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|Exception: {ex.Message}", LogType.Error, LogLevel.NoLogging);
        }
    }
}