using System;

namespace Game.Persistence.Profile
{
    public sealed class WorldSaveData
    {
        public string SceneId { get; set; } = "world_00";

        public TimeSpan GameTime { get; set; }

        public DateTime LastSavedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
