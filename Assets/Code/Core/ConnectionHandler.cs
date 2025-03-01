using System;
using FishNet;
using FishNet.Transporting;
using UnityEngine;

namespace Code.Core
{

    public enum EConnectionType
    {
        None,
        Host,
        Client
    }
    
    public class ConnectionHandler : MonoBehaviour
    {
        [SerializeField] private EConnectionType _connectionType;

        private void OnEnable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (ParrelSync.ClonesManager.IsClone())
            {
                InstanceFinder.ClientManager.StartConnection();
            }
            else
            {
                if (_connectionType is EConnectionType.Host)
                {
                    InstanceFinder.ServerManager.StartConnection();
                    InstanceFinder.ClientManager.StartConnection();
                }
                
                if(_connectionType is EConnectionType.Client)
                {
                    InstanceFinder.ClientManager.StartConnection(); 
                }
            }


#endif
            
        }

        private void OnDisable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState is LocalConnectionState.Stopping)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }
}