﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Libraries
{
    [CreateAssetMenu(fileName = "Library_Asset", menuName = "Game/Library/Asset")]
    public class AssetLibrary : ScriptableObject
    {
        [field: SerializeField, GUIColor(0.1f, 0.7f, 0.9f, 0.5f)] public MaterialLibrary Material { get; private set; }
    }
}