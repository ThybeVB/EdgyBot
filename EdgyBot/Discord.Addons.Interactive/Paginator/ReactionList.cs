namespace Discord.Addons.Interactive
{
    /// <summary>
    /// The reaction list.
    /// </summary>
    public class ReactionList
    {
        public bool First { get; set; } = false;
        public bool Last { get; set; } = false;
        public bool Forward { get; set; } = true;
        public bool Backward { get; set; } = true;
        public bool Jump { get; set; } = true;
        public bool Trash { get; set; } = true;
        public bool Info { get; set; } = false;
    }
}