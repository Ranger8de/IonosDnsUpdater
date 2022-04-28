using IonosDnsUpdater.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IonosDnsUpdater.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class IonosDnsUpdateController : ControllerBase
    {

        private readonly Classes.IonosConfig _ionosConfig;
        private readonly ILogger<IonosDnsUpdateController> _logger;
        private readonly IonosDnsWorker ionosDnsWorker;

        public IonosDnsUpdateController(ILogger<IonosDnsUpdateController> logger)
        {
            _logger = logger;
            _ionosConfig = GetIonosConfig();
            ionosDnsWorker = new IonosDnsWorker(_ionosConfig);
        }

        [HttpGet(Name = "SetIonosDnsUpdate")]
        public async Task<bool> SetNewIp(string ipAddress)
        {
            IPAddress ip;
            bool b = IPAddress.TryParse(ipAddress, out ip);
            if (b)
            {
                IonosCustomerZone customerZone = await GetDnsRecordsFromIonos();
                UpdateIpAtIonos(customerZone, ip) ;
            }

            return true;
        }

        private async Task<IonosCustomerZone> GetDnsRecordsFromIonos()
        {
            List<IonosRecord> result = new List<IonosRecord>();
            IonosCustomerZone customerZone = null;

            // get listed domains
            List<IonosZone> zones = await ionosDnsWorker.GetZones();

            // get configurated domain
            if ((zones != null) && (zones.Count > 0))
            {
                var zone = zones.Find(match: zone => zone.Name.Equals(_ionosConfig.Domain, StringComparison.CurrentCultureIgnoreCase));

                // get listed Subdomains
                if (zone != null && zone.Id != null)
                {
                    customerZone = await ionosDnsWorker.GetCustomerZones(zone.Id);
                    if (customerZone != null)
                    {
                        if (customerZone.Records != null && customerZone.Records.Count > 0)
                        {
                            foreach (string domain in _ionosConfig.SubDomains)
                            {
                                List<IonosRecord> records = customerZone.Records.FindAll(rec => rec.Name.Equals(domain, StringComparison.CurrentCultureIgnoreCase));
                                result.AddRange(records);
                            }

                            customerZone.Records.Clear();
                            customerZone.Records.AddRange(result);
                        }
                    }
                }
            }

            return customerZone;
        }

        private void UpdateIpAtIonos(IonosCustomerZone customerZone, IPAddress ip)
        {
            foreach (IonosRecord record in customerZone.Records)
            {
                ionosDnsWorker.UpdateDnsRecord(record, customerZone.Id, ip);
            }
        }


        private Classes.IonosConfig GetIonosConfig()
        {
            var builder = WebApplication.CreateBuilder();
            return builder.Configuration.GetSection("IonosConfig").Get<Classes.IonosConfig>();
        }
    }
}