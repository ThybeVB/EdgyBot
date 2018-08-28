namespace EdgyCore.Models 
{
    public class StatsModel 
    {
        public Shard[] shards { get; set; }
    }

    public class Shard
    {
        public string name { get; set; }
        public int server_count { get; set; }
        public int user_count { get; set; }
    }
}