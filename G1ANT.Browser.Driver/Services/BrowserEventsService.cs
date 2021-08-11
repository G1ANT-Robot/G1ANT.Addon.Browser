using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BrowserEventsService : IBrowserEvents
    {
        public delegate void ExtensionConnectedHandler();
        public event ExtensionConnectedHandler OnExtensionConnected;

        public delegate void ExtensionDisconnectedHandler();
        public event ExtensionDisconnectedHandler OnExtensionDisconnected;

        public delegate void TabCreatedHandler(BrowserTab tab);
        public event TabCreatedHandler OnTabCreated;

        public delegate void TabUpdatedHandler(BrowserTab tab);
        public event TabUpdatedHandler OnTabUpdated;

        public void ExtensionConnected()
        {
            OnExtensionConnected?.Invoke();
        }

        public void ExtensionDisconnected()
        {
            OnExtensionDisconnected?.Invoke();
        }

        public void TabCreated(BrowserTab tab)
        {
            if (OnTabCreated != null)
                OnTabCreated(tab);
        }

        public void TabUpdated(BrowserTab tab)
        {
            if (OnTabUpdated != null)
                OnTabUpdated(tab);
        }
    }
}
