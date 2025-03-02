﻿using UnityEngine;

namespace Code.UI.Windows.Base
{
    public abstract class UIWindowView : MonoBehaviour
    {
        [SerializeField] protected RectTransform body;

        public virtual void Open()
        {
            body.gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            body.gameObject.SetActive(false);
        }
    }
}