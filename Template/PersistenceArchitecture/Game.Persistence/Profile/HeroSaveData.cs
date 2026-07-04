using System;
using System.Collections.Generic;

namespace Game.Persistence.Profile
{
    public sealed class HeroSaveData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string Name { get; set; } = "Hero";

        public int Health { get; set; } = 100;

        public int MaxHealth { get; set; } = 100;

        public Dictionary<string, int> Resources { get; set; } = new()
        {
            ["gold"] = 0
        };

        public List<string> DeckCardIds { get; set; } = new();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime LastPlayedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
