using FishNet.Object;
using UnityEngine;

namespace Code.Game
{
    public class HeroColor : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public override void OnStartClient()
        {
            base.OnStartClient();

            enabled = IsOwner;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeColorServer(null, Color.red);
            }
        }

        [ObserversRpc]
        private void ChangeColorObserver(GameObject hero, Color color)
        {
            _spriteRenderer.color = color;
        }

        [ServerRpc]
        private void ChangeColorServer(GameObject hero, Color color)
        {
            ChangeColorObserver(hero, color);
        }
    }
}