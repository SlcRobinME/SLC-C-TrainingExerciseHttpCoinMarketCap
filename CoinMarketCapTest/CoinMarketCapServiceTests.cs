namespace Skyline.DataMiner.Utils.UnitTestingFramework.Tests.Protocol
{
	using System;
	using FluentAssertions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using QuickType;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Utils.UnitTestingFramework.Protocol;

	[TestClass]
	public class CoinMarketCapServiceTests
	{
		[TestMethod]
		public void FillLatestListings_SingleItem_RowStoredInTable()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var lastUpdated = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);
			var welcome = new Welcome
			{
				Data = new[]
				{
					new Datum
					{
						Id = 1,
						Name = "Bitcoin",
						Symbol = "BTC",
						Slug = "bitcoin",
						CmcRank = 1,
						CirculatingSupply = 19_000_000,
						MaxSupply = 21_000_000,
						Quote = new Quote
						{
							Usd = new Usd
							{
								Price            = 45_000.50,
								PercentChange24H = 2.5,
								MarketCap        = 855_009_500_000,
								Volume24H        = 30_000_000_000,
								LastUpdated      = lastUpdated,
							},
						},
					},
				},
			};

			// Act
			service.FillLatestListings(welcome);

			// Assert
			protocolMock.Assert()
				.Table(Parameter.Latestlistingstable.tablePid)
				.RowCount.Should().Be(1);

			protocolMock.Assert()
				.Table(Parameter.Latestlistingstable.tablePid)
				.Row<LatestlistingstableQActionRow>("1")
				.Should().BeEquivalentTo(
					new
					{
						Latestlistingstableid_1001 = "1",
						Latestlistingstablename_1002 = "Bitcoin",
						Latestlistingstablesymbol_1003 = "BTC",
						Latestlistingstableslug_1004 = "bitcoin",
						Latestlistingstablecmcrank_1005 = 1,
						Latestlistingstablepriceusd_1006 = 45_000.50,
						Latestlistingstablepercentchange24h_1007 = 2.5,
						Latestlistingstablemarketcapusd_1008 = 855_009_500_000,
						Latestlistingstablevolume24husd_1009 = 30_000_000_000,
						Latestlistingstablecirculatingsupply_1010 = 19_000_000,
						Latestlistingstablemaxsupply_1011 = 21_000_000,
						Latestlistingstablelastupdated_1012 = "2024-01-15 10:30:00",
					}, options => options.ExcludingMissingMembers());
		}

		[TestMethod]
		public void FillLatestListings_NullQuote_DefaultValuesStored()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var welcome = new Welcome
			{
				Data = new[]
				{
					new Datum
					{
						Id = 2,
						Name = "SomeCoin",
						Symbol = "SC",
						Slug = "somecoin",
						CmcRank = 100,
						CirculatingSupply = 1_000_000,
						MaxSupply = null,
						Quote = null,
					},
				},
			};

			// Act
			service.FillLatestListings(welcome);

			// Assert
			var row = protocolMock.Assert()
				.Table(Parameter.Latestlistingstable.tablePid)
				.Row<LatestlistingstableQActionRow>("2");

			Assert.AreEqual(NotAvailable.Numeric, row.Latestlistingstablepriceusd_1006);
			Assert.AreEqual(NotAvailable.Numeric, row.Latestlistingstablepercentchange24h_1007);
			Assert.AreEqual(NotAvailable.Numeric, row.Latestlistingstablemarketcapusd_1008);
			Assert.AreEqual(NotAvailable.Numeric, row.Latestlistingstablevolume24husd_1009);
			Assert.AreEqual(NotAvailable.Numeric, row.Latestlistingstablemaxsupply_1011);
			Assert.AreEqual(NotAvailable.DateTime, row.Latestlistingstablelastupdated_1012);
		}

		[TestMethod]
		public void FillLatestListings_MultipleItems_AllRowsStoredInTable()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var welcome = new Welcome
			{
				Data = new[]
				{
					new Datum { Id = 1, Name = "Bitcoin",  Symbol = "BTC",  Slug = "bitcoin",  CmcRank = 1, Quote = new Quote { Usd = new Usd { LastUpdated = DateTimeOffset.UtcNow } } },
					new Datum { Id = 2, Name = "Ethereum", Symbol = "ETH",  Slug = "ethereum", CmcRank = 2, Quote = new Quote { Usd = new Usd { LastUpdated = DateTimeOffset.UtcNow } } },
					new Datum { Id = 3, Name = "Tether",   Symbol = "USDT", Slug = "tether",   CmcRank = 3, Quote = new Quote { Usd = new Usd { LastUpdated = DateTimeOffset.UtcNow } } },
				},
			};

			// Act
			service.FillLatestListings(welcome);

			// Assert
			protocolMock.Assert()
				.Table(Parameter.Latestlistingstable.tablePid)
				.RowCount.Should().Be(3);

			protocolMock.Assert()
				.Table(Parameter.Latestlistingstable.tablePid)
				.AllRows().Should().ContainKeys("1", "2", "3");
		}

		[TestMethod]
		public void FillCategories_SingleItem_RowStoredInTable()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var response = new CategoriesResponse
			{
				Data = new[]
				{
					new CategoryItem
					{
						Id          = "cat-001",
						Name        = "DeFi",
						NumTokens   = 150,
						AvgPriceChange = 3.2,
						VolumeChange   = 1.5,
						MarketCap      = 500_000_000,
						MarketCapChange = 2.1,
						Volume         = 100_000_000,
						LastUpdated    = "2024-01-15 10:00:00",
					},
				},
			};

			// Act
			service.FillCategories(response);

			// Assert
			protocolMock.Assert()
				.Table(Parameter.Categoriestable.tablePid)
				.RowCount.Should().Be(1);

			protocolMock.Assert()
				.Table(Parameter.Categoriestable.tablePid)
				.Row<CategoriestableQActionRow>("cat-001")
				.Should().BeEquivalentTo(
				new CategoriestableQActionRow
				{
					Categoriestableid_2001 = "cat-001",
					Categoriestablename_2002 = "DeFi",
					Categoriestablenumtokens_2003 = 150,
					Categoriestableavgpricechange_2004 = 3.2,
					Categoriestablevolumechange_2005 = 1.5,
					Categoriestablemarketcapusd_2006 = 500_000_000,
					Categoriestablemarketcapchange_2007 = 2.1,
					Categoriestablevolume24h_2008 = 100_000_000,
					Categoriestablelastupdated_2009 = "2024-01-15 10:00:00",
				}, options => options
					.ExcludingMissingMembers()
					.Excluding(r => r.Columns));
		}

		[TestMethod]
		public void FillCategories_MultipleItems_AllRowsStoredInTable()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var response = new CategoriesResponse
			{
				Data = new[]
				{
					new CategoryItem { Id = "cat-001", Name = "DeFi" },
					new CategoryItem { Id = "cat-002", Name = "NFT" },
				},
			};

			// Act
			service.FillCategories(response);

			// Assert
			protocolMock.Assert()
				.Table(Parameter.Categoriestable.tablePid)
				.AllRows().Should().ContainKeys("cat-001", "cat-002");
		}

		[TestMethod]
		public void UpdateCategoryRow_ExistingRow_RowUpdatedInTable()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var initial = new CategoryItem { Id = "cat-001", Name = "DeFi", MarketCap = 100 };
			service.FillCategories(new CategoriesResponse { Data = new[] { initial } });

			// Act
			var updated = new CategoryItem { Id = "cat-001", Name = "DeFi Updated", MarketCap = 999 };
			service.UpdateCategoryRow(updated);

			// Assert
			var row = protocolMock.Assert()
				.Table(Parameter.Categoriestable.tablePid)
				.Row<CategoriestableQActionRow>("cat-001");

			Assert.AreEqual("DeFi Updated", row.Categoriestablename_2002);
			Assert.AreEqual(999d, row.Categoriestablemarketcapusd_2006);
		}

		[TestMethod]
		public void FillLatestQuotes_ValidData_ParametersStored()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var lastUpdated = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero);
			var data = new GlobalQuotesData
			{
				BtcDominance = 52.3,
				EthDominance = 17.1,
				ActiveCryptocurrencies = 10_000,
				ActiveExchanges = 500,
				Defi24hPercentageChange = 1.2,
				LastUpdated = lastUpdated,
				Quote = new GlobalQuote
				{
					Usd = new GlobalUsd
					{
						TotalMarketCap = 2_500_000_000_000,
						TotalVolume24H = 120_000_000_000,
					},
				},
			};

			// Act
			service.FillLatestQuotes(data);

			// Assert
			Assert.AreEqual(2_500_000_000_000d, protocolMock.Assert().Parameter(Parameter.latestquotestotalmarketcap_300).Value);
			Assert.AreEqual(120_000_000_000d, protocolMock.Assert().Parameter(Parameter.latestquotestotalvolume24h_301).Value);
			Assert.AreEqual(52.3d, protocolMock.Assert().Parameter(Parameter.latestquotesbtcdominance_302).Value);
			Assert.AreEqual(17.1d, protocolMock.Assert().Parameter(Parameter.latestquotesethdominance_303).Value);
			Assert.AreEqual(10_000L, protocolMock.Assert().Parameter(Parameter.latestquotesactivecryptocurrencies_304).Value);
			Assert.AreEqual("2024-06-01 12:00:00", protocolMock.Assert().Parameter(Parameter.latestquoteslastupdated_305).Value);
			Assert.AreEqual(1.2d, protocolMock.Assert().Parameter(Parameter.latestquotesdefi24hpercentagechange_306).Value);
			Assert.AreEqual(500L, protocolMock.Assert().Parameter(Parameter.activeexchanges_307).Value);
		}

		[TestMethod]
		public void FillLatestQuotes_NullQuote_DefaultValuesStored()
		{
			// Arrange
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			var data = new GlobalQuotesData
			{
				BtcDominance = 50.0,
				EthDominance = 15.0,
				ActiveCryptocurrencies = 9_000,
				ActiveExchanges = 400,
				Defi24hPercentageChange = 0.5,
				LastUpdated = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
				Quote = null,
			};

			// Act
			service.FillLatestQuotes(data);

			// Assert
			Assert.AreEqual(NotAvailable.Numeric, protocolMock.Assert().Parameter(Parameter.latestquotestotalmarketcap_300).Value);
			Assert.AreEqual(NotAvailable.Numeric, protocolMock.Assert().Parameter(Parameter.latestquotestotalvolume24h_301).Value);
		}

		[TestMethod]
		public void FillLatestListings_NullWelcome_ThrowsNullReferenceException()
		{
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			Assert.ThrowsExactly<NullReferenceException>(() => service.FillLatestListings(null));
		}

		[TestMethod]
		public void FillCategories_NullResponse_ThrowsNullReferenceException()
		{
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			Assert.ThrowsExactly<NullReferenceException>(() => service.FillCategories(null));
		}

		[TestMethod]
		public void FillLatestQuotes_NullData_ThrowsNullReferenceException()
		{
			var protocolMock = new SLProtocolMock<ConcreteSLProtocolExt>();
			var service = new CoinMarketCapService(protocolMock.Object);

			Assert.ThrowsExactly<NullReferenceException>(() => service.FillLatestQuotes(null));
		}

		[TestMethod]
		[DataRow(null, 9999999999999999d)]
		[DataRow(0d, 0d)]
		[DataRow(42.5, 42.5)]
		[DataRow(-0.5, -0.5)]
		public void Resolve_Double_ReturnsExpected(double? input, double expected)
		{
			var result = DataMinerValue.Resolve(input);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		[DataRow(null, "N/A")]
		[DataRow("", "N/A")]
		[DataRow("BTC", "BTC")]
		public void Resolve_String_ReturnsExpected(string input, string expected)
		{
			var result = DataMinerValue.Resolve(input);
			Assert.AreEqual(expected, result);
		}
	}
}