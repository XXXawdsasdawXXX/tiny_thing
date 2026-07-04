using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Infrastructure.Save;

namespace Game.Persistence.Settings
{
    public sealed class SettingsRepository
    {
        private readonly ISaveStorage _storage;
        private readonly ISaveSerializer _serializer;
        private readonly SavePathProvider _paths;

        public SettingsRepository(
            ISaveStorage storage,
            ISaveSerializer serializer,
            SavePathProvider paths)
        {
            _storage = storage;
            _serializer = serializer;
            _paths = paths;
        }

        public async UniTask<SettingsData> LoadOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            string? json = await _storage.ReadTextAsync(_paths.SettingsFile, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return SettingsData.CreateDefault();
            }

            return _serializer.Deserialize<SettingsData>(json);
        }

        public async UniTask SaveAsync(SettingsData data, CancellationToken cancellationToken = default)
        {
            data.Version = SettingsData.CurrentVersion;
            string json = _serializer.Serialize(data);
            await _storage.WriteTextAsync(_paths.SettingsFile, json, cancellationToken);
        }
    }
}
