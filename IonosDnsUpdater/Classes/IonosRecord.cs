using System.Diagnostics;

namespace IonosDnsUpdater.Classes
{
    [DebuggerDisplay("Record: {Name} ({Content})")]
    public class IonosRecord : IonosRecordBase
    {
        /// <summary>
        /// Name of the subdomain
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Name of the domain
        /// </summary>
        public string? RootName { get; set; }

        /// <summary>
        /// dns record type (A = IPv4, AAAA = IPv6)
        /// </summary>
        public string? Type { get; set; }

        

        /// <summary>
        /// last date of change
        /// </summary>
        public DateTime? ChangeDate { get; set; }


        /// <summary>
        /// internal id of the record
        /// </summary>
        public string? Id { get; set; }

        public override string ToString()
        {
            return "Record: {Name} ({Content})";
        }
    }
}
