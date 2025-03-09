using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace Core.GameLoop
{
    public class NetworkSpawnTracker : IService, IInitializeListener, ISubscriber
    {
        private GameEventDispatcher _gameEventDispatcher;

        public UniTask GameInitialize()
        {
            _gameEventDispatcher = Container.Instance.GetService<GameEventDispatcher>();
            
            return UniTask.CompletedTask;
        }

        public UniTask Subscribe()
        {
            InstanceFinder.NetworkManager.ServerManager.OnSpawn += OnNetworkObjectSpawned;
            InstanceFinder.NetworkManager.ServerManager.OnDespawn += OnNetworkObjectDespawned;
            
            return UniTask.CompletedTask;
        }

        public void Unsubscribe()
        {
            InstanceFinder.NetworkManager.ServerManager.OnSpawn -= OnNetworkObjectSpawned;
            InstanceFinder.NetworkManager.ServerManager.OnDespawn -= OnNetworkObjectDespawned;
        }
        
        private void OnNetworkObjectSpawned(NetworkObject obj)
        {
            IGameListener[] listeners = obj.GetComponentsInChildren<IGameListener>(true);

            foreach (IGameListener gameListener in listeners)
            {
                _gameEventDispatcher.AddListener(gameListener);
            }
            
            Log.Info($"[TRACKER] Заспавнен объект: {obj.name} {listeners.Length}",Color.cyan, this);
        }

        private void OnNetworkObjectDespawned(NetworkObject obj)
        {
            IGameListener[] listeners = obj.GetComponentsInChildren<IGameListener>(true);

            foreach (IGameListener gameListener in listeners)
            {
                _gameEventDispatcher.RemoveListener(gameListener);
            }
            
            Log.Info($"[TRACKER] Удален объект: {obj.name} ",Color.cyan, this);
        }
    }
}