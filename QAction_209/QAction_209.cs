using System;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using SLParameter = Skyline.DataMiner.Scripting.Parameter;

public static class QAction
{
	public static void Run(SLProtocol protocol)
	{
		try
		{
			// The row key (Category ID) is available via the trigger row index
			string categoryId = Convert.ToString(protocol.RowKey());
			if (string.IsNullOrWhiteSpace(categoryId))
			{
				protocol.Log($"QA{protocol.QActionID}|Run|Row-level refresh: category ID is empty.", LogType.Error, LogLevel.NoLogging);
				return;
			}

			// Store the category ID so the HTTP session can use it
			protocol.SetParameter(SLParameter.categoryrefreshid_140, categoryId);

			protocol.CheckTrigger(2010);
        }
        catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|Exception in Category Row Refresh: {ex.Message}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
