using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Game
{
    public class ItemSpawner : NetworkBehaviour
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private List<NetworkObject> _itemInstances;
        [SerializeField] private int _count;
      
        
        [ServerRpc]
        public void DropItemsRPC(Vector3 position)
        {
            var _networkManager = InstanceFinder.NetworkManager;
            var drop = _networkManager.GetPooledInstantiated(_itemPrefab, position,Quaternion.identity,true);
            _networkManager.ServerManager.Spawn(drop);
            _networkManager.SceneManager.AddOwnerToDefaultScene(drop);
            _itemInstances.Add(drop);
        }


        [ServerRpc]
        public  void DespawnItemRPC()
        {
            var _networkManager = InstanceFinder.NetworkManager;
    
            _networkManager.ServerManager.Despawn(_itemInstances[^1]);

            _itemInstances.Remove(_itemInstances[^1]);
        }
    }
}