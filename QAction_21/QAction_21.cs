using System;
using Skyline.DataMiner.Scripting;

public static class QAction
{
	public static void Run(SLProtocol protocol)
	{
		try
		{
			string newToken = Convert.ToString(protocol.GetParameter(Parameter.Write.bearertoken_21));
			if (!string.IsNullOrWhiteSpace(newToken))
			{
				protocol.SetParameter(Parameter.bearertoken_20, "Bearer " + newToken.Trim());
			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|MethodName|Exception setting Bearer token", LogType.Error, LogLevel.NoLogging);
		}
	}
}
