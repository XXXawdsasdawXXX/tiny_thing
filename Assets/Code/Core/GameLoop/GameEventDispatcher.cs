using System.Collections.Generic;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

namespace Core.GameLoop
{
    public class GameEventDispatcher : MonoBehaviour, IService
    {
        private readonly List<IInitializeListener> _initListeners = new();
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
        }

        public async void InitializeRuntimeListener(IGameListeners listener)
        {
            ProfilerMarker marker = new ProfilerMarker($"RuntimeListener: {listener.GetType().Name}");
            marker.Begin();
            
            if (listener is IInitializeListener initListener)
            {
                await initListener.GameInitialize();
            }

            if (listener is ISubscriber subscriber)
            {
                await subscriber.Subscribe();
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

            marker.End();
        }

        public void RemoveRuntimeListener(IGameListeners listener)
        {
            if (listener is IUpdateListener tickListener) _tickListeners.Remove(tickListener);

            if (listener is IExitListener exitListener) _exitListeners.Remove(exitListener);

            if (listener is ISubscriber subscriber) subscriber.Unsubscribe();
        }
        
        private async void _bootGame()
        {
            ProfilerMarker marker = new ProfilerMarker("_bootGame");
            marker.Begin();

            await _notifyGameInitialize();
            await _notifySubscribe();
            await _notifyGameLoad();
            await _notifyGameStart();

            _isGameBooted = true;

            marker.End();
        }

        private void _initializeListeners()
        {
            List<IGameListeners> gameListeners = Container.Instance.GetGameListeners();
            
            foreach (IGameListeners listener in gameListeners)
            {
                if (listener is IInitializeListener initListener) _initListeners.Add(initListener);

                if (listener is ISubscriber subscriber) _subscribers.Add(subscriber);

                if (listener is ILoadListener loadListener) _loadListeners.Add(loadListener);

                if (listener is IStartListener startListener) _startListeners.Add(startListener);

                if (listener is IUpdateListener tickListener) _tickListeners.Add(tickListener);

                if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);
            }
        }

        private async UniTask _notifyGameInitialize()
        {
            ProfilerMarker marker = new ProfilerMarker("_notifyGameInitialize");
            marker.Begin();

            foreach (IInitializeListener listener in _initListeners)
            {
                await listener.GameInitialize();
            }

            marker.End();
        }

        private async UniTask _notifyGameLoad()
        {
            ProfilerMarker marker = new ProfilerMarker("_notifyGameLoad");
            marker.Begin();

            foreach (ILoadListener listener in _loadListeners)
            {
                await listener.GameLoad();
            }
            
            marker.End();
        }

        private async UniTask _notifySubscribe()
        {
            ProfilerMarker marker = new ProfilerMarker("_notifySubscribe");
            marker.Begin();

            foreach (ISubscriber subscriber in _subscribers)
            {
                await subscriber.Subscribe();
            }

            marker.End();
        }

        private async UniTask _notifyGameStart()
        {
            ProfilerMarker marker = new ProfilerMarker("_notifyGameStart");
            marker.Begin();

            foreach (IStartListener listener in _startListeners)
            {
                await listener.GameStart();
            }

            marker.End();
        }

        private void _notifyGameUpdate()
        {
            foreach (IUpdateListener listener in _tickListeners)
            {
                string listenerName = listener.GetType().Name;
                ProfilerMarker marker = new ProfilerMarker($"GameUpdate: {listenerName}");
                marker.Begin();
                
                listener.GameUpdate();

                marker.End();
            }
        }

        private void _notifyGameExit()
        {
            ProfilerMarker marker = new ProfilerMarker("_notifyGameExit");
            marker.Begin();

            foreach (ISubscriber subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            foreach (IExitListener listener in _exitListeners)
            {
                listener.GameExit();
            }

            marker.End();
        }
    }
}