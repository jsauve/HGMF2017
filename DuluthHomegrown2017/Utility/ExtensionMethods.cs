using System;
using MvvmHelpers;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DuluthHomegrown2017
{
	public static class ListExtensions
	{
		public static int[] FindAllIndicesOf(this IEnumerable<string> values, string val1, string val2)
		{
			var result = values.Select((b, i) => b.Contains(val1) || b.Contains(val2) ? i : -1).Where(i => i != -1).ToArray();
			return result;
		}
	}

	public static class ExceptionExtensions
	{
		public static void ReportToHockeyApp(this Exception ex, string hint)
		{
			var exDict = new Dictionary<string, string>();
			//exDict.Add("Hint", hint);
			//exDict.Add("StackTrace", ex?.StackTrace);
			HockeyApp.MetricsManager.TrackEvent($"HandledException-{hint}");
		}
	}

	public static class EnumerableExtensions
	{
		public static IEnumerable<List<TSource>> Split<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			var list = new List<TSource>();

			foreach (var element in source)
			{
				if (predicate(element))
				{
					if (list.Count > 0)
					{
						yield return list;
						list = new List<TSource>();
					}
				}
				else
				{
					list.Add(element);
				}
			}

			if (list.Count > 0)
			{
				yield return list;
			}
		}
	}

	public static class StringExtensions
	{
		const string ScreenNamePattern = @"@([A-Za-z0-9\-_&;]+)";
		const string HashTagPattern = @"#([A-Za-z0-9\-_&;]+)";
		const string HyperLinkPattern = @"(http://\S+)\s?";

		public static string TransformToTweetHtml(this string text)
		{
			string innerHtml = text;

			if (innerHtml.Contains("http://"))
			{
				var links = new List<string>();
				foreach (Match match in Regex.Matches(innerHtml, HyperLinkPattern))
				{
					var url = match.Groups[1].Value;
					if (!links.Contains(url))
					{
						links.Add(url);
						innerHtml = innerHtml.Replace(url, String.Format("<a href=\"{0}\">{0}</a>", url));
					}
				}
			}

			if (innerHtml.Contains("@"))
			{
				var names = new List<string>();
				foreach (Match match in Regex.Matches(innerHtml, ScreenNamePattern))
				{
					var screenName = match.Groups[1].Value;
					if (!names.Contains(screenName))
					{
						names.Add(screenName);
						innerHtml = innerHtml.Replace("@" + screenName,
						   String.Format("<a href=\"http://twitter.com/{0}\">@{0}</a>", screenName));
					}
				}
			}

			if (innerHtml.Contains("#"))
			{
				var names = new List<string>();
				foreach (Match match in Regex.Matches(innerHtml, HashTagPattern))
				{
					var hashTag = match.Groups[1].Value;
					if (!names.Contains(hashTag))
					{
						names.Add(hashTag);
						innerHtml = innerHtml.Replace("#" + hashTag,
						   String.Format("<a href=\"http://twitter.com/search?q={0}\">#{1}</a>",
							System.Net.WebUtility.UrlEncode("#" + hashTag), hashTag));
					}
				}
			}

			var result =
				"<html><head><style type=\"text/css\">body { font-family: Helvetica; }</style><body>" +
				$"{innerHtml}" +
				"</body></html>";

			return result;
		}

		public static string TransformToTweetFormattedText(this string text)
		{
			if (text.Contains("http://"))
			{
				var links = new List<string>();
				foreach (Match match in Regex.Matches(text, HyperLinkPattern))
				{
					var url = match.Groups[1].Value;
					if (!links.Contains(url))
					{
						links.Add(url);
						text = text.Replace(url, $"<Span ForegroundColor=\"Blue\" FontAttributes=\"Bold\">{url}</Span>");
					}
				}
			}

			if (text.Contains("@"))
			{
				var names = new List<string>();
				foreach (Match match in Regex.Matches(text, ScreenNamePattern))
				{
					var screenName = match.Groups[1].Value;
					if (!names.Contains(screenName))
					{
						names.Add(screenName);
						text = text.Replace("@" + screenName, $"<Span FontAttributes=\"Bold\">@{screenName}</Span>");
					}
				}
			}

			if (text.Contains("#"))
			{
				var names = new List<string>();
				foreach (Match match in Regex.Matches(text, HashTagPattern))
				{
					var hashTag = match.Groups[1].Value;
					if (!names.Contains(hashTag))
					{
						names.Add(hashTag);
						text = text.Replace("#" + hashTag, $"<Span FontAttributes=\"Bold\">#{hashTag}</Span>");
					}
				}
			}

			return $"<FormattedString><Spans>{text}</Spans></FormattedString>";
		}

		public static string ToTweetTimestampFormat(this DateTime createdAt)
		{
			return createdAt.ToLocalTime().ToString("h:mm tt - d MMM yyyy");
		}

		public static string ToTimeSinceFormat(this DateTime createdAt)
		{
			var elapsed = DateTime.UtcNow - createdAt.ToUniversalTime();

			if (elapsed.TotalSeconds < 60)
			{
				if (elapsed.TotalSeconds < 1)
					return "just now";

				if (elapsed.TotalSeconds < 2)
					return $"{(int)elapsed.TotalSeconds} sec ago";

				return $"{(int)elapsed.TotalSeconds} secs ago";
			}


			if (elapsed.TotalMinutes < 60)
			{
				if ((int)elapsed.TotalMinutes < 2)
					return $"{(int)elapsed.TotalMinutes} min ago";

				return $"{(int)elapsed.TotalMinutes} mins ago";
			}

			if (elapsed.TotalHours < 24)
			{
				if ((int)elapsed.TotalHours < 2)
					return $"{(int)elapsed.TotalHours} hr ago";

				return $"{(int)elapsed.TotalHours} hrs ago";
			}


			if (elapsed.TotalDays < 7)
			{
				if (elapsed.TotalDays < 2)
					return $"{(int)elapsed.TotalDays} day ago";

				return $"{(int)elapsed.TotalDays} days ago";
			}

			if (((int)elapsed.TotalDays / 7) < 2)
				return $"{(int)elapsed.TotalDays / 7} wk ago ";

			return $"{(int)elapsed.TotalDays / 7} wks ago"; 
		}
	}
}
