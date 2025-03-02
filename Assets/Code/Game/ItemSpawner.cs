using FishNet.Connection;
using FishNet.Object;
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
                _itemInstance = Instantiate(_itemPrefab, transform.position + Vector3.right, Quaternion.identity);
            
                SpawnObject(_itemInstance.gameObject);
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
            ServerManager.Spawn(instance);
        }
        
        
        [ServerRpc]
        private void DeleteObject()
        {
            ServerManager.Despawn(_itemInstance.gameObject);
        }
    }
}