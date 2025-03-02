using Code.Game;
using Code.UI.Components;
using UnityEngine;

namespace Code.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private UIImage _fill;
        
        private void OnEnable()
        {
            _health.Changed += HealthOnChanged;
        }

        private void OnDisable()
        {
            _health.Changed -= HealthOnChanged;
        }

        private void HealthOnChanged()
        {
            _fill.SetFillAmount(_health.GetNormalize());
        }
    }
}