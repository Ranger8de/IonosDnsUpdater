namespace IonosDnsUpdater.Classes
{
    public class IonosRecordBase
    {
        public bool? Disabled { get; set; }

        /// <summary>
        /// ip address
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// time to live
        /// </summary>
        public int? Ttl { get; set; }
    }
}
