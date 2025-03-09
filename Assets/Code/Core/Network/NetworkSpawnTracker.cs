using Core.GameLoop;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Core.Network
{
    public class NetworkSpawnTracker : IService, IInitializeListener, ISubscriber
    {
        private GameEventDispatcher _gameEventDispatcher;
        private PlayerSpawner _playerSpawner;

        public UniTask GameInitialize()
        {
            _gameEventDispatcher = Container.Instance.GetService<GameEventDispatcher>();

            _playerSpawner = Container.Instance.GetService<PlayerSpawner>();
            return UniTask.CompletedTask;
        }

        public UniTask Subscribe()
        {
            InstanceFinder.NetworkManager.ServerManager.OnSpawn += OnNetworkObjectSpawned;
            InstanceFinder.NetworkManager.ServerManager.OnDespawn += OnNetworkObjectDespawned;
            _playerSpawner.OnSpawned += OnNetworkObjectSpawned;

            //   InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;


            return UniTask.CompletedTask;
        }

        public void Unsubscribe()
        {
            InstanceFinder.NetworkManager.ServerManager.OnSpawn -= OnNetworkObjectSpawned;
            InstanceFinder.NetworkManager.ServerManager.OnDespawn -= OnNetworkObjectDespawned;
            _playerSpawner.OnSpawned -= OnNetworkObjectSpawned;

        }

        private void OnClientConnectionState(NetworkConnection arg1, bool arg2)
        {
            var list = InstanceFinder.NetworkManager.ServerManager.Clients;

            Log.Info($"Client: objects count = {list.Count}", Color.cyan, this);
            
            //foreach (NetworkObject obj in list)
 
        }


        /*
        private void OnClientConnectionState(ClientConnectionStateArgs connectionState)
        {
          
            /*Log.Info($"Client: connection state {connectionState.ConnectionState}",Color.cyan, this);
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
                    
                }#1#
            
        }
        */

        private void OnNetworkObjectSpawned(NetworkObject obj)
        {
            IGameListener[] listeners = obj.GetComponentsInChildren<IGameListener>(true);

            foreach (IGameListener gameListener in listeners)
            {
                _gameEventDispatcher.AddListener(gameListener);
            }

            Log.Info($"[TRACKER] Заспавнен объект: {obj.name} {listeners.Length}", Color.cyan, this);
        }

        private void OnNetworkObjectDespawned(NetworkObject obj)
        {
            IGameListener[] listeners = obj.GetComponentsInChildren<IGameListener>(true);

            foreach (IGameListener gameListener in listeners)
            {
                _gameEventDispatcher.RemoveListener(gameListener);
            }

            Log.Info($"[TRACKER] Удален объект: {obj.name} ", Color.cyan, this);
        }
    }
}