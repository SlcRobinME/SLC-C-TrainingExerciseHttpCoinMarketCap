namespace QAction_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

    public class CategoriesHelper
    {
        public static CategoriesResponse ParseResponse(string response)
        {
            return SecureNewtonsoftDeserialization.DeserializeObject<CategoriesResponse>(response);
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
                Categoriesnumtokens_2003 = item.NumTokens ?? -1,
                Categoriesavgpricechange_2004 = item.AvgPriceChange ?? -1,
                Categoriesmarketcap_2005 = item.MarketCap ?? -1,
                Categoriesmarketcapchange_2006 = item.MarketCapChange ?? -1,
                Categoriesvolume_2007 = item.Volume ?? -1,
                Categoriesvolumechange_2008 = item.VolumeChange ?? -1,
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
}
