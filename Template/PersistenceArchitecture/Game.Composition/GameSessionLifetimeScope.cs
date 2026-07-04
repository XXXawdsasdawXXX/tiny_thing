using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Persistence.Meta;
using Game.Runtime.Session;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Composition
{
    /// <summary>
    /// Configure this scope when entering gameplay (one slot = one session).
    /// </summary>
    public sealed class GameSessionLifetimeScope : LifetimeScope
    {
        [SerializeField] private string _slotId = SaveMeta.DefaultSlotId;

        public string SlotId => _slotId;

        public void Configure(string slotId)
        {
            _slotId = slotId;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(new GameSessionConfig(_slotId));

            builder.Register<GameSessionState>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameSessionController>();
        }
    }
}
