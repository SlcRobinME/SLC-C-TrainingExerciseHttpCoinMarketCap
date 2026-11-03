using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: setToken.
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
			string input = Convert.ToString(protocol.GetParameter(Parameter.token_14));

            if (string.IsNullOrEmpty(input))
            {
                protocol.Log($"QA{protocol.QActionID}|Token is empty, keeping the previous value.", LogType.Error, LogLevel.NoLogging);
                return;
            }

            const string prefix = "Bearer ";
            if (!input.Contains(prefix))
            {
                input = prefix + input;
            }

			protocol.SetParameter(Parameter.token_14, input);
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}