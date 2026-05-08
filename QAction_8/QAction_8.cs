using Skyline.DataMiner.Scripting;
using System;
using QAction_1;

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
