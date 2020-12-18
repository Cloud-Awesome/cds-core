namespace CloudAwesome.Xrm.Core.Models
{
    public enum CdsConnectionType { AppRegistration, ConnectionString, UserNameAndPassword }

    public class CdsConnection
    {
        public CdsConnectionType ConnectionType { get; set; }

        public string CdsConnectionString { get; set; }

        public string CdsUrl { get; set; }

        public string CdsUserName { get; set; }

        public string CdsPassword { get; set; }

        public string CdsAppId { get; set; }

        public string CdsAppSecret { get; set; }
    }
}
