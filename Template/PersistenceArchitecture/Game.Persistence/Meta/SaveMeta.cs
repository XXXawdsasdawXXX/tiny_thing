namespace Game.Persistence.Meta
{
    public sealed class SaveMeta
    {
        public const int CurrentVersion = 1;

        public const string DefaultSlotId = "default";

        public int Version { get; set; } = CurrentVersion;

        public string LastSlotId { get; set; } = DefaultSlotId;

        public static SaveMeta CreateDefault()
        {
            return new SaveMeta();
        }
    }
}
