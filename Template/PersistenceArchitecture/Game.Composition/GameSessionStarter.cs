using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Composition
{
    /// <summary>
    /// Creates a child gameplay scope from the root scope.
    /// Assign the prefab reference in the inspector on a menu/bootstrap object.
    /// </summary>
    public sealed class GameSessionStarter
    {
        private readonly LifetimeScope _rootScope;
        private readonly GameSessionLifetimeScope _sessionScopePrefab;

        public GameSessionStarter(
            LifetimeScope rootScope,
            GameSessionLifetimeScope sessionScopePrefab)
        {
            _rootScope = rootScope;
            _sessionScopePrefab = sessionScopePrefab;
        }

        public GameSessionLifetimeScope Start(string slotId)
        {
            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                GameSessionLifetimeScope scope = Object.Instantiate(_sessionScopePrefab);
                scope.Configure(slotId);
                return scope;
            }
        }

        public void End(GameSessionLifetimeScope scope)
        {
            if (scope != null)
            {
                Object.Destroy(scope.gameObject);
            }
        }
    }
}
