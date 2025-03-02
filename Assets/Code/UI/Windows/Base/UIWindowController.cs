using UnityEngine;

namespace Code.UI.Windows.Base
{
    public abstract class UIWindowController<UIView> : MonoBehaviour where UIView : UIWindowView
    {
        [SerializeField] protected UIView view;

        private void OnEnable()
        {
            SubscribeToEvents(true);
        }

        private void OnDisable()
        {
            SubscribeToEvents(false);
        }

        protected abstract void SubscribeToEvents(bool flag);

        private void OnValidate()
        {
            if (view == null && !TryGetComponent(out view))
            {
                view = gameObject.AddComponent<UIView>();
            }
        }
    }
}