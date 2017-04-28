using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CarouselView.FormsPlugin.Android;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms.Platform.Android;
using pyze.xamarin.android;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace DuluthHomegrown2017.Droid
{
    [Activity (Label = "HGMF2017", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        // an IoC Container
        IContainer _IoCContainer;

        protected override void OnCreate (Bundle bundle)
        {

#if !DEBUG
            MobileCenter.Start (Settings.MOBILECENTER_ANDROID_APP_ID, typeof (Analytics), typeof (Crashes));
            Pyze.Initialize(this);
#endif

            RegisterDependencies ();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate (bundle);

            Forms.Init (this, bundle);

            CarouselViewRenderer.Init ();

            LoadApplication (new App ());
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            //CrashManager.Register(this, Settings.HOCKEYAPP_ANDROID_APP_ID);
        }

        /// <summary>
        /// Registers dependencies with an IoC container.
        /// </summary>
        /// <remarks>
        /// Since some of our libraries are shared between the Forms and Native versions 
        /// of this app, we're using an IoC/DI framework to provide access across implementations.
        /// </remarks>
        void RegisterDependencies ()
        {
            var builder = new ContainerBuilder ();

            builder.RegisterInstance (_LazyLocalBundleFileManager.Value).As<ILocalBundleFileManager> ();

            builder.RegisterInstance (_LazyAzureFunctionDataSource.Value).As<IDataSource<Day>> ();

            _IoCContainer = builder.Build ();

            var csl = new AutofacServiceLocator (_IoCContainer);
            ServiceLocator.SetLocatorProvider (() => csl);
        }

        // we need lazy loaded instances of these two types hanging around because if the registration on IoC container changes at runtime, we want the same instances
        static Lazy<LocalBundleFileManager> _LazyLocalBundleFileManager = new Lazy<LocalBundleFileManager> (() => new LocalBundleFileManager ());
        static Lazy<FilesystemOnlyDayDataSource> _LazyFilesystemOnlyDayDataSource = new Lazy<FilesystemOnlyDayDataSource> (() => new FilesystemOnlyDayDataSource ());
        static Lazy<AzureFunctionDayDataSource> _LazyAzureFunctionDataSource = new Lazy<AzureFunctionDayDataSource> (() => new AzureFunctionDayDataSource ());
    }
}
