using System;
using Fusion;
using UnityEngine;

namespace Code
{
    public class Ball : NetworkBehaviour
    {
        [Networked] private TickTimer life { get; set; }

        public void Initialize()
        {
            life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        }

        public override void FixedUpdateNetwork()
        {
            if (life.Expired(Runner))
            {
                Runner.Despawn(Object);
            }
            else
            {
                Transform body = transform;

                body.position += body.forward * 5 * Runner.DeltaTime;
            }
        }
    }
}