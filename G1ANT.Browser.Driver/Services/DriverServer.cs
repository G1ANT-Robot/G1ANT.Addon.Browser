using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Extensions;
using G1ANT.Browser.Driver.Interfaces;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class DriverServer<ServiceT, ContractT> : IDisposable
    {
        private const int maxBufferSize = 50 * 1024 * 1024;
        protected ServiceHost host;
        public ServiceT Service { get; private set; }
        protected abstract string ServerName { get; }

        public DriverServer(ServiceT commandService)
        {
            this.Service = commandService;
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            var serverUrl = $"net.pipe://localhost/{ServerName}";
            host = new ServiceHost(Service, new Uri(serverUrl));

            var binding = new NetNamedPipeBinding()
            {
                MaxBufferSize = maxBufferSize,
                MaxReceivedMessageSize = maxBufferSize,
                MaxBufferPoolSize = maxBufferSize,
            };
            binding.ReaderQuotas.MaxStringContentLength = maxBufferSize;
            var commandExecutorContract = typeof(ContractT);
            host.AddServiceEndpoint(
                commandExecutorContract,
                binding, 
                commandExecutorContract.ServiceContract_Name());

            host.Open();
        }

        public void Stop()
        {
            if (host != null && host.State != CommunicationState.Closed)
                host.Close();
        }
    }
}
