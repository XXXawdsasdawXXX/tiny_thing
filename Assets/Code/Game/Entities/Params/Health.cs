using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Game.Entities
{
    public class Health : NetworkBehaviour
    {
        public event Action Changed;
        public float Current => _health.Value;
        public float Max => 100;
        
        private readonly SyncVar<float> _health = new();

        public override void OnStartClient()
        {
            enabled = IsOwner;
            
            _health.Value = 10;
            
            _health.OnChange += OnHealthChange;
        }

        private void OnDestroy()
        {
            _health.OnChange -= OnHealthChange;
        }

        private void OnHealthChange(float prev, float next, bool asserver)
        {
            Changed?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UpdateHealth(10);
                
                Changed?.Invoke();
            }
        }

        public float GetNormalize()
        {
            return _health.Value / Max;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void UpdateHealth(int value)
        {
            _health.Value += value;
        }
    }
}