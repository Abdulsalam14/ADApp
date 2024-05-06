using System;
using Microsoft.Azure.Documents;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace ADFunction
{
    public class Function1
    {

        [FunctionName("Function1")]
        public async Task Run([QueueTrigger("movienames", Connection = "AzureConStr")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.omdbapi.com/?apikey=286b2a27&t={myQueueItem}");
            request.Content = new StringContent("application/json");
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            JObject obj = JObject.Parse(await response.Content.ReadAsStringAsync());
            var poster = obj["Poster"].ToString();


            log.LogInformation($"{poster}");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-13715.c282.east-us-mz.azure.redns.redis-cloud.com:13715,password=vLdemVDtIuO72UqGs9eY6mP0wrJBG59Y");
            IDatabase db = redis.GetDatabase();
            db.ListRightPush("movies", poster);

        }
    }
}
