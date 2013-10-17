using System;

namespace KibanaTryout
{
	static class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("[A]ggregate or create [I]ndex?");
			var consoleKey = Console.ReadKey().Key;

			switch (consoleKey)
			{
				case ConsoleKey.A:
				{
					var agg = new Aggregator();
					agg.Blahonga();

					return;
				}
				case ConsoleKey.I:
				{
					var indexer = new ElasticSearchIndexer(new ElasticSearchConfiguration());
					var milliseconds = (int)TimeSpan.FromDays(10).TotalMilliseconds;

					while (true)
					{
						var timeStamp = DateTime.UtcNow.AddMilliseconds(new Random().Next(-milliseconds, milliseconds));
						var eventToLog = EventToIndexCreator.CreateEventToIndex(timeStamp);
						indexer.IndexLog(eventToLog);

						Console.WriteLine(eventToLog.JsonBody);
					}

				}
				default:
					Console.WriteLine("Nah...");
					break;
			}
		}
	}
}
