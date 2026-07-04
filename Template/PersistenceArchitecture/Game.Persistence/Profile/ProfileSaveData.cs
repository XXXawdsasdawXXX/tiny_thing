using System;
using System.Collections.Generic;

namespace Game.Persistence.Profile
{
    public sealed class ProfileSaveData
    {
        public const int CurrentVersion = 1;

        public int Version { get; set; } = CurrentVersion;

        public string SlotLabel { get; set; } = "Save";

        public List<HeroSaveData> Heroes { get; set; } = new();

        public int ActiveHeroIndex { get; set; }

        public WorldSaveData World { get; set; } = new();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime LastSavedAtUtc { get; set; } = DateTime.UtcNow;

        public static ProfileSaveData CreateNew(string heroName)
        {
            return new ProfileSaveData
            {
                Heroes = new List<HeroSaveData>
                {
                    new()
                    {
                        Name = heroName
                    }
                },
                ActiveHeroIndex = 0
            };
        }
    }
}
