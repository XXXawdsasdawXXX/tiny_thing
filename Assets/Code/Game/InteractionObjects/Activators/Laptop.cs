using Core;
using Core.GameLoop;
using Core.Input;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet;
using UnityEngine;
using Channel = FishNet.Transporting.Channel;

namespace Game.InteractionObjects.Activators
{
    public class Laptop : InteractionObject, IInitializeListener, ISubscriber
    {
        private InputManager _inputManager;
        private ActivatorBroadcast _activatorBroadcast;
        
        public UniTask GameInitialize()
        {
            _inputManager = Container.Instance.GetService<InputManager>();
            
            Log.Info($"{_inputManager != null}", Color.blue, this);
            
            return UniTask.CompletedTask;
        }

        public override void StartInteraction()
        {
            base.StartInteraction();
        }

        public UniTask Subscribe()
        {
            SubscribeToEvents(true);

            return UniTask.CompletedTask;
        }

        public void Unsubscribe()
        {
            SubscribeToEvents(false);
        }
        
        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                InstanceFinder.ClientManager.RegisterBroadcast<ActivatorBroadcast>(OnActivatorChanged);
                Trigger.InteractionPerformed += OnInteractionPerformed;
            }
            else
            {
                InstanceFinder.ClientManager.UnregisterBroadcast<ActivatorBroadcast>(OnActivatorChanged);

            }
        }

        private void OnInteractionPerformed()
        {
            throw new System.NotImplementedException();
        }

        private void OnActivatorChanged(ActivatorBroadcast broadcast, Channel channel)
        {
            
        }
    }
}