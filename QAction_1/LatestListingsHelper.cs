namespace QAction_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

    public class LatestListingsHelper
    {
        public static LatestListingsResponse ParseResponse(string response)
        {
            return SecureNewtonsoftDeserialization.DeserializeObject<LatestListingsResponse>(response);
        }

        public static string GetResponse(SLProtocolExt protocol)
        {
            return Convert.ToString(protocol.GetParameter(Parameter.latestlistingsresponse_101));
        }

        public static List<LatestlistingsQActionRow> BuildRows(LatestListingsResponse data)
        {
            var rows = new List<LatestlistingsQActionRow>();

            foreach (var item in data.Data)
            {
                if (!item.Quote.TryGetValue("USD", out var usd))
                    continue;

                rows.Add(BuildRow(item, usd));
            }

            return rows;
        }

        public static LatestlistingsQActionRow BuildRow(Listing item, Quote usd)
        {
            return new LatestlistingsQActionRow
            {
                Latestlistingsid_1001 = Convert.ToString(item.Id),
                Latestlistingsname_1002 = item.Name,
                Latestlistingssymbol_1003 = item.Symbol,
                Latestlistingsrank_1004 = (double)item.CmcRank,
                Latestlistingscirculatingsupply_1005 = item.CirculatingSupply ?? -1,
                Latestlistingsmaxsupply_1006 = item.MaxSupply ?? -1,
                Latestlistingspriceusd_1007 = usd.Price ?? -1,
                Latestlistingsmarketcap_1008 = usd.MarketCap ?? -1,
                Latestlistingsvolume24h_1009 = usd.Volume24h ?? -1,
                Latestlistingspercentchange1h_1010 = usd.PercentChange1h ?? -1,
                Latestlistingspercentchange24h_1011 = usd.PercentChange24h ?? -1,
                Latestlistingspercentchange7d_1012 = usd.PercentChange7d ?? -1,
            };
        }

        public static void FillTable(SLProtocolExt protocol, List<LatestlistingsQActionRow> rows)
        {
            protocol.FillArray(
                Parameter.Latestlistings.tablePid,
                rows.Select(r => r.ToObjectArray()).ToList(),
                NotifyProtocol.SaveOption.Full);
        }
    }
}
