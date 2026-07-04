using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Infrastructure.Save;

namespace Game.Persistence.Meta
{
    public sealed class SaveMetaRepository
    {
        private readonly ISaveStorage _storage;
        private readonly ISaveSerializer _serializer;
        private readonly SavePathProvider _paths;

        public SaveMetaRepository(
            ISaveStorage storage,
            ISaveSerializer serializer,
            SavePathProvider paths)
        {
            _storage = storage;
            _serializer = serializer;
            _paths = paths;
        }

        public async UniTask<SaveMeta> LoadOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            string? json = await _storage.ReadTextAsync(_paths.MetaFile, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return SaveMeta.CreateDefault();
            }

            return _serializer.Deserialize<SaveMeta>(json);
        }

        public async UniTask SaveAsync(SaveMeta meta, CancellationToken cancellationToken = default)
        {
            meta.Version = SaveMeta.CurrentVersion;
            string json = _serializer.Serialize(meta);
            await _storage.WriteTextAsync(_paths.MetaFile, json, cancellationToken);
        }
    }
}
