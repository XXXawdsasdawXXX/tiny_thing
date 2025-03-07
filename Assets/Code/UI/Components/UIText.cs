using TMPro;
using UnityEngine;

namespace UI.Components
{
    public class UIText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        public void SetText(string text)
        {
            _textMeshPro.SetText(text);
        }
    }
}