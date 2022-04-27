using System.Net.Http.Headers;

namespace IonosDnsUpdater.Classes
{
    public class IonosDnsWorker
    {
        IonosConfig ionosConfig;
        private readonly HttpClient client;
        public IonosDnsWorker(IonosConfig ionosConfig)
        {
            client = new HttpClient();
            this.ionosConfig = ionosConfig;
        }
        public async Task<List<IonosZone>> GetZones()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("X-API-Key", ionosConfig.ApiKey);

            client.DefaultRequestHeaders.Add("User-Agent", "Ionos Dns Updater");

            var stringTask = client.GetStringAsync(ionosConfig.ApiUrl + "/v1/zones");

            var msg = await stringTask;

            Console.Write(msg);

            return null;

        }
    }
}
