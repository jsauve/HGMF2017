using System;
using DuluthHomegrown2017.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionRetrievalService))]
namespace DuluthHomegrown2017.Droid
{
	public class VersionRetrievalService : IVersionRetrievalService
	{
		public string Version
		{
			get
			{
				var context = Forms.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
			}
		}
	}
}
