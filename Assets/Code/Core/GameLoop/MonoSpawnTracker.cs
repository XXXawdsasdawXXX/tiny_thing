using Core.Network;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;

namespace Core.GameLoop
{
    public class MonoSpawnTracker : IService, IInitializeListener, ISubscriber
    {
        private GameEventDispatcher _gameEventDispatcher;
        private PlayerSpawner _playerSpawner;

        public UniTask GameInitialize()
        {
            _gameEventDispatcher = Container.Instance.GetService<GameEventDispatcher>();
            
            return UniTask.CompletedTask;
        }

        public UniTask Subscribe()
        {
            Essential.Mono.Started += OnMonoStarted;
            Essential.Mono.Destroyed -= OnMonoDestroyed;

            return UniTask.CompletedTask;
        }

        public void Unsubscribe()
        {
            Essential.Mono.Started -= OnMonoStarted;
            Essential.Mono.Destroyed -= OnMonoDestroyed;
        }

        private void OnMonoStarted(Essential.Mono obj)
        {
            if (obj.TryGetComponent(out IGameListener gameListener))
            {
                _gameEventDispatcher.AddListener(gameListener);
            }
        }

        private void OnMonoDestroyed(Essential.Mono obj)
        {
            if (obj.TryGetComponent(out IGameListener gameListener))
            {
                _gameEventDispatcher.RemoveListener(gameListener);
            }
        }
    }
}