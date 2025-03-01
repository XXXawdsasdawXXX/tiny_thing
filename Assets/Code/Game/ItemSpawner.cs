using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class ItemSpawner : NetworkBehaviour
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private Item _itemInstance;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            enabled = IsOwner;
        }

        private void Update()
        {
            if (_itemInstance == null && Input.GetKeyDown(KeyCode.F1))
            {
                SpawnObject();
            }

            if (_itemInstance != null && Input.GetKeyDown(KeyCode.F2))
            {
                DeleteObject();
            }
        }

        [ServerRpc]
        private void SpawnObject()
        {
            _itemInstance = Instantiate(_itemPrefab, transform.position + Vector3.right, Quaternion.identity);
        
            ServerManager.Spawn(_itemInstance.gameObject);
        }
        
        [ServerRpc]
        private void DeleteObject()
        {
            ServerManager.Despawn(_itemInstance.gameObject);

            _itemInstance = null;
        }
    }
}