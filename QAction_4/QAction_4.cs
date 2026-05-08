using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QAction_1;
using Skyline.DataMiner.Scripting;
using System.Linq;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {

            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.latestlistingsstatuscode_100));

            if (!statusCode.Contains("HTTP/1.1 200"))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Unexpected status code: {statusCode}", LogType.Error, LogLevel.NoLogging);
                return;
            }
            string response = LatestListingsHelper.GetResponse(protocol);

            if (string.IsNullOrEmpty(response))
                return;

            var data = LatestListingsHelper.ParseResponse(response);

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