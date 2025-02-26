using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Core
{
    public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private NetworkPrefabRef _heroPrefab;
        
        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();

        private NetworkRunner _runner;

        private bool _mouseButton0;
        private bool _mouseButton1;
        
        private void Update()
        {
            _mouseButton0 |= Input.GetMouseButton(0);
            _mouseButton1 |= Input.GetMouseButton(1);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            NetworkInputData data = new();

            /*float x  =*/
            
            data.direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
            _mouseButton0 = false;
           
            data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _mouseButton1);
            _mouseButton1 = false;

           
            
            input.Set(data);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player.PlayerId} joined the game.");

            if (runner.IsServer || runner.GameMode == GameMode.Single)
            {
                Vector3 spawnPosition = new(player.RawEncoded % runner.Config.Simulation.PlayerCount * 3, 1, 0);
            
                NetworkObject networkPlayerObject = runner.Spawn(_heroPrefab, spawnPosition, Quaternion.identity, player);
                
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player.PlayerId} left the game.");

            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
            
                _spawnedCharacters.Remove(player);
            }
        }
        
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            Debug.LogWarning($"Input missing for player {player.PlayerId}.");
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected to server.");
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log($"Server shutting down. Reason: {shutdownReason}");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            Debug.LogError($"Disconnected from server. Reason: {reason}");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
            Debug.Log("Received a connection request.");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.LogError($"Connection failed to {remoteAddress}. Reason: {reason}");
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            Debug.Log("Received a user simulation message.");
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.Log("Session list updated.");
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            Debug.Log("Received custom authentication response.");
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log("Host migration in progress.");
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("Scene load complete.");
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            Debug.Log("Scene loading started.");
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            Debug.Log($"Object {obj.Id} exited AOI of player {player.PlayerId}.");
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            Debug.Log($"Object {obj.Id} entered AOI of player {player.PlayerId}.");
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
            Debug.Log($"Reliable data received from player {player.PlayerId} with key {key}.");
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            Debug.Log($"Reliable data transfer in progress for player {player.PlayerId}. Progress: {progress * 100}%");
        }

        private async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            Debug.Log($"Start game {mode}");
        }

        private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
                {
                    StartGame(GameMode.Host);
                }

                if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
                {
                    StartGame(GameMode.Client);
                }

                if (GUI.Button(new Rect(0, 80, 200, 40), "Single"))
                {
                    StartGame(GameMode.Single);
                }
            }
        }
    }
}