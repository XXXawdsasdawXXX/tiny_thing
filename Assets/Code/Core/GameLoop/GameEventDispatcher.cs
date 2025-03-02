using System.Collections.Generic;
using Code.Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

namespace Code.Core.GameLoop
{
    public class GameEventDispatcher : MonoBehaviour, IService
    {
        private readonly List<IInitListener> _initListeners = new();
        private readonly List<ILoadListener> _loadListeners = new();
        private readonly List<IStartListener> _startListeners = new();
        private readonly List<IUpdateListener> _tickListeners = new();
        private readonly List<IExitListener> _exitListeners = new();
        private readonly List<ISubscriber> _subscribers = new();

        private bool _isGameBooted;

        private void Awake()
        {
            _bootGame();
        }

        private void Update()
        {
            if (_isGameBooted)
            {
                _notifyGameUpdate();
            }
        }

        private void OnApplicationQuit()
        {
            if (_isGameBooted)
            {
                _notifyGameExit();
            }

#if DEBUGGING
            Debugging.Log(this, "[OnApplicationQuit]", Debugging.Type.GameState);
#endif
        }

        public async void InitializeRuntimeListener(IGameListeners listener)
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker($"RuntimeListener: {listener.GetType().Name}");
            marker.Begin();
#endif
            if (listener is IInitListener initListener)
            {
                await initListener.GameInitialize();
            }

            if (listener is ISubscriber subscriber)
            {
                subscriber.Subscribe();
                _subscribers.Add(subscriber);
            }

            if (listener is ILoadListener loadListener)
            {
                await loadListener.GameLoad();
            }

            if (listener is IStartListener startListener)
            {
                await startListener.GameStart();
            }

            if (listener is IUpdateListener tickListener) _tickListeners.Add(tickListener);

            if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);

#if UNITY_EDITOR
            marker.End();
#endif
        }

        public void RemoveRuntimeListener(IGameListeners listener)
        {
            if (listener is IUpdateListener tickListener) _tickListeners.Remove(tickListener);

            if (listener is IExitListener exitListener) _exitListeners.Remove(exitListener);

            if (listener is ISubscriber subscriber) subscriber.Unsubscribe();
        }
        

        private async void _bootGame()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_bootGame");
            marker.Begin();
#endif
            await _notifyGameInitialize();
            await _notifySubscribe();
            await _notifyGameLoad();
            await _notifyGameStart();

            _isGameBooted = true;

#if UNITY_EDITOR
            marker.End();
#endif

#if DEBUGGING
            Debugging.Log(this, "[_bootGame] completed", Debugging.Type.GameState);
#endif
        }

        private void _initializeListeners()
        {
            List<IGameListeners> gameListeners = Container.Instance.GetGameListeners();
            
            foreach (IGameListeners listener in gameListeners)
            {
                if (listener is IInitListener initListener) _initListeners.Add(initListener);

                if (listener is ISubscriber subscriber) _subscribers.Add(subscriber);

                if (listener is ILoadListener loadListener) _loadListeners.Add(loadListener);

                if (listener is IStartListener startListener) _startListeners.Add(startListener);

                if (listener is IUpdateListener tickListener) _tickListeners.Add(tickListener);

                if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);
            }
        }

        private async UniTask _notifyGameInitialize()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifyGameInitialize");
            marker.Begin();
#endif
            foreach (IInitListener listener in _initListeners)
            {
                await listener.GameInitialize();
            }
#if UNITY_EDITOR
            marker.End();
#endif
        }

        private async UniTask _notifyGameLoad()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifyGameLoad");
            marker.Begin();
#endif
            foreach (ILoadListener listener in _loadListeners)
            {
                await listener.GameLoad();
            }
#if UNITY_EDITOR
            marker.End();
#endif
        }

        private async UniTask _notifySubscribe()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifySubscribe");
            marker.Begin();
#endif
            foreach (ISubscriber subscriber in _subscribers)
            {
                await subscriber.Subscribe();
            }
#if UNITY_EDITOR
            marker.End();
#endif
        }

        private async UniTask _notifyGameStart()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifyGameStart");
            marker.Begin();
#endif
            foreach (IStartListener listener in _startListeners)
            {
                await listener.GameStart();
            }
#if UNITY_EDITOR
            marker.End();
#endif
        }

        private void _notifyGameUpdate()
        {
            foreach (IUpdateListener listener in _tickListeners)
            {
#if UNITY_EDITOR
                string listenerName = listener.GetType().Name;
                ProfilerMarker marker = new ProfilerMarker($"GameUpdate: {listenerName}");
                marker.Begin();
#endif
                listener.GameUpdate();
#if UNITY_EDITOR
                marker.End();
#endif
            }
        }

        private void _notifyGameExit()
        {
#if UNITY_EDITOR
            ProfilerMarker marker = new ProfilerMarker("_notifyGameExit");
            marker.Begin();
#endif
            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            foreach (IExitListener listener in _exitListeners)
            {
                listener.GameExit();
            }
#if UNITY_EDITOR
            marker.End();
#endif
        }
    }
}