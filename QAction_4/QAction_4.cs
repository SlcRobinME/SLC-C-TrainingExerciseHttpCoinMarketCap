using System;
using QAction_1;
using Skyline.DataMiner.Scripting;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            if (!CommunicationHelper.ValidateStatusCode(protocol, Parameter.latestlistingsstatuscode_100, Parameter.latestlistingscommunicationstatus_102, out string statusCode))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Unexpected status code: {statusCode}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            string response = LatestListingsHelper.GetResponse(protocol);

            if (string.IsNullOrEmpty(response))
                return;

            var data = LatestListingsHelper.ParseResponse(response);
            protocol.Log($"QA4|Debug|First item name: '{data?.Data?[0]?.Name}', Price: '{data?.Data?[0]?.Quote?["USD"]?.Price}'", LogType.DebugInfo, LogLevel.NoLogging);

            if (data?.Data == null)
                return;

            var rows = LatestListingsHelper.BuildRows(data);
            LatestListingsHelper.FillTable(protocol, rows);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}