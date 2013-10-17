
using System;
using System.Collections.Generic;
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

                var eventToLog = EventToIndexCreator.CreateEventToIndex();
                indexer.IndexLog(eventToLog);

                Console.WriteLine(eventToLog.JsonBody);
                //Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(1, 5)));

                //Start Datum - ID/paging - Poistion
                //Hämta ett dygns data (med paging)

                //Spara i dictionary - skapa nyckel



                //När allt är klart spara till elasticserch


            }
        }
    }

    public class AggressiveModel
    {
        public DateTime TimeStamp { get; set; }
        public string Parameter { get; set; }
        public int NumberOfUniqueUsers { get; set; }
        //public List<string> UniqueUsers { get; set; }  
    }
}
