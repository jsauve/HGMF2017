using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using MvvmHelpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace HGMF2017
{
	public class TweetsViewModel : BaseNavigationViewModel
	{
		HttpClient _HttpClient = new HttpClient();

		string _TwitterSearchQuery = "#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets";

		public event EventHandler NoNetworkDetected;

		protected virtual void RaiseNoNetworkDetectedEvent()
		{
			EventHandler handler = NoNetworkDetected;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public event EventHandler OnError;

		protected virtual void RaiseOnErrorEvent()
		{
			EventHandler handler = OnError;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public bool IsInitialized { get { return Tweets.Count > 0; } }

		ObservableRangeCollection<TweetWrapper> _Tweets;
		public ObservableRangeCollection<TweetWrapper> Tweets
		{
			get { return _Tweets ?? (_Tweets = new ObservableRangeCollection<TweetWrapper>()); }
			set
			{
				_Tweets = value;
				OnPropertyChanged("Tweets");
			}
		}

		ObservableRangeCollection<TweetImageWrapper> _ImageUrls;
		public ObservableRangeCollection<TweetImageWrapper> ImageUrls
		{
			get { return _ImageUrls ?? (_ImageUrls = new ObservableRangeCollection<TweetImageWrapper>()); }
			set
			{
				_ImageUrls = value;
				OnPropertyChanged("ImageUrls");
			}
		}

		Command _LoadTweetsCommand;
		public Command LoadTweetsCommand
		{
			get { return _LoadTweetsCommand ?? (_LoadTweetsCommand = new Command(async () => await ExecuteLoadTweetsCommand())); }
		}

		public async Task ExecuteLoadTweetsCommand()
		{
			LoadTweetsCommand.ChangeCanExecute();

			await FetchTweets();

			LoadTweetsCommand.ChangeCanExecute();
		}

		Command _RefreshTweetsCommand;
		public Command RefreshTweetsCommand
		{
			get { return _RefreshTweetsCommand ?? (_RefreshTweetsCommand = new Command(async () => await ExecuteRefreshTweetsCommand())); }
		}

		async Task ExecuteRefreshTweetsCommand()
		{
			RefreshTweetsCommand.ChangeCanExecute();

			await FetchTweets();

			RefreshTweetsCommand.ChangeCanExecute();
		}

		async Task FetchTweets()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				RaiseNoNetworkDetectedEvent();
				return;
			}

			if (IsBusy)
				return;

			IsBusy = true;

			//try
			//{
				var statuses = new List<Status>();

				// only grab the twitter search query once per instantiation of the view model, otherwise the web service will get hit too often
				//if (string.IsNullOrWhiteSpace(_TwitterSearchQuery))
				//{
				//	// the query string coming from the web service looks similar to this: "#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets"
				//	_TwitterSearchQuery = await _HttpClient.GetStringAsync($"https://duluthhomegrown2017.azurewebsites.net/api/TwitterSearchQueryProvider?code={Settings.AZURE_FUNCTION_TWITTERSEARCHQUERY_API_KEY}");
				//	//_TwitterSearchQuery = await _HttpClient.GetStringAsync($"https://duluthhomegrown2017.azurewebsites.net/api/TwitterSearchQueryProvider?code=blah");
				//}

				statuses.AddRange(await SearchTweets(_TwitterSearchQuery));

				if (statuses.Count > 0)
				{
					Tweets = new ObservableRangeCollection<TweetWrapper>();

					ImageUrls = new ObservableRangeCollection<TweetImageWrapper>();

					foreach (var s in statuses)
					{
						var statusUrl = $"https://twitter.com/{s.User.ScreenNameResponse}/status/{s.StatusID}";

						var imageUrl = (string)null;

						imageUrl = s?.Entities?.MediaEntities?.FirstOrDefault(x => x.Type == "photo")?.MediaUrl;

						if (!string.IsNullOrWhiteSpace(imageUrl))
							ImageUrls.Add(new TweetImageWrapper(imageUrl, s?.Text, s?.User?.ScreenNameResponse, s?.Entities?.HashTagEntities?.Select(x => x.Tag)?.ToList()));

						Tweets.Add(new TweetWrapper(s, imageUrl));
					}
				}
			//}
			//catch (Exception ex)
			//{
			//	ex.ReportError();
			//	RaiseOnErrorEvent();
			//}
			//finally
			//{
			//	IsBusy = false;
			//}
		}

		async Task<List<Status>> SearchTweets(string query)
		{
			var result = new List<Status>();

			var auth = new ApplicationOnlyAuthorizer
			{
				CredentialStore = new SingleUserInMemoryCredentialStore()
				{
					ConsumerKey = Settings.TWITTER_API_CONSUMER_KEY,
					ConsumerSecret = Settings.TWITTER_API_CONSUMER_SECRET
				}
			};

			await auth.AuthorizeAsync();

			var twitterContext = new TwitterContext(auth);

			var tweets = await GetTweets(twitterContext, query);

			return tweets;
		}

		async Task<List<Status>> GetTweets(TwitterContext twitterContext, string query, List<Status> results = null, ulong? lowestId = null)
		{
			int count = 100;

			if (results == null)
				results = new List<Status>();

			if (!lowestId.HasValue)
			{
				var firstresults = (await
					(from search in twitterContext.Search
					 where
					 search.Type == SearchType.Search &&
					 search.Count == count &&
				     search.ResultType == ResultType.Mixed &&
					 search.IncludeEntities == true &&
					 search.Query == query
					 select search)
					 .SingleOrDefaultAsync())?.Statuses;

				results.AddRange(firstresults);

				if (firstresults.Count < count)
				{
					return results;
				}
				else
				{
					var newLowestId = results.Min(x => x.StatusID);

					return await GetTweets(twitterContext, query, firstresults, newLowestId);
				}
			}
			else
			{
				var subsequentResults = (await	
					(from search in twitterContext.Search
					 where
					 search.Type == SearchType.Search &&
				     search.Count == count &&
				     search.ResultType == ResultType.Mixed &&
					 search.IncludeEntities == true &&
					 search.Query == query &&
				     search.MaxID == lowestId.Value - 1
					 select search)
					 .SingleOrDefaultAsync())?.Statuses;

				results.AddRange(subsequentResults);

				if (subsequentResults.Count < count)
				{
					return results;
				}
				else
				{
					var newLowestId = results.Min(x => x.StatusID);

					return await GetTweets(twitterContext, query, results, newLowestId);
				}
			}
		}
	}
}
