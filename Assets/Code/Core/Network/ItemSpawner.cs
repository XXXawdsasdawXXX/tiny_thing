using System.Collections.Generic;
using Code.Core.ServiceLocator;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

namespace Code.Core.Network
{
    public class ItemSpawner : NetworkBehaviour, IService
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private NetworkObject _itemPrefab;
      
        private readonly List<NetworkObject> _itemInstances = new();

        [ServerRpc(RequireOwnership = false)]
        public void DropItemsRPC(Vector3 position)
        {
            NetworkObject drop = _networkManager.GetPooledInstantiated(_itemPrefab, transform, true);
            drop.transform.position = position;

            _networkManager.ServerManager.Spawn(drop);
            _networkManager.SceneManager.AddOwnerToDefaultScene(drop);
         
            _itemInstances.Add(drop);
        }


        [ServerRpc(RequireOwnership = false)]
        public void DespawnItemRPC()
        {
            if (_itemInstances.Count > 0)
            {
                _networkManager.ServerManager.Despawn(_itemInstances[^1]);

                _itemInstances.Remove(_itemInstances[^1]);
            }
        }
    }
}