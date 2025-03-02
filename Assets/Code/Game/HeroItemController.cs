using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class HeroItemController :  NetworkBehaviour
    {
        [SerializeField] private ItemSpawner _itemSpawner;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            enabled = IsOwner;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _itemSpawner.DropItemsRPC(transform.position + Vector3.right);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _itemSpawner.DespawnItemRPC();
            }
        }

    }
}