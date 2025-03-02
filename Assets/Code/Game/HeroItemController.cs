using Code.Core.ServiceLocator;
using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class HeroItemController :  NetworkBehaviour
    {
        private ItemSpawner _itemSpawner;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            enabled = IsOwner;

            _itemSpawner = Container.Instance.GetService<ItemSpawner>();
            
            Debug.Log($"spawn hero {_itemSpawner != null}");
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.F1))
            {
                _itemSpawner.DropItemsRPC(transform.position + Vector3.right, out NetworkObject item);
                
                Debug.Log($"try spawn item {item.name}");
                
                
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _itemSpawner.DespawnItemRPC();
            }
        }

    }
}