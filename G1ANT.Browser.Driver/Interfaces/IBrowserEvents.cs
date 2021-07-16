using G1ANT.Browser.Driver.Data;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Interfaces
{
    [ServiceContract(Name = "BrowserEvents")]
    public interface IBrowserEvents
    {
        [OperationContract]
        void ExtensionConnected();

        [OperationContract]
        void ExtensionDisconnected();

        [OperationContract]
        void TabCreated(BrowserTab tab);

        [OperationContract]
        void TabUpdated(BrowserTab tab);
    }
}
