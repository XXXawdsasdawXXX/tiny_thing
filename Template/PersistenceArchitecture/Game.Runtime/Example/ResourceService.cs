using Game.Runtime.Session;

namespace Game.Runtime.Example
{
    /// <summary>
    /// Example gameplay service: inject <see cref="GameSessionState"/>, not save DTOs.
    /// </summary>
    public sealed class ResourceService
    {
        private readonly GameSessionState _session;

        public ResourceService(GameSessionState session)
        {
            _session = session;
        }

        public int GetGold()
        {
            _session.ActiveHero.Resources.TryGetValue("gold", out int gold);
            return gold;
        }

        public bool SpendGold(int amount)
        {
            return _session.ActiveHero.TrySpendResource("gold", amount);
        }

        public void AddGold(int amount)
        {
            _session.ActiveHero.AddResource("gold", amount);
        }
    }
}
