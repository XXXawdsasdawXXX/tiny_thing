using System;
using Fusion;

namespace Code
{
    public class Ball : NetworkBehaviour
    {
        public override void FixedUpdateNetwork()
        {
            transform.position += transform.forward * 5 * Runner.DeltaTime;
        }
    }
}