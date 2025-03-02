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
        [SerializeField] private Item _itemInstance;
        [SerializeField] private int _count;
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            enabled = IsOwner;
        }

        private void Update()
        {
            if (_itemInstance == null && Input.GetKeyDown(KeyCode.F1))
            {
               DropItemsRPC(_itemPrefab, transform.position + Vector3.right);
            }

            if (_itemInstance != null && Input.GetKeyDown(KeyCode.F2))
            {
                DeleteObject();
                
                _itemInstance = null;
            }
        }

        [ServerRpc]
        private void SpawnObject(GameObject instance)
        {
            
            instance.SetActive(true);
        }
        
        
        [ServerRpc]
        private void DeleteObject()
        {
            ServerManager.Despawn(_itemInstance.gameObject);
        }
        
        [ServerRpc]
        private void DropItemsRPC(NetworkObject prefab, Vector3 position)
        {
            var _networkManager = InstanceFinder.NetworkManager;
            var drop = _networkManager.GetPooledInstantiated(prefab,position,Quaternion.identity,true);
            _networkManager.ServerManager.Spawn(drop);
            _networkManager.SceneManager.AddOwnerToDefaultScene(drop);
        }
    }
}