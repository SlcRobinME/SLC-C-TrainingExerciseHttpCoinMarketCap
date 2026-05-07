using QuickType;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;

public class CoinMarketCapService
{
    private readonly SLProtocolExt _protocol;

    public CoinMarketCapService(SLProtocolExt protocol)
    {
        _protocol = protocol;
    }

    public void FillLatestListings(Welcome welcome)
    {
        var tableRows = new List<LatestlistingstableQActionRow>();

        foreach (Datum item in welcome.Data)
        {
            Usd quoteUsd = item.Quote?.Usd;

            tableRows.Add(new LatestlistingstableQActionRow
            {
                Latestlistingstableid_1001 = Convert.ToString(item.Id),
                Latestlistingstablename_1002 = item.Name,
                Latestlistingstablesymbol_1003 = item.Symbol,
                Latestlistingstableslug_1004 = item.Slug,
                Latestlistingstablecmcrank_1005 = item.CmcRank,
                Latestlistingstablepriceusd_1006 = quoteUsd?.Price ?? 0,
                Latestlistingstablepercentchange24h_1007 = quoteUsd?.PercentChange24H ?? 0,
                Latestlistingstablemarketcapusd_1008 = quoteUsd?.MarketCap ?? 0,
                Latestlistingstablevolume24husd_1009 = quoteUsd?.Volume24H ?? 0,
                Latestlistingstablecirculatingsupply_1010 = item.CirculatingSupply,
                Latestlistingstablemaxsupply_1011 = item.MaxSupply ?? 0,
                Latestlistingstablelastupdated_1012 = quoteUsd?.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                Latestlistingstablepercentchange1h_1013 = quoteUsd?.PercentChange1H ?? 0,
                Latestlistingstablepercentchange7d_1014 = quoteUsd?.PercentChange7D ?? 0,
                Latestlistingstablepercentchange30d_1015 = quoteUsd?.PercentChange30D ?? 0,
                Latestlistingstablevolumechange24h_1016 = quoteUsd?.VolumeChange24H ?? 0,
                Latestlistingstablefullydilutedmarketcap_1017 = quoteUsd?.FullyDilutedMarketCap ?? 0,
                Latestlistingstabletotalsupply_1018 = item.TotalSupply,
                Latestlistingstableinfinitesupply_1019 = item.InfiniteSupply ? 1 : 0,
            });
        }

        _protocol.FillArray(
            Parameter.Latestlistingstable.tablePid,
            tableRows.ConvertAll(r => r.ToObjectArray()),
            NotifyProtocol.SaveOption.Full);
    }

    public void FillCategories(CategoriesResponse response)
    {
        var tableRows = new List<CategoriestableQActionRow>();

        foreach (CategoryItem item in response.Data)
        {
            tableRows.Add(BuildCategoryRow(item));
        }

        _protocol.FillArray(
            Parameter.Categoriestable.tablePid,
            tableRows.ConvertAll(r => r.ToObjectArray()),
            NotifyProtocol.SaveOption.Full);
    }

    public void UpdateCategoryRow(CategoryItem item)
    {
        var row = BuildCategoryRow(item);
        _protocol.SetRow(Parameter.Categoriestable.tablePid, item.Id, row.ToObjectArray());
    }

    public void FillLatestQuotes(GlobalQuotesData data)
    {
        GlobalUsd usd = data.Quote?.Usd;

        var parameters = new Dictionary<int, object>
        {
            { Parameter.latestquotestotalmarketcap_300,                          usd?.TotalMarketCap ?? 0 },
            { Parameter.latestquotestotalvolume24h_301,                          usd?.TotalVolume24H ?? 0 },
            { Parameter.latestquotesbtcdominance_302,                            data.BtcDominance },
            { Parameter.latestquotesethdominance_303,                            data.EthDominance },
            { Parameter.latestquotesactivecryptocurrencies_304,                  data.ActiveCryptocurrencies },
            { Parameter.latestquoteslastupdated_305,                             data.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") },
            { Parameter.latestquotesdefi24hpercentagechange_306,                 data.Defi24hPercentageChange },
            { Parameter.activeexchanges_307,                                     data.ActiveExchanges },
            { Parameter.latestquotestotalmarketcapyesterday_308,                 usd?.TotalMarketCapYesterday ?? 0 },
            { Parameter.latestquotestotalmarketcapyesterdaypercentagechange_309, usd?.TotalMarketCapYesterdayPercentageChange ?? 0 },
            { Parameter.latestquotestotalvolume24hyesterday_310,                 usd?.TotalVolume24hYesterday ?? 0 },
            { Parameter.latestquotestotalvolume24hyesterdaypercentagechange_311, usd?.TotalVolume24hYesterdayPercentageChange ?? 0 },
            { Parameter.latestquotesaltcoinmarketcap_312,                        usd?.AltcoinMarketCap ?? 0 },
            { Parameter.latestquotesaltcoinvolume24h_313,                        usd?.AltcoinVolume24h ?? 0 },
            { Parameter.latestquotesdefimarketcap_314,                           usd?.DefiMarketCap ?? 0 },
            { Parameter.latestquotesdefivolume24h_315,                           usd?.DefiVolume24h ?? 0 },
            { Parameter.latestquotesstablecoinmarketcap_316,                     usd?.StablecoinMarketCap ?? 0 },
            { Parameter.latestquotesstablecoinvolume24h_317,                     usd?.StablecoinVolume24h ?? 0 },
            { Parameter.latestquotesstablecoin24hpercentagechange_318,           usd?.Stablecoin24hPercentageChange ?? 0 },
            { Parameter.latestquotesderivativesvolume24h_319,                    usd?.DerivativesVolume24h ?? 0 },
            { Parameter.latestquotesderivatives24hpercentagechange_320,          usd?.Derivatives24hPercentageChange ?? 0 },
        };

        _protocol.SetParameters(parameters.Keys.ToArray(), parameters.Values.ToArray());
    }

    private CategoriestableQActionRow BuildCategoryRow(CategoryItem item)
    {
        return new CategoriestableQActionRow
        {
            Categoriestableid_2001 = item.Id,
            Categoriestablename_2002 = item.Name,
            Categoriestablenumtokens_2003 = item.NumTokens,
            Categoriestableavgpricechange_2004 = item.AvgPriceChange,
            Categoriestablevolumechange_2005 = item.VolumeChange,
            Categoriestablemarketcapusd_2006 = item.MarketCap,
            Categoriestablemarketcapchange_2007 = item.MarketCapChange,
            Categoriestablevolume24h_2008 = item.Volume,
            Categoriestablelastupdated_2009 = item.LastUpdated,
        };
    }
}