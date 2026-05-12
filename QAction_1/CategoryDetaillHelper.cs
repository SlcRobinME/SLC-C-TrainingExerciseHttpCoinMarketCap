namespace QAction_1
{
    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

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
            return SecureNewtonsoftDeserialization.DeserializeObject<CategoryDetailResponse>(response);
        }

        public static void UpdateCategoryRow(SLProtocolExt protocol, CategoryData data)
        {
            var row = new CategoriesQActionRow
            {
                Categoriesid_2001 = data.Id,
                Categoriesnumtokens_2003 = data.NumTokens ?? -1,
                Categoriesavgpricechange_2004 = data.AvgPriceChange ?? -1,
                Categoriesmarketcap_2005 = data.MarketCap ?? -1,
                Categoriesmarketcapchange_2006 = data.MarketCapChange ?? -1,
                Categoriesvolume_2007 = data.Volume ?? -1,
                Categoriesvolumechange_2008 = data.VolumeChange ?? -1,
                Categorieslastupdated_2009 = data.LastUpdated,
            };

            protocol.SetRow(Parameter.Categories.tablePid, data.Id, row.ToObjectArray());
        }
    }
}
