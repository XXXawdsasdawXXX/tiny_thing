using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Persistence.Settings;

namespace Game.Runtime.Settings
{
    public sealed class SettingsService
    {
        private readonly SettingsRepository _repository;

        public SettingsData Current { get; private set; } = SettingsData.CreateDefault();

        public event Action? Changed;

        public SettingsService(SettingsRepository repository)
        {
            _repository = repository;
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken = default)
        {
            Current = await _repository.LoadOrDefaultAsync(cancellationToken);
            Changed?.Invoke();
        }

        public async UniTask SaveAsync(CancellationToken cancellationToken = default)
        {
            await _repository.SaveAsync(Current, cancellationToken);
            Changed?.Invoke();
        }

        public async UniTask SetMusicVolumeAsync(float value, CancellationToken cancellationToken = default)
        {
            Current.MusicVolume = value;
            await SaveAsync(cancellationToken);
        }

        public async UniTask SetSfxVolumeAsync(float value, CancellationToken cancellationToken = default)
        {
            Current.SfxVolume = value;
            await SaveAsync(cancellationToken);
        }

        public async UniTask SetLocaleAsync(string localeCode, CancellationToken cancellationToken = default)
        {
            Current.LocaleCode = localeCode;
            await SaveAsync(cancellationToken);
        }
    }
}
