namespace Game.Composition
{
    public readonly struct GameSessionConfig
    {
        public GameSessionConfig(string slotId)
        {
            SlotId = slotId;
        }

        public string SlotId { get; }
    }
}
