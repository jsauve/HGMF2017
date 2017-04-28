using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DuluthHomegrown2017
{
	public class AzureFunctionDayDataSource : IDataSource<Day>
	{
		HttpClient _HttpClient => new HttpClient();

		string AureFunctionKey = Settings.AZURE_FUNCTION_SCHEDULE_API_KEY;

		public AzureFunctionDayDataSource()
		{
			_HttpClient.DefaultRequestHeaders.Accept.Clear();
			_HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<IEnumerable<Day>> GetItems()
		{
			try
			{
				HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://duluthhomegrown2017.azurewebsites.net/api/Schedule?code={AureFunctionKey}");
				return JsonConvert.DeserializeObject<List<Day>>(await _HttpClient.GetStringAsync(req.RequestUri));
			}
			catch (Exception ex)
			{
				ex.ReportError("AzureFunctionDataSource-GetItems");

				RaiseOnErrorEvent();
			}

			return new List<Day>();
		}

		event EventHandler OnErrorEvent;

		object objectLock = new object();

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
