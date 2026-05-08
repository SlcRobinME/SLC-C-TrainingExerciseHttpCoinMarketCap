using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMarketCapTests
{
    public class TestData
    {
        public const string SingleListingJson = @"{
            ""status"": {
                ""timestamp"": ""2026-05-07T06:56:10.818448Z"",
                ""error_code"": 0,
                ""error_message"": null,
                ""elapsed"": 12,
                ""credit_count"": 1,
                ""notice"": null,
                ""total_count"": 9783
            },
            ""data"": [{
                ""id"": 1,
                ""name"": ""Bitcoin"",
                ""symbol"": ""BTC"",
                ""slug"": ""bitcoin"",
                ""cmc_rank"": 1,
                ""max_supply"": 21000000.0,
                ""circulating_supply"": 19864978.0,
                ""total_supply"": 19864978.0,
                ""infinite_supply"": false,
                ""last_updated"": ""2026-05-07T05:56:10.818448Z"",
                ""quote"": {
                    ""USD"": {
                        ""price"": 101815.90673154943,
                        ""volume_24h"": 45620279708.066856,
                        ""volume_change_24h"": -7.5884,
                        ""percent_change_1h"": -0.4304306,
                        ""percent_change_24h"": -1.83894047,
                        ""percent_change_7d"": 2.60309348,
                        ""market_cap"": 2022570747272.2815,
                        ""market_cap_dominance"": 61.8788,
                        ""last_updated"": ""2026-05-07T05:56:10.818448Z""
                    }
                }
            }]
        }";


        public const string MultipleListingsJson = @"{
            ""status"": {
                    ""timestamp"": ""2026-05-07T06:56:10.818448Z"", 
                    ""error_code"": 0, 
                    ""error_message"": null, 
                    ""elapsed"": 12, 
                    ""credit_count"": 1, 
                    ""notice"": null, 
                    ""total_count"": 9783 },
                        ""data"": [
                            {
                                ""id"": 1, 
                                ""name"": ""Bitcoin"", 
                                ""symbol"": ""BTC"", 
                                ""slug"": ""bitcoin"", 
                                ""cmc_rank"": 1,
                                ""max_supply"": 21000000.0, 
                                ""circulating_supply"": 19864978.0, 
                                ""total_supply"": 19864978.0, 
                                ""infinite_supply"": false,
                                ""last_updated"": ""2026-05-07T05:56:10.818448Z"",
                                ""quote"": { 
                                      ""USD"": { 
                                            ""price"": 101815.90673154943, 
                                            ""volume_24h"": 45620279708.066856, 
                                            ""volume_change_24h"": -7.5884, 
                                            ""percent_change_1h"": -0.4304306, 
                                            ""percent_change_24h"": -1.83894047, 
                                            ""percent_change_7d"": 2.60309348, 
                                            ""market_cap"": 2022570747272.2815, 
                                            ""market_cap_dominance"": 61.8788, 
                                            ""last_updated"": ""2026-05-07T05:56:10.818448Z"" 
                                 }
                               }
                            },
                            {
                                ""id"": 1027, 
                                ""name"": ""Ethereum"", 
                                ""symbol"": ""ETH"", 
                                ""slug"": ""ethereum"", 
                                ""cmc_rank"": 2,
                                ""max_supply"": null, 
                                ""circulating_supply"": 120000000.0, 
                                ""total_supply"": 120000000.0, 
                                ""infinite_supply"": true,
                                ""last_updated"": ""2026-05-07T05:56:10.818448Z"",
                                ""quote"": { 
                                        ""USD"": { 
                                            ""price"": 3000.0, 
                                            ""volume_24h"": 15000000000.0, 
                                            ""volume_change_24h"": -3.2, 
                                            ""percent_change_1h"": -0.3, 
                                            ""percent_change_24h"": 2.1, 
                                            ""percent_change_7d"": 5.0, 
                                            ""market_cap"": 360000000000.0, 
                                            ""market_cap_dominance"": 9.35, 
                                            ""last_updated"": ""2026-05-07T05:56:10.818448Z"" } }
                            }
                        ]
        }";

        public const string MissingUSDQuote = @"{ 
                            ""status"": { 
                                ""timestamp"": ""2026-05-07T06:56:10.818448Z"", 
                                ""error_code"": 0, 
                                ""error_message"": null, 
                                ""elapsed"": 12, 
                                ""credit_count"": 1, 
                                ""notice"": null, 
                                ""total_count"": 1 
                                        }, 
                            ""data"": [{ 
                                    ""id"": 1, 
                                    ""name"": ""EuroCoin"", 
                                    ""symbol"": ""EUR"", 
                                    ""cmc_rank"": 1, 
                                    ""circulating_supply"": 1000000.0, 
                                    ""max_supply"": null, 
                                    ""quote"": { 
                                        ""EUR"": { 
                                            ""price"": 1.0, 
                                            ""volume_24h"": 0, 
                                            ""volume_change_24h"": 0, 
                                            ""percent_change_1h"": 0, 
                                            ""percent_change_24h"": 0, 
                                            ""percent_change_7d"": 0, 
                                            ""market_cap"": 0, 
                                            ""market_cap_dominance"": 0, 
                                            ""last_updated"": ""2026-05-07T05:56:10.818448Z"" 
                                                  } 
                                                } 
                                        }] 
                        }";
        public const string MissingData = @"{ ""status"": 
                                        { 
                                        ""timestamp"": ""2026-05-07T06:56:10.818448Z"", 
                                        ""error_code"": 0, 
                                        ""error_message"": null, 
                                        ""elapsed"": 12, 
                                        ""credit_count"": 1, 
                                        ""notice"": null, 
                                        ""total_count"": 0 
                                        }, 
                            ""data"": [] 
                            }";

        public const string SingleCategoryJson = @"{ 
                                                    ""status"": { 
                                                            ""timestamp"": ""2026-05-07T06:57:27.0338434Z"", 
                                                            ""error_code"": 0, 
                                                            ""error_message"": null, 
                                                            ""elapsed"": 6, 
                                                            ""credit_count"": 1, 
                                                            ""notice"": null 
                                                                }, 
                                                    ""data"": [{ 
                                                            ""id"": ""6823f463f4035758156a501c"", 
                                                            ""name"": ""Internet Capital Markets"", 
                                                            ""title"": ""Internet Capital Markets"", 
                                                            ""description"": ""Internet Capital Markets"", 
                                                            ""num_tokens"": 8, 
                                                            ""avg_price_change"": -19.930721654598546, 
                                                            ""market_cap"": 319300362.08430588, 
                                                            ""market_cap_change"": -18.342860046142, 
                                                            ""volume"": 490986489.37452543, 
                                                            ""volume_change"": -0.565025088224, 
                                                            ""last_updated"": ""2026-05-07T05:57:27.0338434Z"" 
                                                            }] 
                                                    }";

        public const string MultipleCategoriesJson = @"{ 
                                                    ""status"": { 
                                                            ""timestamp"": ""2026-05-07T06:57:27.0338434Z"", 
                                                            ""error_code"": 0, 
                                                            ""error_message"": null, 
                                                            ""elapsed"": 6, 
                                                            ""credit_count"": 1, 
                                                            ""notice"": null 
                                                            }, 
                                                    ""data"": [ { 
                                                            ""id"": ""6823f463f4035758156a501c"", 
                                                            ""name"": ""Internet Capital Markets"", 
                                                            ""title"": ""Internet Capital Markets"", 
                                                            ""description"": ""Internet Capital Markets"", 
                                                            ""num_tokens"": 8, 
                                                            ""avg_price_change"": -19.930721654598546, 
                                                            ""market_cap"": 319300362.08430588, 
                                                            ""market_cap_change"": -18.342860046142, 
                                                            ""volume"": 490986489.37452543, 
                                                            ""volume_change"": -0.565025088224, 
                                                            ""last_updated"": ""2026-05-07T05:57:27.0338434Z"" 
                                                            }, { 
                                                            ""id"": ""68130fbed34f151a9a19ded2"", 
                                                            ""name"": ""Sonic Ecosystem"", 
                                                            ""title"": ""Sonic Ecosystem"", 
                                                            ""description"": ""Sonic Ecosystem"", 
                                                            ""num_tokens"": 11, 
                                                            ""avg_price_change"": -11.373271582451567, 
                                                            ""market_cap"": 1648744123.5993054, 
                                                            ""market_cap_change"": -10.480864162102, 
                                                            ""volume"": 174262574.393657, 
                                                            ""volume_change"": -30.109502696623, 
                                                            ""last_updated"": ""2026-05-07T05:57:27.0338434Z"" 
                                                            }, { 
                                                            ""id"": ""681079f420466a6e284b7364"", 
                                                            ""name"": ""Bonk Fun Ecosystem"", 
                                                            ""title"": ""Bonk Fun Ecosystem"", 
                                                            ""description"": ""Bonk Fun Ecosystem"", 
                                                            ""num_tokens"": 3, 
                                                            ""avg_price_change"": -19.576538661209458, 
                                                            ""market_cap"": 26184771.219408084, 
                                                            ""market_cap_change"": -20.106692061923, 
                                                            ""volume"": 27936623.791032258, 
                                                            ""volume_change"": -19.565319801208, 
                                                            ""last_updated"": ""2026-05-07T05:57:27.0338434Z"" 
                                                            } ] 
                                                       }";

        public const string SingleCategoryDetailJson = @"{ 
                                                    ""status"": { 
                                                            ""timestamp"": ""2026-05-07T06:57:27.0338434Z"", 
                                                            ""error_code"": 0, 
                                                            ""error_message"": null, 
                                                            ""elapsed"": 6, 
                                                            ""credit_count"": 1, 
                                                            ""notice"": null 
                                                                }, 
                                                    ""data"": { 
                                                            ""id"": ""6823f463f4035758156a501c"", 
                                                            ""name"": ""Internet Capital Markets"", 
                                                            ""title"": ""Internet Capital Markets"", 
                                                            ""description"": ""Internet Capital Markets"", 
                                                            ""num_tokens"": 8, 
                                                            ""avg_price_change"": -19.930721654598546, 
                                                            ""market_cap"": 319300362.08430588, 
                                                            ""market_cap_change"": -18.342860046142, 
                                                            ""volume"": 490986489.37452543, 
                                                            ""volume_change"": -0.565025088224, 
                                                            ""last_updated"": ""2026-05-07T05:57:27.0338434Z"" 
                                                            } 
                                                    }";
        public const string MissingDataCategoryDetail = @"{ ""status"": { 
                                    ""timestamp"": ""2026-05-07T06:57:27.0338434Z"", 
                                    ""error_code"": 0, 
                                    ""error_message"": null, 
                                    ""elapsed"": 6, 
                                    ""credit_count"": 1, 
                                    ""notice"": null 
                                        }, 
                              ""data"": null
                        }";
    }
}
