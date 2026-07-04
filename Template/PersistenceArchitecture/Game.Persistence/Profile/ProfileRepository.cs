using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Infrastructure.Save;

namespace Game.Persistence.Profile
{
    public sealed class ProfileRepository
    {
        private readonly ISaveStorage _storage;
        private readonly ISaveSerializer _serializer;
        private readonly SavePathProvider _paths;

        public ProfileRepository(
            ISaveStorage storage,
            ISaveSerializer serializer,
            SavePathProvider paths)
        {
            _storage = storage;
            _serializer = serializer;
            _paths = paths;
        }

        public async UniTask<ProfileSaveData?> LoadAsync(
            string slotId,
            CancellationToken cancellationToken = default)
        {
            string? json = await _storage.ReadTextAsync(_paths.SlotFile(slotId), cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return _serializer.Deserialize<ProfileSaveData>(json);
        }

        public async UniTask SaveAsync(
            string slotId,
            ProfileSaveData data,
            CancellationToken cancellationToken = default)
        {
            data.Version = ProfileSaveData.CurrentVersion;
            data.LastSavedAtUtc = System.DateTime.UtcNow;

            string json = _serializer.Serialize(data);
            await _storage.WriteTextAsync(_paths.SlotFile(slotId), json, cancellationToken);
        }

        public async UniTask<IReadOnlyList<string>> ListSlotIdsAsync(
            CancellationToken cancellationToken = default)
        {
            return await UniTask.RunOnThreadPool(
                () =>
                {
                    string slotsDirectory = _paths.GetAbsolutePath("slots");

                    if (!Directory.Exists(slotsDirectory))
                    {
                        return (IReadOnlyList<string>)Array.Empty<string>();
                    }

                    return Directory
                        .GetFiles(slotsDirectory, "*.json")
                        .Select(Path.GetFileNameWithoutExtension)
                        .Where(id => !string.IsNullOrEmpty(id))
                        .Cast<string>()
                        .ToList();
                },
                cancellationToken: cancellationToken);
        }

        public void Delete(string slotId)
        {
            _storage.Delete(_paths.SlotFile(slotId));
        }
    }
}
