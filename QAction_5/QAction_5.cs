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
	public static void Run(SLProtocolExt protocol)
	{
		try
		{
            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.categoriesstatuscode_200));

            if (!statusCode.Contains("HTTP/1.1 200"))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Unexpected status code: {statusCode}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            string response = CategoriesHelper.GetResponse(protocol);
            if (string.IsNullOrEmpty(response))
                return;

            var data = CategoriesHelper.ParseResponse(response);
            if (data?.Data == null)
                return;

            var rows = CategoriesHelper.BuildRows(data);
            CategoriesHelper.FillTable(protocol, rows);
        }
        catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
