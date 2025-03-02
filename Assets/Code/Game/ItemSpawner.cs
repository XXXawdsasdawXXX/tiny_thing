using System.Collections.Generic;
using Code.Core.ServiceLocator;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class ItemSpawner : NetworkBehaviour, IService
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private List<NetworkObject> _itemInstances;
        [SerializeField] private int _count;
        
     
        [ServerRpc(RequireOwnership = false)]
        public void DropItemsRPC(Vector3 position)
        {
            var _networkManager = InstanceFinder.NetworkManager;
            var drop = _networkManager.GetPooledInstantiated(_itemPrefab, transform, true);
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
                var _networkManager = InstanceFinder.NetworkManager;
        
                _networkManager.ServerManager.Despawn(_itemInstances[^1]);

                _itemInstances.Remove(_itemInstances[^1]);
            }
        }
    }
}