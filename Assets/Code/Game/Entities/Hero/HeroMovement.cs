using Core.GameLoop;
using Core.Input;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet.Object;
using UnityEngine;

namespace Game.Entities
{
    public class HeroMovement : NetworkBehaviour, IInitializeListener, IFixedUpdateListener
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _cameraYOffset = 4;
      
        private Camera _camera;
        private InputManager _inputManager;

        public override void OnStartClient()
        {
            base.OnStartClient();
           
            if (!IsOwner)
            {
                return;
            }
            
            _camera = Camera.main;

            if (_camera != null)//todo move to hero pool
            {
                Transform cameraTransform = _camera.transform;
                Transform playerTransform = transform;

                Vector3 cameraPosition = playerTransform.position + new Vector3(0, _cameraYOffset, -10);

                cameraTransform.SetParent(playerTransform);
                cameraTransform.position = cameraPosition;
                
                
                Log.Info("spawn",Color.magenta, this);
            }
        }

        public UniTask GameInitialize()
        {
                Log.Info("initialize",Color.magenta, this);
            _inputManager = Container.Instance.GetService<InputManager>();
            
            return UniTask.CompletedTask;
        }
        
        public void GameFixedUpdate(float fixedDeltaTime)
        {
            _rigidbody2D.velocity = _inputManager.Direction.normalized * _moveSpeed * fixedDeltaTime;
        }
    }
}