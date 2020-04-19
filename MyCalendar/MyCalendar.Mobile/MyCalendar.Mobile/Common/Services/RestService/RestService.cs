using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyCalendar.Mobile.Common.Services.RestService
{
    public class RestService : IRestService
    {
        public async Task<object> PostAsync(string requestUri, string serializedObject = null)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var message = new HttpRequestMessage(HttpMethod.Post, requestUri);

                if (!String.IsNullOrEmpty(serializedObject))
                    message.Content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("access_token"));

                var response = await client.SendAsync(message);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
