using System.Collections.Generic;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
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
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;

            
    
            return UniTask.CompletedTask;
        }

        public void Unsubscribe()
        {
            InstanceFinder.NetworkManager.ServerManager.OnSpawn -= OnNetworkObjectSpawned;
            InstanceFinder.NetworkManager.ServerManager.OnDespawn -= OnNetworkObjectDespawned;
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }

        private void OnClientConnectionState(ClientConnectionStateArgs connectionState)
        {
            Log.Info($"Client: connection state {connectionState.ConnectionState}",Color.cyan, this);
            if (connectionState.ConnectionState == LocalConnectionState.Started)
            {
                var list = InstanceFinder.ClientManager.Connection.Objects;
             
            Log.Info($"Client: objects count = {list.Count}",Color.cyan, this);
                foreach ( NetworkObject obj in list)
                {
                    
                    Log.Info($"Client: find object {obj.name} ",Color.cyan, this);
                        IGameListener[] listeners = obj.GetComponentsInChildren<IGameListener>(true);

                    foreach (IGameListener gameListener in listeners)
                    {
                        _gameEventDispatcher.AddListener(gameListener);
                    }
                    
                }
            }
            
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