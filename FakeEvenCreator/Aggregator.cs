using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace KibanaTryout
{
    public class Aggregator
    {
        public void Blahonga()
        {
            var configuration = new ElasticSearchConfiguration();
			var clientSettings = new ConnectionSettings(new Uri(string.Format("http://{0}:{1}", configuration.Host, configuration.Port)));
            clientSettings.SetDefaultIndex("_all");
            var client = new ElasticClient(clientSettings);

            var blahonga = client.Search<SearchEvent>(
                s => s.Query(q =>
                             q.Range(r =>
                                     r.OnField(ElasticSearchFields.Timestamp)
                                      .From(DateTime.UtcNow.Date)
                                      .To(DateTime.UtcNow.AddDays(2).Date))));

            Console.WriteLine(blahonga.Total);

            foreach (var searchEvent in blahonga.Documents)
            {
                Console.WriteLine("User: {0}, TimeStamp: {1}", searchEvent.User, searchEvent.TimeStamp);
            }

            Console.WriteLine("YAY!");
        }

                
    }

    [ElasticType(Name = "SearchEvent")]
    public class SearchEvent
    {
        [JsonProperty(ElasticSearchFields.Timestamp)]
        public DateTime TimeStamp { get; set; }
        public List<string> Parameters { get; set; }
        public string User { get; set; }
    }
}
