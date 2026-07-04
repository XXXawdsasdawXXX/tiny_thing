using System;
using System.Collections.Generic;
using System.Linq;
using Game.Persistence.Profile;

namespace Game.Runtime.Session
{
    public sealed class HeroState
    {
        public string Id { get; private set; } = string.Empty;

        public string Name { get; set; } = "Hero";

        public int Health { get; private set; }

        public int MaxHealth { get; private set; }

        public IReadOnlyDictionary<string, int> Resources => _resources;

        public IReadOnlyList<string> DeckCardIds => _deckCardIds;

        private readonly Dictionary<string, int> _resources = new();
        private readonly List<string> _deckCardIds = new();

        public void Apply(HeroSaveData data)
        {
            Id = data.Id;
            Name = data.Name;
            Health = data.Health;
            MaxHealth = data.MaxHealth;

            _resources.Clear();
            foreach (KeyValuePair<string, int> pair in data.Resources)
            {
                _resources[pair.Key] = pair.Value;
            }

            _deckCardIds.Clear();
            _deckCardIds.AddRange(data.DeckCardIds);
        }

        public HeroSaveData ToSaveData()
        {
            return new HeroSaveData
            {
                Id = Id,
                Name = Name,
                Health = Health,
                MaxHealth = MaxHealth,
                Resources = new Dictionary<string, int>(_resources),
                DeckCardIds = new List<string>(_deckCardIds),
                LastPlayedAtUtc = DateTime.UtcNow
            };
        }

        public void AddResource(string resourceId, int amount)
        {
            _resources.TryGetValue(resourceId, out int current);
            _resources[resourceId] = current + amount;
        }

        public bool TrySpendResource(string resourceId, int amount)
        {
            _resources.TryGetValue(resourceId, out int current);

            if (current < amount)
            {
                return false;
            }

            _resources[resourceId] = current - amount;
            return true;
        }

        public void Damage(int amount)
        {
            Health = Math.Max(0, Health - amount);
        }

        public void Heal(int amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
        }
    }
}
