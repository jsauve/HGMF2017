using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HGMF2017;
using Newtonsoft.Json;
using Xamarin.Forms;

// If you want to use this local filesystem data source, uncomment the following line, and then comment the corresponding line in AzureFunctionDayDataSource.cs
//[assembly: Dependency(typeof(FilesystemOnlyDayDataSource))] 
namespace HGMF2017
{
	/// <summary>
	/// This implementation of IDataSource<Day> is intended for loading the 
	/// schedule from JSON that is stored in the app as an Asset in Android,
	/// and a Resource in iOS.
	/// 
	/// *** NOT ACTIVELY USED IN THE PRODUCTION VERSIONS OF HGMF2017 ***
	/// 
	/// </summary>
	public class FilesystemOnlyDayDataSource : IDataSource<Day>
	{
		const string _FileName = "Schedule.json";

		bool _IsInitialized;

		List<Day> _Days;

		#region IDataSource implementation

		public async Task<IEnumerable<Day>> GetItems()
		{
			var result = new List<Day>();

			try
			{
				await EnsureInitialized().ConfigureAwait(false);

				result = await Task.FromResult(_Days).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				ex.ReportError();
				RaiseOnErrorEvent();
			}

			return result;
		}

		#endregion

		#region supporting methods

		async Task Initialize()
		{
			_Days = DeserializeSchedule();

			_IsInitialized = true;
		}

		async Task EnsureInitialized()
		{
			if (!_IsInitialized)
				await Initialize().ConfigureAwait(false);
		}

		List<Day> DeserializeSchedule()
		{
			var json = DependencyService.Get<ILocalBundleFileService>().ReadFileFromBundleAsString(_FileName);

			var days = JsonConvert.DeserializeObject<List<Day>>(json);

			return days;
		}

		#endregion

		event EventHandler OnErrorEvent;

		object objectLock = new Object();

		/// <summary>
		/// Explicit implementation of events is necessary for events from interfaces
		/// </summary>
		event EventHandler IDataSource<Day>.OnError
		{
			add
			{
				lock (objectLock)
				{
					OnErrorEvent += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					OnErrorEvent -= value;
				}
			}
		}

		protected virtual void RaiseOnErrorEvent()
		{
			EventHandler handler = OnErrorEvent;

			if (handler != null)
				handler(this, new EventArgs());
		}
	}
}
