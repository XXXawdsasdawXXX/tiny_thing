using System;
using System.Collections.Generic;
using System.Linq;
using Game.Persistence.Profile;

namespace Game.Runtime.Session
{
    public sealed class GameSessionState
    {
        public string SlotId { get; private set; } = string.Empty;

        public string SlotLabel { get; set; } = "Save";

        public IReadOnlyList<HeroState> Heroes => _heroes;

        public HeroState ActiveHero => _heroes[_activeHeroIndex];

        public WorldSaveData World { get; private set; } = new();

        private readonly List<HeroState> _heroes = new();
        private int _activeHeroIndex;

        public void LoadFrom(string slotId, ProfileSaveData data)
        {
            SlotId = slotId;
            SlotLabel = data.SlotLabel;
            World = data.World ?? new WorldSaveData();
            _activeHeroIndex = Math.Clamp(data.ActiveHeroIndex, 0, Math.Max(0, data.Heroes.Count - 1));

            _heroes.Clear();

            foreach (HeroSaveData heroData in data.Heroes)
            {
                HeroState hero = new();
                hero.Apply(heroData);
                _heroes.Add(hero);
            }

            if (_heroes.Count == 0)
            {
                HeroState hero = new();
                hero.Apply(new HeroSaveData());
                _heroes.Add(hero);
                _activeHeroIndex = 0;
            }
        }

        public ProfileSaveData ToSaveData()
        {
            return new ProfileSaveData
            {
                SlotLabel = SlotLabel,
                ActiveHeroIndex = _activeHeroIndex,
                World = World,
                Heroes = _heroes.Select(hero => hero.ToSaveData()).ToList()
            };
        }

        public void SelectHero(int index)
        {
            if (index < 0 || index >= _heroes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _activeHeroIndex = index;
        }
    }
}
