﻿using System;
using Core.GameLoop;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Game.Entities.Params
{
    public class PersonName : NetworkBehaviour, ISubscriber
    {

        public event Action Changed;

        public string Name => _name.Value;
        
        private readonly SyncVar<string> _name = new();
        
        public override void OnStartClient()
        {
            enabled = IsOwner;
            
            
            _name.Value = GetHashCode().ToString();
            
        }

        public UniTask Subscribe()
        {
            _name.OnChange += OnNameChanged;
        
            return UniTask.CompletedTask;
        }
        
        public void Unsubscribe()
        {
            _name.OnChange -= OnNameChanged;
        }

        private void OnDestroy()
        {
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetName(string personName)
        {
            _name.Value = personName;
        }

        private void OnNameChanged(string prev, string next, bool asserver)
        {
            Changed?.Invoke();
        }
    }
}