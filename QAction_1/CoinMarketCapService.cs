using System;
using System.Collections.Generic;
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
        _protocol.SetParameter(Parameter.latestquotestotalmarketcap_300, usd?.TotalMarketCap ?? 0);
        _protocol.SetParameter(Parameter.latestquotestotalvolume24h_301, usd?.TotalVolume24H ?? 0);
        _protocol.SetParameter(Parameter.latestquotesbtcdominance_302, data.BtcDominance);
        _protocol.SetParameter(Parameter.latestquotesethdominance_303, data.EthDominance);
        _protocol.SetParameter(Parameter.latestquotesactivecryptocurrencies_304, data.ActiveCryptocurrencies);
        _protocol.SetParameter(Parameter.latestquoteslastupdated_305, data.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss"));
        _protocol.SetParameter(Parameter.latestquotesdefi24hpercentagechange_306, data.Defi24hPercentageChange);
        _protocol.SetParameter(Parameter.activeexchanges_307, data.ActiveExchanges);

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