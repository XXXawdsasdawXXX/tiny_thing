using System.Collections.Generic;
using Code.Core.ServiceLocator;
using FishNet;
using FishNet.Object;
using UnityEditor;
using UnityEngine;

namespace Code.Game
{
    public class ItemSpawner : NetworkBehaviour, IService
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private List<NetworkObject> _itemInstances;
        [SerializeField] private int _count;
        
        [ServerRpc(RequireOwnership = false)]
        public void DropItemsRPC(Vector3 position, out NetworkObject item)
        {
            var _networkManager = InstanceFinder.NetworkManager;
            var drop = _networkManager.GetPooledInstantiated(_itemPrefab, position,Quaternion.identity, true);
            _networkManager.ServerManager.Spawn(drop);
            _networkManager.SceneManager.AddOwnerToDefaultScene(drop);
            _itemInstances.Add(drop);

            drop.name += $"{drop.ObjectId}";
            
            Debug.Log($"spawn item. count = {_itemInstances.Count}");

            item = drop;
        }


        [ServerRpc(RequireOwnership = false)]
        public void DespawnItemRPC()
        {
            Debug.Log($"despawn item. count = {_itemInstances.Count}");
            var _networkManager = InstanceFinder.NetworkManager;
    
            _networkManager.ServerManager.Despawn(_itemInstances[^1]);

            _itemInstances.Remove(_itemInstances[^1]);
        }
    }
}