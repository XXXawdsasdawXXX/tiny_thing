using Code.Core.Network;
using UnityEngine;

namespace Code.UI.Windows.Connection
{
    public class ConnectionWindowController : UIWindowController<ConnectionWindowView>
    {
        [SerializeField] private ConnectionHandler _connectionHandler;

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