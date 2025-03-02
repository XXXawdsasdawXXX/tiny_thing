using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Code.Game
{
    public class Health : NetworkBehaviour
    {
        private readonly SyncVar<int> _health = new SyncVar<int>(); 
        public override void OnStartClient()
        {
            enabled = IsOwner;
            _health.Value = 10;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UpdateHealth(10);
            }
        }
        
        [ServerRpc]
        public void UpdateHealth(int value)
        {
            _health.Value += value;
            Debug.Log($"{gameObject.name} health {_health.Value}");
        }
    }
}