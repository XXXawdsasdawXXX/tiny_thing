using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Code.Game
{
    public class Health : NetworkBehaviour
    {
        public event Action Changed;
        public float Current => _health.Value;
        public float Max => 100;
        
        private readonly SyncVar<float> _health = new SyncVar<float>(); 
        
        public override void OnStartClient()
        {
            enabled = IsOwner;
            
            _health.Value = 10;
            
                      
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
            Debug.Log($"GetNormalize health {_health.Value} / {Max} = {_health.Value / Max}");
            return _health.Value / Max;
        }
        
        [ServerRpc]
        public void UpdateHealth(int value)
        {
            _health.Value += value;
         
            Debug.Log($"update health {_health.Value}");
            Changed?.Invoke();
        }
    }
}