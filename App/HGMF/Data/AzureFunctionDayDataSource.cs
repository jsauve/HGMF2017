using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HGMF2017;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureFunctionDayDataSource))]
namespace HGMF2017
{
	public class AzureFunctionDayDataSource : IDataSource<Day>
	{
		HttpClient _HttpClient => new HttpClient();

		public AzureFunctionDayDataSource()
		{
			_HttpClient.DefaultRequestHeaders.Accept.Clear();
			_HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<IEnumerable<Day>> GetItems()
		{
			HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"https://duluthhomegrown2017.azurewebsites.net/api/Schedule?code={Settings.AZURE_FUNCTION_SCHEDULE_API_KEY}");
			return JsonConvert.DeserializeObject<List<Day>>(await _HttpClient.GetStringAsync(req.RequestUri));

			return new List<Day>();
		}
	}
}
