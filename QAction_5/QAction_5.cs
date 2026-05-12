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
	public static void Run(SLProtocolExt protocol)
	{
		try
		{
            if (!CommunicationHelper.ValidateStatusCode(protocol, Parameter.categoriesstatuscode_200, Parameter.categoriescommunicationstatus_204, out string statusCode))
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
