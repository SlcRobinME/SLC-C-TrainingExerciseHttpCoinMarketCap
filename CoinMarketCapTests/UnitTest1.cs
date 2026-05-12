namespace CoinMarketCapTests
{

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using QAction_1;
    using Skyline.DataMiner.Scripting;
    using Skyline.DataMiner.Utils.UnitTestingFramework.Protocol;
    using static Skyline.DataMiner.Scripting.Parameter;
    [TestClass]
    public class LatestListingsJsonParsingTests
    {

        // Verifies that parsing a valid JSON response returns the correct number of listings.
        [TestMethod]
        public void ParseResponse_ValidJson_ReturnsCorrectListingCount()
		{
			// Arrange & Act
			var result = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);

			Assert.AreEqual(1, result.Data.Count);
        }

        //Verifies that listing properties(id, name, symbol, rank, supply) are correctly mapped from JSON.
        [TestMethod]
        public void ParseResponse_ValidJson_ReturnsCorrectListingProperties()
        {
            // Arrange & Act
            var result = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var listing = result.Data[0];

            // Assert
            Assert.AreEqual(1, listing.Id);
            Assert.AreEqual("Bitcoin", listing.Name);
            Assert.AreEqual("BTC", listing.Symbol);
            Assert.AreEqual(1, listing.CmcRank);
            Assert.AreEqual(19864978.0, listing.CirculatingSupply);
            Assert.AreEqual(21000000.0, listing.MaxSupply);
        }
        //verifies that usd quote properties(price, volume, market cap, percent changes) are correctly mapped from json.
        [TestMethod]
        public void ParseResponse_ValidJson_ReturnsCorrectUsdQuoteProperties()
        {
            // Arrange & Act
            var result = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var usd = result.Data[0].Quote["USD"];
            // Assert
            Assert.AreEqual(101815.90673154943, usd.Price.Value, 0.0001);
            Assert.AreEqual(45620279708.066856, usd.Volume24h.Value, 0.01);
            Assert.AreEqual(2022570747272.2815, usd.MarketCap.Value, 0.01);
            Assert.AreEqual(-0.4304306, usd.PercentChange1h);
            Assert.AreEqual(-1.83894047, usd.PercentChange24h);
            Assert.AreEqual(2.60309348, usd.PercentChange7d);
        }
        //Verifies that an empty data array results in an empty list.
        [TestMethod]
        public void ParseResponse_EmptyDataArray_ReturnsEmptyList()
        {
            // Arrange & Act
            var result = LatestListingsHelper.ParseResponse(TestData.MissingData);
            // Assert
            Assert.AreEqual(0, result.Data.Count);
        }
        //Verifies that a null max supply in JSON is correctly mapped to null.
        [TestMethod]
        public void ParseResponse_NullMaxSupply_MapsToNull()
        {
            // Arrange & Act
            var result = LatestListingsHelper.ParseResponse(TestData.MultipleListingsJson);
            // Assert 
            Assert.IsNull(result.Data[1].MaxSupply);
        }
        //Verifies that all listings are returned when multiple listings are present in the response.
        [TestMethod]
        public void ParseResponse_MultipleListings_ReturnsAllItems()
        {
            // Arrange & Act
            var result = LatestListingsHelper.ParseResponse(TestData.MultipleListingsJson);
            // Assert
            Assert.AreEqual(2, result.Data.Count);
        }
        //Verifies that a null max supply defaults to zero when building a table row.
        [TestMethod]
        public void BuildRow_NullMaxSupply_DefaultsToZero()
        {
            // Arrange
            var result = LatestListingsHelper.ParseResponse(TestData.MultipleListingsJson);
            var ethereum = result.Data[1];
            var usd = ethereum.Quote["USD"];
            // Act
            var row = LatestListingsHelper.BuildRow(ethereum, usd);
            // Assert
            Assert.AreEqual(-1.0, row.Latestlistingsmaxsupply_1006);
        }
        //Verifies that the listing ID is correctly converted to string when building a row.
        [TestMethod]
        public void BuildRow_ValidListing_ReturnsCorrectId()
        {
            // Arrange
            var result = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var bitcoin = result.Data[0];
            var usd = bitcoin.Quote["USD"];
            // Act
            var row = LatestListingsHelper.BuildRow(bitcoin, usd);
            // Assert
            Assert.AreEqual("1", row.Latestlistingsid_1001);
        }
        //Verifies that the USD price is correctly mapped when building a row.
        [TestMethod]
        public void BuildRow_ValidListing_ReturnsCorrectPrice()
        {
            // Arrange
            var result = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var bitcoin = result.Data[0];
            var usd = bitcoin.Quote["USD"];
            // Act
            var row = LatestListingsHelper.BuildRow(bitcoin, usd);
            // Assert
            Assert.AreEqual(101815.90673154943, row.Latestlistingspriceusd_1007);
        }
        //Verifies that an empty list is returned when no listings have a USD quote.
       [TestMethod]
        public void BuildRows_NoItemsHaveUsdQuote_ReturnsEmptyList()
        {
            // Arrange
            var data = LatestListingsHelper.ParseResponse(TestData.MissingUSDQuote);
            // Act
            var rows = LatestListingsHelper.BuildRows(data);
            // Assert
            Assert.AreEqual(0, rows.Count);
        }
    }

    [TestClass]
    public class LatestListingsTablePopulationTests
    {
        //Verifies that filling the table with a single listing results in one row.
        [TestMethod]
        public void FillTable_SingleListing_TableRowCountIsOne()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var rows = LatestListingsHelper.BuildRows(data);
            // Act
            LatestListingsHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Latestlistings.tablePid).RowCount.Should().Be(1);
        }
        //Verifies that filling the table with multiple listings results in the correct row count.
        [TestMethod]
        public void FillTable_MultipleListings_TableRowCountCorrect()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = LatestListingsHelper.ParseResponse(TestData.MultipleListingsJson);
            var rows = LatestListingsHelper.BuildRows(data);
            // Act
            LatestListingsHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Latestlistings.tablePid).RowCount.Should().Be(2);
        }
        //Verifies that the table row contains correct name, symbol, and rank values.
        [TestMethod]
        public void FillTable_SingleListing_RowContainsCorrectParameters()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = LatestListingsHelper.ParseResponse(TestData.SingleListingJson);
            var rows = LatestListingsHelper.BuildRows(data);
            // Act
            LatestListingsHelper.FillTable(protocolMock.Object, rows);
            // Assert
            Assert.AreEqual("Bitcoin", protocolMock.Assert().Table(Latestlistings.tablePid).Row<LatestlistingsQActionRow>("1").Latestlistingsname_1002);
            Assert.AreEqual("BTC", protocolMock.Assert().Table(Latestlistings.tablePid).Row<LatestlistingsQActionRow>("1").Latestlistingssymbol_1003);
            Assert.AreEqual(1.0, protocolMock.Assert().Table(Latestlistings.tablePid).Row<LatestlistingsQActionRow>("1").Latestlistingsrank_1004);
        }
        //Verifies that filling the table with an empty list results in an empty table.
        [TestMethod]
        public void FillTable_EmptyRows_TableRemainsEmpty()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = LatestListingsHelper.ParseResponse(TestData.MissingData);
            var rows = LatestListingsHelper.BuildRows(data);
            // Act
            LatestListingsHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Latestlistings.tablePid).RowCount.Should().Be(0);
        }
    }


    [TestClass]
    public class CategoriesJsonParsingTests
    {
        //Verifies that parsing a valid JSON response returns the correct number of categories.
        [TestMethod]
        public void ParseResponse_ValidJson_ReturnsCorrectCategoryCount()
        {
            // Arrange & Act
            var result = CategoriesHelper.ParseResponse(TestData.SingleCategoryJson);
            // Assert
            Assert.AreEqual(1, result.Data.Count);
        }
        //Verifies that all category properties are correctly mapped from JSON.
        [TestMethod]
        public void ParseResponse_ValidJson_ReturnsCorrectCategoryProperties()
        {
            // Arrange & Act
            var result = CategoriesHelper.ParseResponse(TestData.SingleCategoryJson);
            var item = result.Data[0];
            // Assert
            Assert.AreEqual("6823f463f4035758156a501c", item.Id);
            Assert.AreEqual("Internet Capital Markets", item.Name);
            Assert.AreEqual(8, item.NumTokens);
            Assert.AreEqual(-19.930721654598546, item.AvgPriceChange);
            Assert.AreEqual(319300362.08430588, item.MarketCap);
            Assert.AreEqual(-18.342860046142, item.MarketCapChange);
            Assert.AreEqual(490986489.37452543, item.Volume);
            Assert.AreEqual(-0.565025088224, item.VolumeChange);
            Assert.AreEqual("2026-05-07T05:57:27.0338434Z", item.LastUpdated);
        }
        //Verifies that all categories are returned when multiple categories are present in the response.
        [TestMethod]
        public void ParseResponse_MultipleCategories_ReturnsAllItems()
        {
            // Arrange & Act
            var result = CategoriesHelper.ParseResponse(TestData.MultipleCategoriesJson);
            // Assert
            Assert.AreEqual(3, result.Data.Count);
        }
        //Verifies that an empty data array results in an empty list.
        [TestMethod]
        public void ParseResponse_EmptyData_ReturnsEmptyList()
        {
            // Arrange & Act
            var result = CategoriesHelper.ParseResponse(TestData.MissingData);
            // Assert
            Assert.AreEqual(0, result.Data.Count);
        }
        //Verifies that negative values for price change, market cap change, and volume change are correctly parsed.
        [TestMethod]
        public void ParseResponse_NegativeValues_ParsedCorrectly()
        {
            // Arrange & Act — sva tri iz Postmana imaju negativne vrijednosti
            var result = CategoriesHelper.ParseResponse(TestData.MultipleCategoriesJson);
            // Assert
            Assert.IsTrue(result.Data[0].AvgPriceChange < 0);
            Assert.IsTrue(result.Data[0].MarketCapChange < 0);
            Assert.IsTrue(result.Data[1].VolumeChange < 0);
        }
    }
    [TestClass]
    public class CategoriesTablePopulationTests
    {
        //Verifies that filling the table with a single category results in one row.
        [TestMethod]
        public void FillTable_SingleCategory_TableRowCountIsOne()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = CategoriesHelper.ParseResponse(TestData.SingleCategoryJson);
            var rows = CategoriesHelper.BuildRows(data);
            // Act
            CategoriesHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Categories.tablePid).RowCount.Should().Be(1);
        }
        //Verifies that filling the table with multiple categories results in the correct row count.
        [TestMethod]
        public void FillTable_MultipleCategories_TableRowCountCorrect()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = CategoriesHelper.ParseResponse(TestData.MultipleCategoriesJson);
            var rows = CategoriesHelper.BuildRows(data);
            // Act
            CategoriesHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Categories.tablePid).RowCount.Should().Be(3);
        }
        //Verifies that the table row contains correct name and num tokens values.
        [TestMethod]
        public void FillTable_SingleCategory_RowContainsCorrectParameters()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = CategoriesHelper.ParseResponse(TestData.SingleCategoryJson);
            var rows = CategoriesHelper.BuildRows(data);
            // Act
            CategoriesHelper.FillTable(protocolMock.Object, rows);
            // Assert
            Assert.AreEqual("Internet Capital Markets", protocolMock.Assert().Table(Categories.tablePid).Row<CategoriesQActionRow>("6823f463f4035758156a501c").Categoriesname_2002);
            Assert.AreEqual(8, protocolMock.Assert().Table(Categories.tablePid).Row<CategoriesQActionRow>("6823f463f4035758156a501c").Categoriesnumtokens_2003);

        }
        //Verifies that filling the table with an empty list results in an empty table.
        [TestMethod]
        public void FillTable_EmptyRows_TableRemainsEmpty()
        {
            // Arrange
            var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
            var data = CategoriesHelper.ParseResponse(TestData.MissingData);
            var rows = CategoriesHelper.BuildRows(data);
            // Act
            CategoriesHelper.FillTable(protocolMock.Object, rows);
            // Assert
            protocolMock.Assert().Table(Categories.tablePid).RowCount.Should().Be(0);
        }

    }
    [TestClass]
    public class CategoryDetailTests
    {
        //Verifies that a valid JSON response is considered valid.
        [TestMethod]
        public void IsValidResponse_ValidJson_ReturnsTrue()
        {
            // Arrange & Act
            bool result = CategoryDetailHelper.IsValidResponse(TestData.SingleCategoryDetailJson);
            // Assert
            Assert.IsTrue(result);
        }
        //Verifies that a null string is considered invalid.
        [TestMethod]
        public void IsValidResponse_NullString_ReturnsFalse()
        {
            // Arrange & Act
            bool result = CategoryDetailHelper.IsValidResponse(null);
            // Assert
            Assert.IsFalse(result);
        }
        //Verifies that an empty string is considered invalid.
       [TestMethod]
        public void IsValidResponse_EmptyString_ReturnsFalse()
        {
            // Arrange & Act
            bool result = CategoryDetailHelper.IsValidResponse(string.Empty);
            // Assert
            Assert.IsFalse(result);
        }
        //Verifies that a whitespace-only string is considered invalid.
        [TestMethod]
        public void IsValidResponse_WhitespaceOnly_ReturnsFalse()
        {
            // Arrange & Act
            bool result = CategoryDetailHelper.IsValidResponse("   ");
            // Assert
            Assert.IsFalse(result);
        }
        //Verifies that a "Bad Request" string is considered invalid.
       [TestMethod]
        public void IsValidResponse_BadRequest_ReturnsFalse()
        {
            // Arrange & Act
            bool result = CategoryDetailHelper.IsValidResponse("Bad Request");
            // Assert
            Assert.IsFalse(result);
        }
        //Verifies that all category detail fields are correctly mapped from JSON.
        [TestMethod]
        public void ParseResponse_ValidJson_MapsAllFields()
        {
            // Arrange & Act
            var result = CategoryDetailHelper.ParseResponse(TestData.SingleCategoryDetailJson);
            // Assert
            Assert.AreEqual("6823f463f4035758156a501c", result.Data.Id);
            Assert.AreEqual(8, result.Data.NumTokens);
            Assert.AreEqual(-19.930721654598546, result.Data.AvgPriceChange.Value, 0.0001);
            Assert.AreEqual(319300362.08430588, result.Data.MarketCap.Value, 0.01);
            Assert.AreEqual(-18.342860046142, result.Data.MarketCapChange.Value, 0.0001);
            Assert.AreEqual(490986489.37452543, result.Data.Volume.Value, 0.01);
            Assert.AreEqual(-0.565025088224, result.Data.VolumeChange.Value, 0.0001);
            Assert.AreEqual("2026-05-07T05:57:27.0338434Z", result.Data.LastUpdated);
        }
        //Verifies that a null data property in JSON is correctly mapped to null.
        [TestMethod]
        public void ParseResponse_NullData_DataPropertyIsNull()
		{
			// Arrange & Act
			var result = CategoryDetailHelper.ParseResponse(TestData.MissingDataCategoryDetail);
			// Assert
            Assert.IsNull(result.Data);
        }

    }
}
