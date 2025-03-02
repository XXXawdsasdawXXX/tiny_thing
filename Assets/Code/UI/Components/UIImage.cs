using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Components
{
    public class UIImage : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetFillAmount(float normalizedValue)
        {
            _image.fillAmount = normalizedValue;
        }
    }
}