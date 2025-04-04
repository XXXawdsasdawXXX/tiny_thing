using Core.Network;
using UI.Windows.Base;
using UnityEngine;

namespace UI.Windows.Connection
{
    public class ConnectionWindowController : UIWindowController<ConnectionWindowView>
    {
        [SerializeField] private ConnectionHandler _connectionHandler;

        private void Awake()
        {
            view.TextUserIP.SetText("your ip: " + ConnectionHandler.GetLocalIPAddress());
          
            view.InputFieldHostIP.SetTextWithoutNotify(_connectionHandler.LastJoinedIP);
            
            view.Open();
        }

        protected override void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                view.ButtonHost.Clicked += ButtonHostOnClicked;
                view.ButtonClient.Clicked += ButtonClientOnClicked;
            }
            else
            {
                view.ButtonHost.Clicked -= ButtonHostOnClicked;
                view.ButtonClient.Clicked -= ButtonClientOnClicked;
            }
        }

        private void ButtonClientOnClicked()
        {
            _connectionHandler.ConnectAsClient(view.InputFieldHostIP.Value);
            
            view.Close();
        }

        private void ButtonHostOnClicked()
        {
            _connectionHandler.StartHost();
            
            view.Close();
        }
    }
}