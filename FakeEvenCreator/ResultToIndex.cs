using System;
using System.Collections.Generic;

namespace KibanaTryout
{
	public class ResultToIndex
	{
		public string Id { get; set; }
		public string JsonBody { get; set; }
		public string LogType { get; set; }
		public DateTime TimeStamp { get; set; }
		public string User { get; set; }
		public List<string> Parameters { get; set; }
	}
}