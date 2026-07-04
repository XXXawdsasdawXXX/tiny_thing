namespace Game.Persistence.Settings
{
    public sealed class SettingsData
    {
        public const int CurrentVersion = 1;

        public int Version { get; set; } = CurrentVersion;

        public float MusicVolume { get; set; } = 0.7f;

        public float SfxVolume { get; set; } = 0.7f;

        public string LocaleCode { get; set; } = "en";

        public static SettingsData CreateDefault()
        {
            return new SettingsData();
        }
    }
}
