using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace SimulatorOfValues
{
    public class ApiClient : IDisposable
    {
        private HttpClient httpClient;
        public ApiClient()
        {
            httpClient = new HttpClient();
        }
        public async Task<bool> SendMessage(object message, string baseurl, string authToken)
        {
            try
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings() { Culture = new CultureInfo("cs-CZ") };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(message, Formatting.Indented, settings);
                Console.WriteLine("\n-------------------------------------------");
                Console.WriteLine("message:");
                Console.WriteLine(json);

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

                var response = await httpClient.PostAsync($@"{baseurl}", data);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Message sent successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            return false;
        }
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}