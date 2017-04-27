using System;
using Newtonsoft.Json;
using MvvmHelpers;

namespace DuluthHomegrown2017
{
	public class Performance : ObservableObject
	{
		string _Name;
		public string Name
		{
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		DateTime _Time;
		public DateTime Time
		{
			get { return _Time; }
			set { SetProperty(ref _Time, value); }
		}

		string _VenueName;
		[JsonIgnore]
		public string VenueName
		{
			get { return _VenueName; }
			set { SetProperty(ref _VenueName, value); }
		}
	}

}
