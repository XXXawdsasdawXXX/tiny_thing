using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Runtime.Settings;
using VContainer.Unity;

namespace Game.Composition
{
    /// <summary>
    /// Runs once at app start inside the root LifetimeScope.
    /// Loads user settings before any scene or menu is shown.
    /// </summary>
    public sealed class BootstrapEntryPoint : IAsyncStartable
    {
        private readonly SettingsService _settings;

        public BootstrapEntryPoint(SettingsService settings)
        {
            _settings = settings;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _settings.LoadAsync(cancellation);
        }
    }
}
