using System;
using Game.Collisions;
using UnityEngine;

namespace Game.InteractionObjects
{
    public abstract class InteractionObject : Core.Mono
    {
        public event Action<InteractionObject> InteractionStarted;
        
        [field: SerializeField] public InteractionTrigger Trigger { get; private set; }
        
        public EInteractionObjectType Type { get; private set; }

        public string ID { get; private set; }


        public virtual void StartInteraction()
        {
            InteractionStarted?.Invoke(this);
        }
    }
}