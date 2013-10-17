using System;
using System.Collections.Generic;
using Nest;

namespace KibanaTryout
{
	public class ElasticSearchIndexer
	{
		private readonly ElasticSearchConfiguration _configuration;
		private readonly HashSet<string> _indexNames = new HashSet<string>();
		private readonly ElasticClient _client;
		private readonly RawElasticClient _rawClient;

		public ElasticSearchIndexer(ElasticSearchConfiguration configuration)
		{
			_configuration = configuration;
			var clientSettings = new ConnectionSettings(new Uri(string.Format("http://{0}:{1}", configuration.Host, configuration.Port)));
			_rawClient = new RawElasticClient(clientSettings);
			_client = new ElasticClient(clientSettings);
		}

		public void IndexLog(ResultToIndex resultToIndex)
		{
			var indexName = BuildIndexName(resultToIndex.TimeStamp);
			EnsureIndexExists(indexName);

			var indexResult = _rawClient.IndexPut(indexName, resultToIndex.LogType, resultToIndex.Id, resultToIndex.JsonBody);

			if (!indexResult.Success)
			{
				throw new ApplicationException(string.Format("Failed to index: '{0}'. Result: '{1}'.", resultToIndex.JsonBody, indexResult.Result));
			}

		}

		private string BuildIndexName(DateTime timestamp)
		{
			return timestamp.ToString(_configuration.IndexNameFormat);
		}

		private void EnsureIndexExists(string indexName)
		{
			if (_indexNames.Contains(indexName))
				return;

			CreateIndex(indexName);
			_indexNames.Add(indexName);
		}


		private void CreateIndex(string indexName)
		{
			if (_client.IndexExists(indexName).Exists)
				return;

			var indexSettings = new IndexSettings
				{
					{"index.store.compress.stored", true},
					{"index.store.compress.tv", true},
					{"index.query.default_field", ElasticSearchFields.Message}
				};

			IIndicesOperationResponse result = _client.CreateIndex(indexName, indexSettings);

			CreateMappings(indexName);

			if (!result.OK)
			{
				throw new ApplicationException(string.Format("Failed to create index: '{0}'. Result: '{1}'", indexName, result.ConnectionStatus.Result));
			}

		}

		private void CreateMappings(string indexName)
		{
			_client.MapFluent(map => map
				.IndexName(indexName)
				.DisableAllField()
				.TypeName("_default_")
				.TtlField(t => t.SetDisabled(false))
				.SourceField(s => s.SetCompression())
				.Properties(descriptor => descriptor
					.Date(m => m.Name(ElasticSearchFields.Timestamp).Index(NonStringIndexOption.not_analyzed))
					.String(m => m.Name(ElasticSearchFields.Type).Index(FieldIndexOption.not_analyzed))
					.String(m => m.Name("Parameter").Index(FieldIndexOption.not_analyzed))
					.String(m => m.Name(ElasticSearchFields.Message).IndexAnalyzer("whitespace"))
				)
			);
		}
	}

}
