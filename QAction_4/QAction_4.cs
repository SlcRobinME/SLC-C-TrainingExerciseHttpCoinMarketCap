using Newtonsoft.Json.Linq;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// DataMiner QAction Class: coinInfo.
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
			string selectedCoin = Convert.ToString(protocol.GetParameter(Parameter.criptocurrencies_11));

            if (String.IsNullOrEmpty(selectedCoin))
                return;

            if (String.IsNullOrEmpty(json))
                return;

            JObject response = JObject.Parse(json);
            JArray coins = (JArray)response["data"];

            JObject coin = coins
                .OfType<JObject>()
                .FirstOrDefault(c =>
                    String.Equals(
                        (string)c["name"],
                        selectedCoin,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (coin == null)
            {
                protocol.Log(
                    "Selected cryptocurrency not found: " + selectedCoin,
                    LogType.Error,
                    LogLevel.NoLogging
                );

                return;
            }

            JObject quote = (JObject)coin["quote"]["USD"];

            Dictionary<int, object> parameterValues =
                new Dictionary<int, object>
                {
                    { Parameter.name_16, Convert.ToString(coin["name"]) },
                    { Parameter.symbol_17, Convert.ToString(coin["symbol"]) },
                    { Parameter.max_supply_18, GetDouble(coin["max_supply"]) },
                    { Parameter.circulating_supply_19, GetDouble(coin["circulating_supply"]) },
                    { Parameter.price_20, GetDouble(quote["price"]) },
                    { Parameter.marketcap_21, GetDouble(quote["market_cap"]) },
                    { Parameter.marketcapdominance_22, GetDouble(quote["market_cap_dominance"]) },
                    { Parameter.volume24h_23, GetDouble(quote["volume_24h"]) },
                    { Parameter.percentchange24h_24, GetDouble(quote["percent_change_24h"]) },
                };

            protocol.SetParameters(parameterValues);
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static double GetDouble(JToken value)
    {
        if (value == null || value.Type == JTokenType.Null)
            return 0;

        return value.Value<double>();
    }
}