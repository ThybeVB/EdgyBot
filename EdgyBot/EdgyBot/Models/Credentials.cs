namespace EdgyCore.Models
{
    public class Credentials
    {
        public string token { get; set; }
        public string dblToken { get; internal set; }
        public string dbToken { get; internal set; }
        public string bfdToken { get; internal set; }
        public string blsToken { get; internal set; }
        public string listcordToken { get; internal set; }
        public string GJP { get; internal set; }

        public string prefix { get; internal set; }
        public ulong clientID { get; internal set; }
        public string invLink { get; internal set; }
        public string accID { get; internal set; }
        public ulong ownerID { get; internal set; }
    }
}