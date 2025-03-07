using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Components
{
    [Serializable]
    public class UIElementPool<UIElement> where UIElement : MonoBehaviour, IPoolableUIElement
    {
        [SerializeField] private Transform _root;
        [SerializeField] private UIElement _prefab;
        [SerializeField] private List<UIElement> _all = new();
        [SerializeField] private List<UIElement> _enabled = new();

        public UIElement GetNext()
        {
            UIElement entity = GetDisabledEntity() ?? AddNewEntity();
          
            _enabled.Add(entity);
            
            entity.Enable();
            
            return entity;
        }

        private UIElement GetDisabledEntity()
        {
            return _all.FirstOrDefault(entity => entity != null && !entity.gameObject.activeSelf);
        }

        private UIElement AddNewEntity()
        {
            UIElement entity = Object.Instantiate(_prefab, _root);
         
            _all.Add(entity);
            
            return entity;
        }

        public IReadOnlyList<UIElement> GetAll()
        {
            return _all;
        }

        public IEnumerable<UIElement> GetAllEnabled()
        {
            return _enabled;
        }

        public void Disable(UIElement entity)
        {
            if (entity == null || !entity.gameObject.activeSelf)
            {
                return;
            }
            
            entity.Disable();
            
            _enabled.Remove(entity);
        }

        public void DisableAll()
        {
            foreach (UIElement entity in _all)
            {
                entity.Disable();
            }

            _enabled.Clear();
        }

        public UIElement GetByIndex(int tabIndex)
        {
            return _all[tabIndex];
        }
    }
    
    public interface IPoolableUIElement
    {
        void Enable();
        void Disable();
    }
}