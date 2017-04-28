using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using PCLStorage;

namespace DuluthHomegrown2017
{
	/// <summary>
	/// This implementation of IDataSource<Day> is intended for loading the 
	/// schedule from JSON that is stored in the app as an Asset in Android,
	/// and a Resource in iOS.
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
				ex.ReportError("FilesystemOnlyDayDataSource-GetItems");
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
			var json = ServiceLocator.Current.GetInstance<ILocalBundleFileManager>().ReadFileFromBundleAsString(_FileName);

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
