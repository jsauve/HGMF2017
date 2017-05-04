using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Linq;
using Xamarin.Forms;

namespace HGMF2017
{
	public partial class Main : Xamarin.Forms.TabbedPage
	{
		public Main()
		{
			InitializeComponent();

			// Disables the swipping between tabs functionality in Android. Necessary because the Schedule tab's content is swipable as well.
			On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);

			// setup Schedule tab
			var scheduleNavPage = new NavigationPage(new ScheduleMaster() { BindingContext = new ScheduleMasterViewModel() }) { Title = "Schedule" };
			var lyftToolBarItem = new ToolbarItem("Lyft", "LyftToolbar", () => {
				DependencyService.Get<ILyftService>().OpenLyft();
			});
			var uberToolBarItem = new ToolbarItem("Uber", "UberToolbar", () => {
				DependencyService.Get<IUberService>().OpenUber();			
			});
			scheduleNavPage.ToolbarItems.Add(lyftToolBarItem);
			scheduleNavPage.ToolbarItems.Add(uberToolBarItem);
			if (Device.RuntimePlatform == "iOS")
				scheduleNavPage.Icon = "Calendar";


			// setup Tweets tab
			var tweetsViewModel = new TweetsViewModel();
			var tweetsPage = new Tweets() { BindingContext = tweetsViewModel };
			NavigationPage tweetsNavPage = new NavigationPage(tweetsPage) { Title = "Tweets" };
			var photosToolBarItem = new ToolbarItem("Photos", "PhotosToolbar", async () => {
				if (tweetsViewModel.Tweets.Count(x => x.HasImage) < 1)
				{
					await App.DisplayNoPhotosAlert(this);
				}
				else
				{
					var tweetDetailPage = new TweetImageDetailPage() { BindingContext = tweetsViewModel };
					var tweetDetailNavPage = new NavigationPage(tweetDetailPage) { BarBackgroundColor = Color.Black };
					var backToolBarItem = new ToolbarItem("Back", null, async () => { await Navigation.PopModalAsync(); });
					tweetDetailNavPage.ToolbarItems.Add(backToolBarItem);
					await Navigation.PushModalAsync(tweetDetailNavPage);
				}
			});
			tweetsNavPage.ToolbarItems.Add(photosToolBarItem);
			if (Device.RuntimePlatform == "iOS")
				tweetsNavPage.Icon = "Twitter";

			// setup More tab
			var moreNavPage = new NavigationPage(new MoreMaster()) { Title = "More" };
			if (Device.RuntimePlatform == "iOS")
				moreNavPage.Icon = "More";

			Children.Add(scheduleNavPage);
			Children.Add(tweetsNavPage);
			Children.Add(moreNavPage);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}
	}
}
