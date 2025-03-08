using System;
using Sirenix.OdinInspector;

namespace Core.Data
{
    [Serializable]
    public struct UniqueID
    {
        [ReadOnly, ShowInInspector] public string Value { get; private set; }
        
        public void Validate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                Value = Guid.NewGuid().ToString();
            }
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Value);
        }
    }
}