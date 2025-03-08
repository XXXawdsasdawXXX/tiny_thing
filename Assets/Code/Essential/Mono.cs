using UnityEngine;

namespace Essential
{
    public class Mono : MonoBehaviour
    {
        [field: SerializeField] public UniqueID ID;

        protected virtual void OnEnable()
        {
            if (ID.IsEmpty())
            {
                ID.Validate();
            }
        }
        
    }
}