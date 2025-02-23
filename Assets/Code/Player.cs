using Fusion;
using UnityEngine;

namespace Code
{
    public class Player : NetworkBehaviour
    {
        [Networked] private TickTimer _delay { get; set; }
        
        [SerializeField] private Ball _prefabBall;
        
        private NetworkCharacterController _characterController;
        
        private Vector3 _forward;

        private void Awake()
        {
            _characterController = GetComponent<NetworkCharacterController>();
            
            _forward = Vector3.forward;
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                
                _characterController.Move(5 * data.direction * Runner.DeltaTime);

                if (data.direction.sqrMagnitude > 0)
                {
                    _forward = data.direction;
                }

                TrySpawnBall(data);
            }
        }

        private void TrySpawnBall(NetworkInputData data)
        {
            if (HasStateAuthority && _delay.ExpiredOrNotRunning(Runner))
            {
                _delay = TickTimer.CreateFromSeconds(Runner, 0.5f);

                if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
                {
                    Vector3 spawnPosition = transform.position + _forward;

                    Runner.Spawn(
                        _prefabBall,
                        spawnPosition,
                        Quaternion.LookRotation(_forward),
                        Object.InputAuthority,
                        (runner, networkObject) => { networkObject.GetComponent<Ball>().Initialize(); });
                }
            }
        }
    }
}