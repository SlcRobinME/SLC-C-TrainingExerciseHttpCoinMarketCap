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
            string statusCode = Convert.ToString(protocol.GetParameter(Parameter.latestquotesstatuscode_300));

            if (!statusCode.Contains("HTTP/1.1 200"))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Unexpected status code: {statusCode}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            string response = GetResponse(protocol);

            if (!CategoryDetailHelper.IsValidResponse(response))
                return;

            var categoryDetail = CategoryDetailHelper.ParseResponse(response);

            if (categoryDetail?.Data == null)
            {
                return;
            }
            protocol.Log($"QA{protocol.QActionID}|UpdateCategoryRow|Category ID: '{categoryDetail.Data.Id}'", LogType.Error, LogLevel.NoLogging);

            CategoryDetailHelper.UpdateCategoryRow(protocol, categoryDetail.Data);
        }
        catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

    public static string GetResponse(SLProtocolExt protocol)
    {
        return Convert.ToString(protocol.GetParameter(Parameter.categorydetailresponse_203));
    }
}
