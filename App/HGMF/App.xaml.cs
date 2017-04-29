using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HGMF2017
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new Main();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public static async Task DisplayNoNetworkAlert(Page page)
		{
			await page.DisplayAlert("No Internet!", "Please check your device's internet connection and try again.", "OK");
		}

		public static async Task DisplayErrorAlert(Page page)
		{
			await page.DisplayAlert("Oh noezzzz!", "An error occurred, but it's totally not your fault. If you continue to see this, please report it to joe@joesauve.com. Carry on!", "OK");
		}

		public static async Task DisplayNoPhotosAlert(Page page)
		{
			await page.DisplayAlert("No Photos!", "It looks like there's no photos in the Twitter feed right now. Check back later!", "OK");
		}
	}
}
