using System;
using HGMF2017.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2017.iOS
{
	public class LyftService : ILyftService
	{
		public void OpenLyft()
		{
			if (LyftIsInstalled)
				UIApplication.SharedApplication.OpenUrl(new NSUrl($"lyft://partner={Settings.LYFT_CLIENT_ID}"));
			else
				UIApplication.SharedApplication.OpenUrl(new NSUrl($"https://www.lyft.com/signup/SDKSIGNUP?clientId={Settings.LYFT_CLIENT_ID}&sdkName=iOS_direct"));
				
		}

		bool LyftIsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("lyft://"));
	}
}
