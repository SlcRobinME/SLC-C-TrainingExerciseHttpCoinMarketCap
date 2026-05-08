using System;
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
            string token = GetWrittenToken(protocol);
            SetBearerToken(protocol, token);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|Run|Exception ", LogType.Error, LogLevel.NoLogging);
        }
    }
    private static string GetWrittenToken(SLProtocol protocol)
    {
        return Convert.ToString(protocol.GetParameter(Parameter.Write.bearertoken_11));
    }
    private static void SetBearerToken(SLProtocol protocol, string token)
    {
        protocol.SetParameter(Parameter.bearertoken_10, "Bearer " + token.Trim());
    }
}
