using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToTwitter;
using Xamarin.Forms;

namespace HGMF2017
{
	public partial class Tweets : ContentPage
	{
		protected TweetsViewModel ViewModel => BindingContext as TweetsViewModel;

		bool IsPresentingModally;

		public Tweets()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ViewModel.NoNetworkDetected += async (sender, e) => {
				await App.DisplayNoNetworkAlert(this);
				TweetsListView.EndRefresh();
			};

			ViewModel.OnError += async (sender, e) => {
				await App.DisplayErrorAlert(this);
				TweetsListView.EndRefresh();
			};
		}

		protected override async void OnAppearing()
		{
			if (IsPresentingModally)
			{
				IsPresentingModally = false;
				return;
			}

			base.OnAppearing();

			if (ViewModel.IsInitialized)
				return;

			await ViewModel.ExecuteLoadTweetsCommand();
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var tweetWrapper = (TweetWrapper)e.Item;

			if (tweetWrapper.HasUrl)
				try
				{
					var url = tweetWrapper.StatusUrl;

					if (url != null)
						Device.OpenUri(new Uri(url));
				}
				catch
				{
					Task.Factory.StartNew(async () => { await App.DisplayNoNetworkAlert(this); });
					
				}

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}

		// Leaving this here in case I get around to solving why image taps in a listview don't work reliably on Android
		//async void OnImageTap(object sender, EventArgs args)
		//{
		//	IsPresentingModally = true;

		//	var imageSender = (Image)sender;

		//	var imageSource = (UriImageSource)imageSender.Source;

		//	ViewModel.SelectedImagePosition = ViewModel.ImageUrls.Select(x => x.ImageUrl).ToList().IndexOf(imageSource.Uri.ToString());

		//	var tweetDetailPage = new TweetImageDetailPage() { BindingContext = this.ViewModel };

		//	Page detailPage;

		//	if (Device.RuntimePlatform == "iOS")
		//	{
		//		detailPage = new NavigationPage(tweetDetailPage) { BarBackgroundColor = Color.Black };

		//		var toolBarItem = new ToolbarItem("Back", null, () => { Navigation.PopModalAsync(); });

		//		detailPage.ToolbarItems.Add(toolBarItem);
		//	}
		//	else
		//		detailPage = tweetDetailPage;

		//	await Navigation.PushModalAsync(detailPage);
		//}
	}
}
