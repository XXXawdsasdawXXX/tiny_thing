using Code.Game;
using Code.UI.Components;
using UnityEngine;

namespace Code.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private UIImage _fill;
        
        private void Awake()
        {
            _health.Changed += HealthOnChanged;
        }

        private void OnDestroy()
        {
            _health.Changed -= HealthOnChanged;
        }

        private void HealthOnChanged()
        {
            _fill.SetFillAmount(_health.GetNormalize());
        }
    }
}