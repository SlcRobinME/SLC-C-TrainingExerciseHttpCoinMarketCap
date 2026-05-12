namespace QAction_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

    public static class LatestQuotesHelper
    {
        public static LatestQuotesResponse ParseResponse(string response)
        {
            return SecureNewtonsoftDeserialization.DeserializeObject<LatestQuotesResponse>(response);
        }

        public static string GetResponse(SLProtocol protocol)
        {
            return Convert.ToString(protocol.GetParameter(Parameter.latestquotesresponse_301));
        }

        public static void SetAllParameters(SLProtocol protocol, LatestQuotesData data)
        {
            var parameters = new Dictionary<int, object>
            {
                { Parameter.latestquotesactivecryptocurrencies_310, data.ActiveCryptocurrencies ?? -1 },
                { Parameter.latestquotesbtcdominance_311, data.BtcDominance ?? -1},
                { Parameter.latestquotesethdominance_312, data.EthDominance ?? -1 },
                { Parameter.latestquotestotalmarketcap_313, data.Quote?.Usd?.TotalMarketCap ?? -1 },
                { Parameter.latestquotestotalvolume24h_314, data.Quote?.Usd?.TotalVolume24h ?? -1 },
                { Parameter.latestquotesdefimarketcap_315, data.Quote?.Usd?.DefiMarketCap ?? -1 },
                { Parameter.latestquotesstablecoinmarketcap_316, data.Quote?.Usd?.StablecoinMarketCap ?? -1 },
                { Parameter.latestquoteslastupdated_317, data.Quote?.Usd?.LastUpdated ?? string.Empty },
            };
            protocol.SetParameters(parameters.Keys.ToArray(), parameters.Values.ToArray());
        }
    }
}
