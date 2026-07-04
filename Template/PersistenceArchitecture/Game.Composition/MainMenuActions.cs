using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Runtime.Session;

namespace Game.Composition
{
    /// <summary>
    /// Example menu action: continue last save or start a new slot.
    /// </summary>
    public sealed class MainMenuActions
    {
        private readonly GameSessionStarter _sessionStarter;
        private readonly GameSessionPersistence _persistence;

        private GameSessionLifetimeScope? _activeSession;

        public MainMenuActions(
            GameSessionStarter sessionStarter,
            GameSessionPersistence persistence)
        {
            _sessionStarter = sessionStarter;
            _persistence = persistence;
        }

        public async UniTask ContinueAsync(CancellationToken cancellationToken = default)
        {
            await _startSession(await _persistence.LoadLastAsync(cancellationToken), cancellationToken);
        }

        public async UniTask NewGameAsync(
            string slotId,
            string heroName,
            CancellationToken cancellationToken = default)
        {
            GameSessionState session = await _persistence.CreateNewAsync(
                slotId,
                heroName,
                cancellationToken);

            await _startSession(session, cancellationToken);
        }

        public void ReturnToMainMenu()
        {
            if (_activeSession == null)
            {
                return;
            }

            _sessionStarter.End(_activeSession);
            _activeSession = null;
        }

        private UniTask _startSession(GameSessionState session, CancellationToken cancellationToken)
        {
            ReturnToMainMenu();
            _activeSession = _sessionStarter.Start(session.SlotId);
            return UniTask.CompletedTask;
        }
    }
}
