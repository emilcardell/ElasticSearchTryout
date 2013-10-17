using System;
using System.Collections.Generic;
using System.Linq;
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

			//Start Datum - ID/paging - Poistion

			var dateToAggregate = DateTime.UtcNow.Date;
			var currentSkip = 0;
			var currentTake = 1000;
			var currentTotal = 0;

			//Hämta ett dygns data (med paging)
			var parametersAndUsers = new Dictionary<string, HashSet<string>>();

			while (true)
			{
				var skip = currentSkip;
				var take = currentTake;

				var blahonga = client.Search<SearchEvent>(
					s => s.Skip(skip).Take(take).Query(q =>
								 q.Range(r =>
										 r.OnField(ElasticSearchFields.Timestamp)
										  .From(dateToAggregate)
										  .To(dateToAggregate.AddDays(1))
										  .ToExclusive()
									 )));

				foreach (var searchEvent in blahonga.Documents)
				{
					currentTotal++;

					foreach (var parameter in searchEvent.Parameters)
					{
						if (parametersAndUsers.ContainsKey(parameter))
						{
							var users = parametersAndUsers[parameter];

							if (!users.Contains(searchEvent.User))
							{
								users.Add(searchEvent.User);
							}
						}
						else
						{
							parametersAndUsers.Add(parameter, new HashSet<string>(new[] { searchEvent.User }));
						}
					}
				}

				currentSkip += currentTake;
				var diff = (blahonga.Total - currentTotal);

				if (currentTake > diff)
				{
					currentTake = diff;
				}

				if (blahonga.Total <= currentTotal)
				{
					break;
				}
			}

			//Spara i dictionary - skapa nyckel
			foreach (var kvp in parametersAndUsers)
			{
				Console.WriteLine("Parameter: {0}, NumberOfUsers: {1}", kvp.Key, kvp.Value.Count());
			}

			//När allt är klart spara till elasticserch

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