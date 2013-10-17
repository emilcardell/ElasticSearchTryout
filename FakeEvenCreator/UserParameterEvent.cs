using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace KibanaTryout
{
	public class UserParameterEvent
	{
		public static Dictionary<string, List<string>> userPerTermAndTime = new Dictionary<string, List<string>>();

		public List<ResultToIndex> CreateUserParameterEvent(ResultToIndex input)
		{
			var result = new List<ResultToIndex>();
			foreach (var parameter in input.Parameters)
			{
				var output = new ResultToIndex();
				output.Id = input.TimeStamp.ToString("yyyyMMddhh") + "_" + parameter;

				var document = new JObject();
				var intervalDate = new DateTime(input.TimeStamp.Year, input.TimeStamp.Month, input.TimeStamp.Day, input.TimeStamp.Hour, 0, 0);
				document.Add(ElasticSearchFields.Timestamp, new JValue(intervalDate));

				if (!userPerTermAndTime.ContainsKey(output.Id))
				{
					userPerTermAndTime.Add(output.Id, new List<string>());
				}

				if (!userPerTermAndTime[output.Id].Contains(input.User))
				{
					userPerTermAndTime[output.Id].Add(input.User);
				}

				document.Add("Parameter", new JValue(parameter.Trim()));
				document.Add("NumberOfUniqeUsers", new JValue(userPerTermAndTime[output.Id].Count));
				document.Add(ElasticSearchFields.Type, new JValue("AGGRO"));

				output.LogType = "AGGRO";
				output.TimeStamp = intervalDate;
				output.JsonBody = document.ToString(Newtonsoft.Json.Formatting.None);

				result.Add(output);

			}

			return result;
		}
	}
}
