namespace IonosDnsUpdater.Classes
{
    public class IonosConfig
    {
        public Uri ApiUrl { get; set; }

        public string ApiKey { get; set; }

        public string Domain { get; set; }

        public List<string> SubDomains { get; set; }

    }
}
