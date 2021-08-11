using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;
using System;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class BrowserEventsClient :
        BrowserClientBase<IBrowserEvents>,
        IBrowserEvents
    {
        private TimeSpan defaultTimeout = TimeSpan.FromSeconds(1);

        public void ExtensionConnected()
        {
            var pipeProxy = CreateChannel(defaultTimeout);
            pipeProxy.ExtensionConnected();
        }

        public void ExtensionDisconnected()
        {
            var pipeProxy = CreateChannel(defaultTimeout);
            pipeProxy.ExtensionDisconnected();
        }

        public void TabCreated(BrowserTab tab)
        {
            var pipeProxy = CreateChannel(defaultTimeout);
            pipeProxy.TabCreated(tab);
        }

        public void TabUpdated(BrowserTab tab)
        {
            var pipeProxy = CreateChannel(defaultTimeout);
            pipeProxy.TabUpdated(tab);
        }
    }
}
