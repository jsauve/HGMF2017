using System;
using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using pyze.xamarin.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace HGMF2017.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		// an IoC Container
		IContainer _IoCContainer;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if !DEBUG
            MobileCenter.Start(Settings.MOBILECENTER_IOS_APP_ID, typeof(Analytics), typeof(Crashes));
#endif

			RegisterDependencies();

			#if ENABLE_TEST_CLOUD
				Xamarin.Calabash.Start();
			#endif

			Forms.Init();

			CarouselViewRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
#if !DEBUG
			Pyze.Initialize(Settings.PYZE_IOS_API_KEY);
#endif
			return base.WillFinishLaunching(uiApplication, launchOptions);
		}

		/// <summary>
		/// Registers dependencies with an IoC container.
		/// </summary>
		/// <remarks>
		/// Since some of our libraries are shared between the Forms and Native versions 
		/// of this app, we're using an IoC/DI framework to provide access across implementations.
		/// </remarks>
		void RegisterDependencies()
		{
			var builder = new ContainerBuilder();

			builder.RegisterInstance(_LazyLocalBundleFileManager.Value).As<ILocalBundleFileManager>();

			builder.RegisterInstance(_LazyAzureFunctionDataSource.Value).As<IDataSource<Day>>();

			_IoCContainer = builder.Build();

			var csl = new AutofacServiceLocator(_IoCContainer);
			ServiceLocator.SetLocatorProvider(() => csl);   
		}

		// we need lazy loaded instances of these two types hanging around because if the registration on IoC container changes at runtime, we want the same instances
		static Lazy<LocalBundleFileManager> _LazyLocalBundleFileManager = new Lazy<LocalBundleFileManager>(() => new LocalBundleFileManager());
		static Lazy<FilesystemOnlyDayDataSource> _LazyFilesystemOnlyDayDataSource = new Lazy<FilesystemOnlyDayDataSource>(() => new FilesystemOnlyDayDataSource());
        static Lazy<AzureFunctionDayDataSource> _LazyAzureFunctionDataSource = new Lazy<AzureFunctionDayDataSource> (() => new AzureFunctionDayDataSource());
	}
}
