using UnityEngine;

namespace Core.Input
{
    public class InputConfig : ScriptableObject
    {
        [field: SerializeField] public InputActionKey[] InputActionKeys { get; private set; }
    }
}