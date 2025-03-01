using Code.UI.Components;
using UnityEngine;

namespace Code.UI.Windows.Connection
{
    public class ConnectionWindowView : UIWindowView
    {
        [field: SerializeField] public UIButton ButtonHost { get; private set; }
        [field: SerializeField] public UIButton ButtonClient { get; private set; }
        [field: SerializeField] public UIInputField InputFieldHostIP { get; private set; }
    }
}