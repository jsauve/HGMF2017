using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;

namespace DuluthHomegrown2017
{
	public class ScheduleParser
	{
		//HttpClient _Client;

		//public ScheduleParser()
		//{
		//	_Client = new HttpClient();
		//	_Client.BaseAddress = new Uri("http://www.duluthhomegrown.org");
		//}

		//public async Task<string> FetchScheduleHtml()
		//{
		//	return await _Client.GetStringAsync("schedule");
		//}

		//public List<IElement> FetchScheduleParagraphTags(string html)
		//{
		//	var parser = new HtmlParser();
		//	var document = parser.Parse(html);

		//	var scheduleRootElement = document.All.SingleOrDefault(m => m.LocalName == "div" && m.ClassList.Contains("post-content"));

		//	var ps = scheduleRootElement.Children.Where(m => m.LocalName == "p" && !String.IsNullOrWhiteSpace(m.TextContent)).ToList();

		//	ps.RemoveAt(0);

		//	return ps;
		//}

		//public async Task<List<ScheduleItem>> FetchScheduleItems()
		//{
		//	// fetch schedule html
		//	var html = await FetchScheduleHtml();

		//	// fetch schedule p tags
		//	var pTags = FetchScheduleParagraphTags(html);

		//	// split list of p tags into separate lists by day
		//	//var groupedPTags = GroupPTagsByDays(pTags);

		//	// parse each day into DayVenues and PerformerSlots

		//	throw new Exception("");
		//}

		//public List<List<string>> GetListOfListsRepresentingDays(List<IElement> pNodes)
		//{
		//	var values = new List<string>();

		//	foreach (var p in pNodes)
		//	{
		//		values.Add(p.TextContent);
		//	}

		//	var dateNodeIndicies = values.FindAllIndicesOf(", April", ", May");

		//	List<string> dates = new List<string>();

		//	foreach (var x in dateNodeIndicies)
		//	{
		//		dates.Add(values[x]);
		//	}

		//	var dateNodeIndexPairs = new List<Tuple<int, int>>();

		//	int i = 0;

		//	foreach (var y in dateNodeIndicies)
		//	{
		//		dateNodeIndexPairs.Add(new Tuple<int, int>(i, y));
		//		i++;
		//	}

		//	var days = values.Split(x => x.Contains(", April") || x.Contains(", May")).ToList();

		//	for (int j = 0; j < dates.Count; j++)
		//	{
		//		days[j].Insert(0, dates[j]);
		//	}

		//	return days;
		//}

		//public List<List<string>> GetListOfListsRepresentingVenues(List<List<string>> days)
		//{
		//	var values = new List<string>();

		//	List<int> venueTitleIndicies = new List<int>();

		//	foreach (var d in days)
		//	{
		//		foreach (var v in d)
		//		{
		//			int j;
		//			if (!int.TryParse(v.Substring(0, 1), out j))
		//			{
		//				venueTitleIndicies.Add(
		//			}
		//		}
		//	}

		//	var dateNodeIndicies = values.FindAllIndicesOf(", April", ", May");

		//	foreach (var d in days)
		//	{

		//	}
		//}


		//public List<List<string>> GetListOfListsRepresentingVenues(List<List<string>> days)
		//{


		//	var scheduleItems = new List<ScheduleItem>();

		//	foreach (var d in days)
		//	{
		//		var newSI = new ScheduleItem() { Date = d[0] };




		//		scheduleItems.Add(new ScheduleItem() { Date = d[0] });
		//	}


		//}


		//public List<List<string>> GetListOfListsRepresentingVenues()
		//{

		//}

		//public List<DayVenueItem> GetDayVenueItems(List<string> day)
		//{
		//	var dayVenuItems = new List<DayVenueItem>();

		//	foreach (var line in day)
		//	{
		//		int j;
		//		if (int.TryParse(line.Substring(0, 1), out j))
		//		{
		//			var newdayVenueItem = new DayVenueItem() { VenueTitle = line };

		//			while (!int.TryParse(line.Substring(0, 1), out j))
		//			{

		//			}
		//		}
		//	}
		//}


	}
}
