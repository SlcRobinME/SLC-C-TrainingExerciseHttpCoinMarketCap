using Newtonsoft.Json.Linq;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

/// <summary>
/// DataMiner QAction Class: parseCategory.
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
            string code = Convert.ToString(protocol.GetParameter(Parameter.getcategoryresponsecode_39));
            if (code.Contains("200"))
            {
                protocol.Log($"QA{protocol.QActionID}|Category request failed, status: {code}", LogType.Error, LogLevel.NoLogging);
                return;
            }

            protocol.Log(
                "Parsing row",
                LogType.Information,
                LogLevel.NoLogging
            );

            string json = Convert.ToString(protocol.GetParameter(Parameter.getcategoryresponse_40));
            JToken data = JObject.Parse(json)["data"];

            string id = (string)data["id"];

            var row = new CategoriesQActionRow
            {
                Categoryid = id,
                Categoryname = (string)data["name"],
                Categorytitle = (string)data["title"],
                Categorydesc = (string)data["description"],
                Categorynumtokens = (double?)data["num_tokens"] ?? 0d,
                Categoryavgpricechange = (double?)data["avg_price_change"] ?? 0d,
                Categorymarketcap = (double?)data["market_cap"] ?? 0d,
                Categorymarketcapchange = (double?)data["market_cap_change"] ?? 0d,
                Categoryvolume = (double?)data["volume"] ?? 0d,
                Categoryvolumechange = (double?)data["volume_change"] ?? 0d,
                Categorylastupdated = ToOADate((string)data["last_updated"]),
            };

            protocol.categories.SetRow(row);
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
    private static double ToOADate(string iso)
    {
        return DateTime.TryParse(iso, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime dt)
            ? dt.ToUniversalTime().ToOADate()
            : 0d;
    }
}