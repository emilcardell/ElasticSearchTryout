
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
                var eventToLog = EventToIndexCreator.CreateEventToIndex();
                indexer.IndexLog(eventToLog);

                //var moreEventToIndex = new UserParameterEvent().CreateUserParameterEvent(eventToLog);
                //moreEventToIndex.ForEach(indexer.IndexLog);

                Console.WriteLine(eventToLog.JsonBody);
                Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 5)));
            }
        }
    }
}
