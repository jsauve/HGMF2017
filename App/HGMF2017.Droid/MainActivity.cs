using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Xamarin.Forms.Platform.Android;
using pyze.xamarin.android;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using FFImageLoading.Forms.Droid;

namespace HGMF2017.Droid
{
    [Activity (Label = "HGMF2017", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate (Bundle bundle)
        {

#if !DEBUG
            MobileCenter.Start (Settings.MOBILECENTER_ANDROID_APP_ID, typeof (Analytics), typeof (Crashes));
            Pyze.Initialize(this);
#endif

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate (bundle);

            Forms.Init (this, bundle);

			CachedImageRenderer.Init();

            CarouselViewRenderer.Init ();

            LoadApplication (new App ());
        }

        protected override void OnResume()
        {
            base.OnResume();

			//TODO: Put version-checking code here
		}
    }
}
