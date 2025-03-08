using FishNet;
using FishNet.Object;
using UnityEngine;

namespace Core.GameLoop
{
    public class NetworkSpawnTracker : MonoBehaviour
    {
        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {
         
        }

        private void OnNetworkObjectSpawned(NetworkObject obj)
        {
            Debug.Log($"[TRACKER] Заспавнен объект: {obj.name} (ID: {obj.ObjectId})");
        }

        private void OnNetworkObjectDespawned(NetworkObject obj)
        {
            Debug.Log($"[TRACKER] Удалён объект: {obj.name} (ID: {obj.ObjectId})");
        }
    }
}