using System;
using System.Threading;

namespace KibanaTryout
{
	class Program
	{
		static void Main(string[] args)
		{
			var indexer = new ElasticSearchIndexer(new ElasticSearchConfiguration());

			while (true)
			{
				var agg = new Aggregator();
				agg.Blahonga();

				return;

				var timeStamp = DateTime.UtcNow.AddDays(new Random().Next(-10, 10));

				var eventToLog = EventToIndexCreator.CreateEventToIndex(timeStamp);
				indexer.IndexLog(eventToLog);

				Console.WriteLine(eventToLog.JsonBody);
				//Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 5)));
			}
		}
	}
}
