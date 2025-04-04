﻿using System.Collections.Generic;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Essential;
using FishNet;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

namespace Core.GameLoop
{
    public sealed class GameEventDispatcher : Essential.Mono, IService
    {
        private static readonly ProfilerMarker _fixedUpdateProfilerMarker = new("_notifyGameFixedUpdate:");
        private static readonly ProfilerMarker _updateProfilerMarker = new("_notifyGameUpdate:");

        private readonly HashSet<IGameListener> _listeners = new();
        private readonly HashSet<IInitializeListener> _initListeners = new();
        private readonly HashSet<ILoadListener> _loadListeners = new();
        private readonly HashSet<IStartListener> _startListeners = new();
        private readonly HashSet<IUpdateListener> _updateListeners = new();
        private readonly HashSet<IFixedUpdateListener> _fixedUpdateListeners = new();
        private readonly HashSet<IExitListener> _exitListeners = new();
        private readonly HashSet<ISubscriber> _subscribers = new();

        private bool _isGameBooted;


        private async void Awake()
        {
            _initializeListeners();

            await _notifyGameInitialize();
            await _notifyGameLoad();

            await UniTask.WaitUntil(() => InstanceFinder.IsServerStarted || InstanceFinder.IsClientStarted);

            await _notifySubscribe();
            await _notifyGameStart();

            _isGameBooted = true;
        }

        private void Update()
        {
            if (_isGameBooted)
            {
                _notifyGameUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_isGameBooted)
            {
                _notifyGameFixedUpdate(Time.fixedDeltaTime);
            }
        }

        private void OnApplicationQuit()
        {
            if (_isGameBooted)
            {
                _notifyGameExit();
            }
        }

        public async void AddSpawnableListener(IGameListener listener)
        {
            if (!_listeners.Add(listener))
            {
                return;
            }

            Log.Info($"AddSpawnableListener {listener.GetType().Name}", Color.cyan, this);

            ProfilerMarker marker = new($"AddSpawnableListener: {listener.GetType().Name}");
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

            if (listener is ILoadListener loadListener) await loadListener.GameLoad();

            if (listener is IStartListener startListener) await startListener.GameStart();

            if (listener is IUpdateListener updateListener) _updateListeners.Add(updateListener);

            if (listener is IFixedUpdateListener fixedUpdateListener) _fixedUpdateListeners.Add(fixedUpdateListener);

            if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);

            marker.End();
        }

        public void RemoveSpawnableListener(IGameListener listener)
        {
            if (!_listeners.Remove(listener))
            {
                return;
            }

            if (listener is IUpdateListener updateListener)
            {
                _updateListeners.Remove(updateListener);
            }

            if (listener is IFixedUpdateListener fixedUpdateListener)
            {
                _fixedUpdateListeners.Remove(fixedUpdateListener);
            }

            if (listener is IExitListener exitListener)
            {
                _exitListeners.Remove(exitListener);
            }

            if (listener is ISubscriber subscriber)
            {
                subscriber.Unsubscribe();

                _subscribers.Remove(subscriber);
            }
        }

        private void _initializeListeners()
        {
            List<IGameListener> gameListeners = Container.Instance.GetGameListeners();

            foreach (IGameListener listener in gameListeners)
            {
                if (!_listeners.Add(listener)) continue;

                if (listener is IInitializeListener initListener) _initListeners.Add(initListener);

                if (listener is ISubscriber subscriber) _subscribers.Add(subscriber);

                if (listener is ILoadListener loadListener) _loadListeners.Add(loadListener);

                if (listener is IStartListener startListener) _startListeners.Add(startListener);

                if (listener is IUpdateListener updateListener) _updateListeners.Add(updateListener);

                if (listener is IFixedUpdateListener fixedUpdateListener)
                    _fixedUpdateListeners.Add(fixedUpdateListener);

                if (listener is IExitListener exitListener) _exitListeners.Add(exitListener);
            }
        }

        private async UniTask _notifyGameInitialize()
        {
            ProfilerMarker marker = new("_notifyGameInitialize");
            marker.Begin();

            foreach (IInitializeListener listener in _initListeners)
            {
                await listener.GameInitialize();
            }

            marker.End();
        }

        private async UniTask _notifyGameLoad()
        {
            ProfilerMarker marker = new("_notifyGameLoad");
            marker.Begin();

            foreach (ILoadListener listener in _loadListeners)
            {
                await listener.GameLoad();
            }

            marker.End();
        }

        private async UniTask _notifySubscribe()
        {
            ProfilerMarker marker = new("_notifySubscribe");
            marker.Begin();

            foreach (ISubscriber subscriber in _subscribers)
            {
                await subscriber.Subscribe();
            }

            marker.End();
        }

        private async UniTask _notifyGameStart()
        {
            ProfilerMarker marker = new("_notifyGameStart");
            marker.Begin();

            foreach (IStartListener listener in _startListeners)
            {
                await listener.GameStart();
            }

            marker.End();
        }

        private void _notifyGameUpdate(float deltaTime)
        {
            using (_updateProfilerMarker.Auto())
            {
                foreach (IUpdateListener listener in _updateListeners)
                {
                    Profiler.BeginSample(listener.RuntimeListenerName);

                    listener.GameUpdate(deltaTime);

                    Profiler.EndSample();
                }
            }
        }

        private void _notifyGameFixedUpdate(float fixedDeltaTime)
        {
            using (_fixedUpdateProfilerMarker.Auto())
            {
                foreach (IFixedUpdateListener listener in _fixedUpdateListeners)
                {
                    Profiler.BeginSample(listener.RuntimeListenerName);

                    listener.GameFixedUpdate(fixedDeltaTime);

                    Profiler.EndSample();
                }
            }
        }

        private void _notifyGameExit()
        {
            ProfilerMarker marker = new("_notifyGameExit");
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