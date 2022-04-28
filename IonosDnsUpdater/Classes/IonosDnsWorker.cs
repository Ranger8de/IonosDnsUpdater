using System.Net;
using System.Net.Http.Headers;

namespace IonosDnsUpdater.Classes
{
    public class IonosDnsWorker
    {
        IonosConfig ionosConfig;
        public IonosDnsWorker(IonosConfig ionosConfig)
        {
            this.ionosConfig = ionosConfig; 
        }

        private HttpClient InitializeHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Key", ionosConfig.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "Ionos Dns Updater");

            return client;
        }

        /// <summary>
        /// get the ionos zones = get the registered domains
        /// </summary>
        /// <returns></returns>
        public async Task<List<IonosZone>> GetZones()
        {
            List<IonosZone> result = new List<IonosZone>();

            try
            {
                HttpClient client = InitializeHttpClient();
                string url = string.Format("{0}/v1/zones", ionosConfig.ApiUrl);
                var stringTask = client.GetStringAsync(url);
                var msg = await stringTask;

                result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IonosZone>>(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;

        }


        /// <summary>
        /// get the ionos cutomerZones =  get the registered subdomains
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<IonosCustomerZone> GetCustomerZones(string domainId)
        {
            IonosCustomerZone result = null;

            try
            {
                HttpClient client = InitializeHttpClient();
                string url = string.Format("{0}/v1/zones/{1}?suffix={2}&recordType=A", ionosConfig.ApiUrl, domainId, ionosConfig.Domain);
                var stringTask = client.GetStringAsync(url);
                var msg = await stringTask;

                result = Newtonsoft.Json.JsonConvert.DeserializeObject<IonosCustomerZone>(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async void UpdateDnsRecord(IonosRecord record, string customerZoneId, IPAddress ip)
        {
            try
            {
                HttpClient client = InitializeHttpClient();
                string url = string.Format("{0}/v1/zones/{1}/records/{2}", ionosConfig.ApiUrl, customerZoneId, record.Id);
                var response = await client.PutAsJsonAsync(url, new IonosRecordUpdate() { Content = ip.ToString(), Disabled = false, Prio = 0, Ttl = 3600});

                if (response != null )
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
