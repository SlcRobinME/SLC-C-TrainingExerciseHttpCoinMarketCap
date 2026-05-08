using System;
using System.Collections.Generic;
using System.Linq;
using QuickType;
using Skyline.DataMiner.Scripting;

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
            Usd usd = item.Quote?.Usd;

            tableRows.Add(new LatestlistingstableQActionRow
            {
                Latestlistingstableid_1001 = Convert.ToString(item.Id),
                Latestlistingstablename_1002 = DataMinerValue.Resolve(item.Name),
                Latestlistingstablesymbol_1003 = DataMinerValue.Resolve(item.Symbol),
                Latestlistingstableslug_1004 = DataMinerValue.Resolve(item.Slug),
                Latestlistingstablecmcrank_1005 = item.CmcRank,

                Latestlistingstablepriceusd_1006 = usd?.Price ?? NotAvailable.Numeric,
                Latestlistingstablepercentchange24h_1007 = usd?.PercentChange24H ?? NotAvailable.Numeric,
                Latestlistingstablemarketcapusd_1008 = usd?.MarketCap ?? NotAvailable.Numeric,
                Latestlistingstablevolume24husd_1009 = usd?.Volume24H ?? NotAvailable.Numeric,
                Latestlistingstablecirculatingsupply_1010 = item.CirculatingSupply,
                Latestlistingstablemaxsupply_1011 = DataMinerValue.ResolveLongDouble(item.MaxSupply),
                Latestlistingstablelastupdated_1012 = DataMinerValue.Resolve(usd?.LastUpdated),
                Latestlistingstablepercentchange1h_1013 = usd?.PercentChange1H ?? NotAvailable.Numeric,
                Latestlistingstablepercentchange7d_1014 = usd?.PercentChange7D ?? NotAvailable.Numeric,
                Latestlistingstablepercentchange30d_1015 = usd?.PercentChange30D ?? NotAvailable.Numeric,
                Latestlistingstablevolumechange24h_1016 = usd?.VolumeChange24H ?? NotAvailable.Numeric,
                Latestlistingstablefullydilutedmarketcap_1017 = usd?.FullyDilutedMarketCap ?? NotAvailable.Numeric,
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
            tableRows.Add(BuildCategoryRow(item));

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
            { Parameter.latestquotestotalmarketcap_300,                          usd?.TotalMarketCap ?? NotAvailable.Numeric },
            { Parameter.latestquotestotalvolume24h_301,                          usd?.TotalVolume24H ?? NotAvailable.Numeric },
            { Parameter.latestquotesbtcdominance_302,                            data.BtcDominance },
            { Parameter.latestquotesethdominance_303,                            data.EthDominance },
            { Parameter.latestquotesactivecryptocurrencies_304,                  data.ActiveCryptocurrencies },
            { Parameter.latestquoteslastupdated_305,                             data.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") },
            { Parameter.latestquotesdefi24hpercentagechange_306,                 data.Defi24hPercentageChange },
            { Parameter.activeexchanges_307,                                     data.ActiveExchanges },
            { Parameter.latestquotestotalmarketcapyesterday_308,                 usd?.TotalMarketCapYesterday ?? NotAvailable.Numeric },
            { Parameter.latestquotestotalmarketcapyesterdaypercentagechange_309, usd?.TotalMarketCapYesterdayPercentageChange ?? NotAvailable.Numeric },
            { Parameter.latestquotestotalvolume24hyesterday_310,                 usd?.TotalVolume24hYesterday ?? NotAvailable.Numeric },
            { Parameter.latestquotestotalvolume24hyesterdaypercentagechange_311, usd?.TotalVolume24hYesterdayPercentageChange ?? NotAvailable.Numeric },
            { Parameter.latestquotesaltcoinmarketcap_312,                        usd?.AltcoinMarketCap ?? NotAvailable.Numeric },
            { Parameter.latestquotesaltcoinvolume24h_313,                        usd?.AltcoinVolume24h ?? NotAvailable.Numeric },
            { Parameter.latestquotesdefimarketcap_314,                           usd?.DefiMarketCap ?? NotAvailable.Numeric },
            { Parameter.latestquotesdefivolume24h_315,                           usd?.DefiVolume24h ?? NotAvailable.Numeric },
            { Parameter.latestquotesstablecoinmarketcap_316,                     usd?.StablecoinMarketCap ?? NotAvailable.Numeric },
            { Parameter.latestquotesstablecoinvolume24h_317,                     usd?.StablecoinVolume24h ?? NotAvailable.Numeric },
            { Parameter.latestquotesstablecoin24hpercentagechange_318,           usd?.Stablecoin24hPercentageChange ?? NotAvailable.Numeric },
            { Parameter.latestquotesderivativesvolume24h_319,                    usd?.DerivativesVolume24h ?? NotAvailable.Numeric },
            { Parameter.latestquotesderivatives24hpercentagechange_320,          usd?.Derivatives24hPercentageChange ?? NotAvailable.Numeric },
        };

        _protocol.SetParameters(parameters.Keys.ToArray(), parameters.Values.ToArray());
    }

    private CategoriestableQActionRow BuildCategoryRow(CategoryItem item)
    {
        return new CategoriestableQActionRow
        {
            Categoriestableid_2001 = item.Id,
            Categoriestablename_2002 = DataMinerValue.Resolve(item.Name),
            Categoriestablenumtokens_2003 = item.NumTokens,
            Categoriestableavgpricechange_2004 = item.AvgPriceChange,
            Categoriestablevolumechange_2005 = item.VolumeChange,
            Categoriestablemarketcapusd_2006 = item.MarketCap,
            Categoriestablemarketcapchange_2007 = item.MarketCapChange,
            Categoriestablevolume24h_2008 = item.Volume,
            Categoriestablelastupdated_2009 = DataMinerValue.Resolve(item.LastUpdated),
        };
    }
}