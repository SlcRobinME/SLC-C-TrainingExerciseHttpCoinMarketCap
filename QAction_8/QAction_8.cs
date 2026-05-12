using System;
using QAction_1;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocol protocol)
	{
		try
		{
            if (!CommunicationHelper.ValidateStatusCode(protocol, Parameter.latestquotesstatuscode_300, Parameter.latestquotescommunicationstatus_302, out string statusCode))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Unexpected status code: {statusCode}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            string response = LatestQuotesHelper.GetResponse(protocol);

            if (string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            var root = LatestQuotesHelper.ParseResponse(response);

            if (root?.Data == null)
            {
                return;
            }

            LatestQuotesHelper.SetAllParameters(protocol, root.Data);
        }
        catch (Exception ex)
        {
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
