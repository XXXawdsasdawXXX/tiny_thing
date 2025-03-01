using System;
using System.Net;
using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
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
        [SerializeField] private string _serverIP = "192.168.1.100"; // Ввести IP сервера вручную
        [SerializeField] private ushort _port = 7777;
        private void OnEnable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (ParrelSync.ClonesManager.IsClone())
            {
                ConnectAsClient();
            }
            else
            {
                if (_connectionType == EConnectionType.Host)
                {
                    StartHost();
                }
                else if (_connectionType == EConnectionType.Client)
                {
                    ConnectAsClient();
                }
            }
#endif
        }

        private void ConnectAsClient()
        {
            // Получаем TugboatTransport (или другой транспорт)
            if (InstanceFinder.NetworkManager.TransportManager.Transport is Tugboat tugboat)
            {
                tugboat.SetClientAddress(_serverIP);
                tugboat.SetPort(_port);
            }

            InstanceFinder.ClientManager.StartConnection();
            Debug.Log($"[Client] Подключение к серверу {_serverIP}:{_port}");
        }

        private void StartHost()
        {
            if (InstanceFinder.NetworkManager.TransportManager.Transport is Tugboat tugboat)
            {
                tugboat.SetClientAddress(GetLocalIPAddress());
                tugboat.SetPort(_port);
            }

            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
            Debug.Log($"[Host] Сервер запущен на {GetLocalIPAddress()}:{_port}");
        }
        private void OnDisable()
        {
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Stopping)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
        
        public static string GetLocalIPAddress()
        {
            string localIP = "127.0.0.1"; // По умолчанию
            foreach (var ip in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
    
    public class NetworkUtils : MonoBehaviour
    {
        public static string GetLocalIPAddress()
        {
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString(); // Возвращаем локальный IP
                }
            }
            return "127.0.0.1"; // Если не найден IP
        }

        private void Start()
        {
            Debug.Log($"[Server] Локальный IP: {GetLocalIPAddress()}");
        }
    }
}
