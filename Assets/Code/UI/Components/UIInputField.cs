using System;
using TMPro;
using UnityEngine;

namespace UI.Components
{
    public class UIInputField : MonoBehaviour
    {
        public event Action<string> Changed;
        public string Value => _inputField.text;
        
        [SerializeField] private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField.onEndEdit.AddListener(EndEdit);
        }

        public void SetTextWithoutNotify(string text)
        {
            _inputField.SetTextWithoutNotify(text);
        }

        protected virtual void EndEdit(string value)
        {
            Changed?.Invoke(value);
        }
    }
}