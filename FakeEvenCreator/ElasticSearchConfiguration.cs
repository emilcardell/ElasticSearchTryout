namespace KibanaTryout
{
	public class ElasticSearchConfiguration
	{
		public ElasticSearchConfiguration()
		{
			Host = "localhost";
			Port = 9200;
			IndexNameFormat = @"\k\i\b\a\n\a\-yyyyMM";
			AggreagateIndexNameFormat = @"\a\g\g\r\o\-yyyyMM";
			ConnectionLimit = 5;
		}

		public string Host { get; set; }
		public int Port { get; set; }
		public string Ttl { get; set; }
		public int ConnectionLimit { get; set; }
		public string IndexNameFormat { get; set; }
		public string AggreagateIndexNameFormat { get; set; }
	}
}