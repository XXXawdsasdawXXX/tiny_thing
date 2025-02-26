using Unity.Mathematics;
using UnityEngine;

namespace Code.Core.Extensions
{
    public static class VectorExtension
    {
        public static Vector2 AsVector2(this float2 vector)
        {
            return new Vector2(vector.x, vector.y);
        }
    }
}