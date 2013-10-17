using System;
using System.Collections.Generic;

namespace KibanaTryout
{
	public class AggressiveModel
	{
		public DateTime TimeStamp { get; set; }
		public string Parameter { get; set; }
		public HashSet<string> UniqueUsers { get; set; }  
	}
}