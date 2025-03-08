using Core.Data;
using UnityEngine;

namespace Core
{
    public class Mono : MonoBehaviour
    {
        [field: SerializeField] public UniqueID ID;

        protected virtual void OnValidate()
        {
            if (ID.IsEmpty())
            {
                ID.Validate();
            }
        }
    }
}