using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KibanaTryout
{
	public class EventToIndexCreator
	{
		public static ResultToIndex CreateEventToIndex()
		{
			var document = new JObject();
			var id = Guid.NewGuid().ToString();
			var timeStamp = DateTime.UtcNow;
			document.Add(ElasticSearchFields.Id, new JValue(id));
			document.Add(ElasticSearchFields.Timestamp, new JValue(timeStamp));
            document.Add(ElasticSearchFields.Type, new JValue("SearchEvent"));
			document.Add(ElasticSearchFields.Message, new JValue("Event with id: " + id));

			var rnd = new Random();

			var parameters = AvalableParameters.Where(p => rnd.Next(0, 2) == 1).Take(rnd.Next(0, 10));
			var result = string.Empty;
			while (string.IsNullOrWhiteSpace(result))
			{
				result = AvalableResult.FirstOrDefault(r => rnd.Next(0, 2) == 1);
			}

			var user = string.Empty;
			while(string.IsNullOrWhiteSpace(user))
			{
				user = AvalableUsers.FirstOrDefault(r => rnd.Next(0, 2) == 1);
			}

			var parameterArray = new JArray();
			parameters.ToList().ForEach(p => parameterArray.Add(new JValue(p)));
			var resultToIndex = new ResultToIndex()
			{
				Id = id,
				TimeStamp = timeStamp,
                LogType = "SearchEvent",
				Parameters = parameters.ToList(),
				User = user
			};

			document.Add("Parameters", parameterArray);
			document.Add("NumberOfHits", new JValue(rnd.Next(0, 1200)));
			document.Add("RequestTime", new JArray(rnd.Next(100, 5000)));
			document.Add("Result", new JArray(result));
			document.Add("User", new JValue(user));

			resultToIndex.JsonBody = document.ToString(Newtonsoft.Json.Formatting.None);
			
			

			return resultToIndex;
		}

		public static List<string> AvalableUsers = new List<string>() { "Brandy"," Heather"," Channing"," Brianna"," Amber"," Serena"," Melody"," Dakota"," Sierra"," Bambi"," Crystal"," Samantha"," Autumn" };
		public static List<string> AvalableParameters = new List<string>() { "Ostriches", " Emus", " Kiwis", " Penguins", " Albatrosses", " Grebes", " Flamingos", " Storks", " Herons", " Pelicans", " Cranes", " Sandgrouses", " Pigeons", " Doves", " Parrots", " Cuckoos", " Turacos", " Owls", " Nightjars", " Frogmouths", " Swifts", " Hummingbirds", " Mousebirds", " Trogons", " Woodpeckers", " Toucans" };
		public static List<string> AvalableResult = new List<string>() { "Mason wasp nest", "Mealybug", "Mealybug Ladybird", "Mercury Island tusked weta", "Metallic green rove beetle", "Millipedes", "Mite damage", "Mites", "Mole Cricket", "Monarch butterfly", "North Island Lichen Moth", "Nurseryweb spider" };
	}
}