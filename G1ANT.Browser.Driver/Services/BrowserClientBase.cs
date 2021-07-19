using G1ANT.Browser.Driver.Extensions;
using System;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class BrowserClientBase<T> 
    {
        protected abstract string ServerName { get; }
        protected string ServerEndpoint => $"net.pipe://localhost/{ServerName}/{typeof(T).ServiceContract_Name()}";

        protected T CreateChannel(TimeSpan timeout)
        {
            var binding = new NetNamedPipeBinding()
            {
                SendTimeout = timeout
            };

            ChannelFactory<T> pipeFactory = null;
            if (typeof(T).ServiceContract_HasCallbackContract())
            {
                pipeFactory = new DuplexChannelFactory<T>(
                    new InstanceContext(this),
                    binding,
                    new EndpointAddress(ServerEndpoint));
            }
            else
            {
                pipeFactory = new ChannelFactory<T>(
                    binding,
                    new EndpointAddress(ServerEndpoint));
            }
            return pipeFactory.CreateChannel();
        }
    }
}
