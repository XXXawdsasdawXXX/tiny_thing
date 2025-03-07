﻿using System;
using Core.GameLoop;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;

namespace Core.Input
{
    public class InputManager : IService, IInitializeListener, IUpdateListener
    {
        public event Action<EInputAction> ActionStarted; 
        public event Action<EInputAction> ActionEnded; 
        public float2 Direction { get; private set; }

        private InputActionKey[] _inputActionKeys;


        public UniTask GameInitialize()
        {
            _inputActionKeys = Container.Instance.GetConfig<InputConfig>().InputActionKeys;
                
            return UniTask.CompletedTask;
        }

        public void GameUpdate()
        {
            Direction = new float2(
                UnityEngine.Input.GetAxisRaw("Horizontal"),
                UnityEngine.Input.GetAxisRaw("Vertical"));

            foreach (InputActionKey inputActionKey in _inputActionKeys)
            {
                if (UnityEngine.Input.GetKeyDown(inputActionKey.Key))
                {
                    ActionStarted?.Invoke(inputActionKey.Action);
                }
                
                if (UnityEngine.Input.GetKeyUp(inputActionKey.Key))
                {
                    ActionEnded?.Invoke(inputActionKey.Action);
                }
            }
        }
    }
}