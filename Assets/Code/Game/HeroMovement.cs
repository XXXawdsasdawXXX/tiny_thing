using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class HeroMovement : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _cameraYOffset = -1;
        private Vector2 _inputDirection;
        private Camera _camera;

        public override void OnStartClient()
        {
            base.OnStartClient();
       
            if (IsOwner)
            {
                _camera = Camera.main;
                
                if (_camera != null)
                {
                    Transform cameraTransform = _camera.transform;
                    Transform playerTransform = transform;
                    
                    Vector3 cameraPosition = playerTransform.position + new Vector3(0, _cameraYOffset, -10);
                    
                    cameraTransform.SetParent(playerTransform);
                    cameraTransform.position = cameraPosition;
                }
            }
        }

        private void Update()
        {
            if (!IsOwner) return; // Только локальный игрок обрабатывает ввод

            _inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            if (_inputDirection != Vector2.zero)
            {
                SendMovementToServer(_inputDirection);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendMovementToServer(Vector2 direction)
        {
            if (_rigidbody2D != null)
            {
                Vector2 newPosition = _rigidbody2D.position + direction.normalized * _moveSpeed * Time.fixedDeltaTime;
                _rigidbody2D.MovePosition(newPosition);
            }
        }
    }
}