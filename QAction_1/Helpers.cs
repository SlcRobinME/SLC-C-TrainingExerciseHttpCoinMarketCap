using Newtonsoft.Json;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QAction_1
{
    public static class LatestListingsHelper
    {
        public static LatestListingsResponse ParseResponse(string response)
        {
            return JsonConvert.DeserializeObject<LatestListingsResponse>(response);
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
                Latestlistingscirculatingsupply_1005 = item.CirculatingSupply,
                Latestlistingsmaxsupply_1006 = item.MaxSupply ?? 0.0,
                Latestlistingspriceusd_1007 = usd.Price,
                Latestlistingsmarketcap_1008 = usd.MarketCap,
                Latestlistingsvolume24h_1009 = usd.Volume24h,
                Latestlistingspercentchange1h_1010 = usd.PercentChange1h,
                Latestlistingspercentchange24h_1011 = usd.PercentChange24h,
                Latestlistingspercentchange7d_1012 = usd.PercentChange7d,
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

    public static class CategoriesHelper
    {
        public static CategoriesResponse ParseResponse(string response)
        {
            return JsonConvert.DeserializeObject<CategoriesResponse>(response);
        }

        public static string GetResponse(SLProtocolExt protocol)
        {
            return Convert.ToString(protocol.GetParameter(Parameter.categoriesresponse_201));
        }

        public static List<CategoriesQActionRow> BuildRows(CategoriesResponse data)
        {
            return data.Data.Select(item => new CategoriesQActionRow
            {
                Categoriesid_2001 = item.Id,
                Categoriesname_2002 = item.Name,
                Categoriesnumtokens_2003 = item.NumTokens,
                Categoriesavgpricechange_2004 = item.AvgPriceChange,
                Categoriesmarketcap_2005 = item.MarketCap,
                Categoriesmarketcapchange_2006 = item.MarketCapChange,
                Categoriesvolume_2007 = item.Volume,
                Categoriesvolumechange_2008 = item.VolumeChange,
                Categorieslastupdated_2009 = item.LastUpdated,
            }).ToList();
        }

        public static void FillTable(SLProtocolExt protocol, List<CategoriesQActionRow> rows)
        {
            protocol.FillArray(
                Parameter.Categories.tablePid,
                rows.Select(r => r.ToObjectArray()).ToList(),
                NotifyProtocol.SaveOption.Full);
        }
    }
    
    public static class CategoryDetailHelper
    {
        public static bool IsValidResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response) || response == "Bad Request")
                return false;

            return true;
        }

        public static CategoryDetailResponse ParseResponse(string response)
        {
            return JsonConvert.DeserializeObject<CategoryDetailResponse>(response);
        }

        public static void UpdateCategoryRow(SLProtocolExt protocol, CategoryData data)
        {
            var row = new CategoriesQActionRow
            {
                Categoriesid_2001 = data.Id,
                Categoriesnumtokens_2003 = data.NumTokens,
                Categoriesavgpricechange_2004 = data.AvgPriceChange,
                Categoriesmarketcap_2005 = data.MarketCap,
                Categoriesmarketcapchange_2006 = data.MarketCapChange,
                Categoriesvolume_2007 = data.Volume,
                Categoriesvolumechange_2008 = data.VolumeChange,
                Categorieslastupdated_2009 = data.LastUpdated,
            };

            protocol.SetRow(Parameter.Categories.tablePid, data.Id, row.ToObjectArray());
        }

    }

    public static class LatestQuotesHelper
    {
        public static LatestQuotesResponse ParseResponse(string response)
        {
            return JsonConvert.DeserializeObject<LatestQuotesResponse>(response);
        }
        public static string GetResponse(SLProtocol protocol)
        {
            return Convert.ToString(protocol.GetParameter(Parameter.latestquotesresponse_301));
        }

        public static void SetAllParameters(SLProtocol protocol, LatestQuotesData data)
        {
            var parameters = new Dictionary<int, object>
            {
                { Parameter.latestquotesactivecryptocurrencies_310, data.ActiveCryptocurrencies },
                { Parameter.latestquotesbtcdominance_311, data.BtcDominance },
                { Parameter.latestquotesethdominance_312, data.EthDominance },
                { Parameter.latestquotestotalmarketcap_313, data.Quote?.Usd?.TotalMarketCap ?? 0 },
                { Parameter.latestquotestotalvolume24h_314, data.Quote?.Usd?.TotalVolume24h ?? 0 },
                { Parameter.latestquotesdefimarketcap_315, data.Quote?.Usd?.DefiMarketCap ?? 0 },
                { Parameter.latestquotesstablecoinmarketcap_316, data.Quote?.Usd?.StablecoinMarketCap ?? 0 },
                { Parameter.latestquoteslastupdated_317, data.Quote?.Usd?.LastUpdated ?? string.Empty },
            };
            protocol.SetParameters(parameters.Keys.ToArray(),parameters.Values.ToArray());
        }
    }

}
