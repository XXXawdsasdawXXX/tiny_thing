using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Infrastructure.Save
{
    /// <summary>
    /// Low-level persistence: read and write text files under the save root.
    /// Knows nothing about game data shapes.
    /// </summary>
    public interface ISaveStorage
    {
        UniTask WriteTextAsync(
            string relativePath,
            string content,
            CancellationToken cancellationToken = default);

        UniTask<string?> ReadTextAsync(
            string relativePath,
            CancellationToken cancellationToken = default);

        bool Exists(string relativePath);

        void Delete(string relativePath);
    }
}
