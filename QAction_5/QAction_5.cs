using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;

/// <summary>
/// DataMiner QAction Class: getCategories.
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
            string json = Convert.ToString(protocol.GetParameter(Parameter.getcategoriesresponse_38));

            if (String.IsNullOrEmpty(json))
                return;

            JObject response = JObject.Parse(json);
            JArray categories = (JArray)response["data"];

            List<QActionTableRow> rows = new List<QActionTableRow>();

            foreach (var category in categories)
            {
                CategoriesQActionRow row = new CategoriesQActionRow
                {
                    Categoryid_1001 = (string)category["id"],
                    Categoryname_1002 = (string)category["name"],
                    Categorytitle_1003 = (string)category["title"],
                    Categorydesc_1004 = (string)category["description"],

                    Categorynumtokens_1005 = GetDouble(category["num_tokens"]),
                    Categoryavgpricechange_1006 = GetDouble(category["avg_price_change"]),
                    Categorymarketcap_1007 = GetDouble(category["market_cap"]),
                    Categorymarketcapchange_1008 = GetDouble(category["market_cap_change"]),
                    Categoryvolume_1009 = GetDouble(category["volume"]),
                    Categoryvolumechange_1010 = GetDouble(category["volume_change"]),

                    Categorylastupdated_1011 = GetDateTime(category["last_updated"]),
                };

                rows.Add(row);
            }

            protocol.categories.FillArray(rows);
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

    private static double GetDateTime(JToken value)
    {
        if (value == null || value.Type == JTokenType.Null)
            return 0;

        DateTime dateTime = DateTime.Parse(
            value.ToString(),
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind
        );

        return dateTime.ToOADate();
    }
}