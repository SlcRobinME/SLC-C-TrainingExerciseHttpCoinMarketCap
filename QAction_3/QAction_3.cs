using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: getLatestListings.
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
            string json = Convert.ToString(protocol.GetParameter(Parameter.getlatestlistingsresponse_15));

            if (String.IsNullOrEmpty(json))
                return;

            JObject response = JObject.Parse(json);

            JArray coins = (JArray)response["data"];

            List<string> options = new List<string>();

            foreach (var coin in coins)
            {
                var name = coin["name"].ToString();// + " (" + coin["symbol"].ToString() + ") ";

                options.Add(name);
            }

            options.Sort();
            var values = String.Join(";", options);
            protocol.SetParameter(Parameter.cryptocurrencievalues_13, values);
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
