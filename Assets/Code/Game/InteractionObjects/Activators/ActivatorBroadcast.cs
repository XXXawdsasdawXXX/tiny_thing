using FishNet.Broadcast;

namespace Game.InteractionObjects.Activators
{
    public struct ActivatorBroadcast : IBroadcast
    {
        public string ObjectID;
        public bool IsActive;
    }
}