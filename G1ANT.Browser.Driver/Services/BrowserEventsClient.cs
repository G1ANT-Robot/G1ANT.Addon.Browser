using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class BrowserEventsClient :
        BrowserClientBase<IBrowserEvents>,
        IBrowserEvents
    {
        const int Timeout = 1000;

        public void ExtensionConnected()
        {
            var pipeProxy = CreateChannel(Timeout);
            pipeProxy.ExtensionConnected();
        }

        public void ExtensionDisconnected()
        {
            var pipeProxy = CreateChannel(Timeout);
            pipeProxy.ExtensionDisconnected();
        }

        public void TabCreated(BrowserTab tab)
        {
            var pipeProxy = CreateChannel(Timeout);
            pipeProxy.TabCreated(tab);
        }

        public void TabUpdated(BrowserTab tab)
        {
            var pipeProxy = CreateChannel(Timeout);
            pipeProxy.TabUpdated(tab);
        }
    }
}
