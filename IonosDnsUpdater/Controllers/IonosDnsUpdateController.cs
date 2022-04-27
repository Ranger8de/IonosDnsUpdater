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

        public IonosDnsUpdateController(ILogger<IonosDnsUpdateController> logger)
        {
            _logger = logger;
            _ionosConfig = GetIonosConfig();
        }

        [HttpGet(Name = "GetIonosDnsUpdate")]
        public IEnumerable<IonosDnsUpdate> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new IonosDnsUpdate
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55)
            })
            .ToArray();
        }

        [HttpGet(Name = "SetIonosDnsUpdate")]
        public IonosDnsUpdate SetNewIp(string ipAddress)
        {
            IPAddress ip;
            bool b = IPAddress.TryParse(ipAddress, out ip);
            if (b)
            {
                UpdateIpAtIonos(ip);
                
            }
            return new IonosDnsUpdate() { 
            Date = DateTime.Now 
            , TemperatureC = Random.Shared.Next(-20, 55)
            };
        }

        private void UpdateIpAtIonos(IPAddress? ip)
        {
            IonosDnsWorker worker = new IonosDnsWorker(_ionosConfig);

            worker.GetZones();
        }

        private Classes.IonosConfig GetIonosConfig()
        {
            var builder = WebApplication.CreateBuilder();
            return builder.Configuration.GetSection("IonosConfig").Get<Classes.IonosConfig>();
        }
    }
}