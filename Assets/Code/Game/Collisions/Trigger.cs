using System;
using UnityEngine;

namespace Game.Collisions
{
    public class Trigger : Core.Essential.Mono
    {
        public event Action<GameObject> Enter; 
        public event Action<GameObject> Exit; 
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            Enter?.Invoke(col.gameObject);    
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            Exit?.Invoke(other.gameObject);    
        }
    }
}