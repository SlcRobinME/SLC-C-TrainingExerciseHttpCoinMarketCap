using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_1
{
    public class LatestListingsResponse
    {
        [JsonProperty("data")]
        public List<Listing> Data { get; set; }
    }

    public class Listing
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("cmc_rank")]
        public int CmcRank { get; set; }

        [JsonProperty("circulating_supply")]
        public double CirculatingSupply { get; set; }

        [JsonProperty("max_supply")]
        public double? MaxSupply { get; set; }

        [JsonProperty("quote")]
        public Dictionary<string, Quote> Quote { get; set; }
    }

    public class Quote
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7d { get; set; }
    }

    public class CategoriesResponse
    {
        [JsonProperty("data")]
        public List<CategoryData> Data { get; set; }
    }

    public class CategoryDetailResponse
    {
        [JsonProperty("data")]
        public CategoryData Data { get; set; }
    }

    public class CategoryData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("num_tokens")]
        public int NumTokens { get; set; }

        [JsonProperty("avg_price_change")]
        public double AvgPriceChange { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("market_cap_change")]
        public double MarketCapChange { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("volume_change")]
        public double VolumeChange { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }
    }


    public class LatestQuotesResponse
    {
        [JsonProperty("data")]
        public LatestQuotesData Data { get; set; }
    }

    public class LatestQuotesData
    {
        [JsonProperty("active_cryptocurrencies")]
        public int ActiveCryptocurrencies { get; set; }

        [JsonProperty("btc_dominance")]
        public double BtcDominance { get; set; }

        [JsonProperty("eth_dominance")]
        public double EthDominance { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        [JsonProperty("quote")]
        public QuoteData Quote { get; set; }
    }

    public class QuoteData
    {
        [JsonProperty("USD")]
        public UsdQuote Usd { get; set; }
    }

    public class UsdQuote
    {
        [JsonProperty("total_market_cap")]
        public double TotalMarketCap { get; set; }

        [JsonProperty("total_volume_24h")]
        public double TotalVolume24h { get; set; }

        [JsonProperty("defi_market_cap")]
        public double DefiMarketCap { get; set; }

        [JsonProperty("stablecoin_market_cap")]
        public double StablecoinMarketCap { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }
    }
}
