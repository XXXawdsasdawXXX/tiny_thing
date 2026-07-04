using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Infrastructure.Save
{
    public sealed class FileSaveStorage : ISaveStorage
    {
        private readonly SavePathProvider _paths;

        public FileSaveStorage(SavePathProvider paths)
        {
            _paths = paths;
            _paths.EnsureRootExists();
        }

        public async UniTask WriteTextAsync(
            string relativePath,
            string content,
            CancellationToken cancellationToken = default)
        {
            string absolutePath = _paths.GetAbsolutePath(relativePath);
            string? directory = Path.GetDirectoryName(absolutePath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await UniTask.RunOnThreadPool(
                () => File.WriteAllText(absolutePath, content),
                cancellationToken: cancellationToken);
        }

        public async UniTask<string?> ReadTextAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            string absolutePath = _paths.GetAbsolutePath(relativePath);

            if (!File.Exists(absolutePath))
            {
                return null;
            }

            return await UniTask.RunOnThreadPool(
                () => File.ReadAllText(absolutePath),
                cancellationToken: cancellationToken);
        }

        public bool Exists(string relativePath)
        {
            return File.Exists(_paths.GetAbsolutePath(relativePath));
        }

        public void Delete(string relativePath)
        {
            string absolutePath = _paths.GetAbsolutePath(relativePath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}
