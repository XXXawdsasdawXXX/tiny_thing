using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core.ServiceLocator
{
    public class Container : MonoBehaviour
    {
        public static Container Instance { get; private set; }
        
        [SerializeField] private List<ScriptableObject> _configs;

        private MonoBehaviour[] _allObjects;
        private List<IService> _services = new();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
         
            Instance = this;
            
            _allObjects = FindObjectsOfType<MonoBehaviour>();
           
            InitList(ref _services);
        }
        
        public T FindConfig<T>() where T : ScriptableObject
        {
            foreach (ScriptableObject scriptableObject in _configs)
            {
                if (scriptableObject is T findConfig)
                {
                    return findConfig;
                }
            }

            return null;
        }

        public T FindService<T>() where T : IService
        {
            foreach (IService service in _services)
            {
                if (service is T findService)
                {
                    return findService;
                }
            }

            return default;
        }
        
        private void InitList<T>(ref List<T> list)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            IEnumerable<Type> serviceTypes = types.Where(t =>
                typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract &&
                !typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (Type serviceType in serviceTypes)
            {
                if (Activator.CreateInstance(serviceType) is T service)
                {
                    list.Add(service);
                }
            }

            T[] typedMono = _allObjects.OfType<T>().ToArray();
            
            if (typedMono.Any())
            {
                list.AddRange(typedMono);
            }
        }
    }
}