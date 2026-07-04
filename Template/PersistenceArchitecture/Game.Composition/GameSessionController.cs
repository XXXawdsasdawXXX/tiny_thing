using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Runtime.Session;
using UnityEngine;
using VContainer.Unity;

namespace Game.Composition
{
    /// <summary>
    /// Owns one gameplay session: load on enter, save on exit.
    /// </summary>
    public sealed class GameSessionController : IAsyncStartable, IAsyncDisposable
    {
        private readonly GameSessionConfig _config;
        private readonly GameSessionPersistence _persistence;
        private readonly GameSessionState _session;

        public GameSessionController(
            GameSessionConfig config,
            GameSessionPersistence persistence,
            GameSessionState session)
        {
            _config = config;
            _persistence = persistence;
            _session = session;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _persistence.LoadIntoAsync(_session, _config.SlotId, cancellation);
            Application.quitting += _saveOnQuit;
        }

        public async ValueTask DisposeAsync()
        {
            Application.quitting -= _saveOnQuit;
            await _persistence.SaveAsync(_session, CancellationToken.None);
        }

        public async UniTask SaveNowAsync(CancellationToken cancellationToken = default)
        {
            await _persistence.SaveAsync(_session, cancellationToken);
        }

        private void _saveOnQuit()
        {
            _persistence.SaveAsync(_session, CancellationToken.None).Forget();
        }
    }
}
