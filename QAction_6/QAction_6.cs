using Skyline.DataMiner.Scripting;
using System;

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
            string categoryId = GetRowKey(protocol);

            if (string.IsNullOrWhiteSpace(categoryId))
                return;

            SetCategoryDetailRequestId(protocol, categoryId);
        }
        catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
    private static string GetRowKey(SLProtocolExt protocol)
    {
        return Convert.ToString(protocol.RowKey());
    }
    private static void SetCategoryDetailRequestId(SLProtocolExt protocol, string categoryId)
    {
        protocol.SetParameter(Parameter.categorydetailrequestid_210, categoryId);
    }
}
