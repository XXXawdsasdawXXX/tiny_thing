using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class UIButton : MonoBehaviour
    {
        public event Action Clicked;
        
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(Click);
        }

        protected virtual void Click()
        {
            Clicked?.Invoke();
        }
    }
}