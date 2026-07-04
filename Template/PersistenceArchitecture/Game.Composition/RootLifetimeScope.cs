using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Composition
{
    /// <summary>
    /// Root scope: lives for the entire app lifetime.
    /// Attach to a DontDestroyOnLoad GameObject in the bootstrap scene.
    /// </summary>
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSessionLifetimeScope _sessionScopePrefab = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            SaveInstaller.RegisterSaveInfrastructure(builder);
            SaveInstaller.RegisterSessionStarter(builder, _sessionScopePrefab);
            builder.Register<MainMenuActions>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BootstrapEntryPoint>();
        }
    }
}
