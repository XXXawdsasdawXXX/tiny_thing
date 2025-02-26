using Code.Core;
using Code.Core.Extensions;
using Fusion;
using UnityEngine;

namespace Code.Game
{
    public class Hero : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed = 5;

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                Vector2 velocity = data.direction * _speed * Runner.DeltaTime;

                _rigidbody.velocity = velocity;

                Debug.Log(velocity);
            }
        }
    }
}