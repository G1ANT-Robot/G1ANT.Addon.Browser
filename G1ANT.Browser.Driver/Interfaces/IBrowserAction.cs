using G1ANT.Browser.Driver.Data;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Interfaces
{
    public interface IBrowserActionCallback
    {
        [OperationContract(IsOneWay = true)]
        void CommandFinished(ActionResponse response);
    }

    [ServiceContract(Name = "CommandExecutor", CallbackContract = typeof(IBrowserActionCallback))]
    public interface IBrowserAction
    {
        [OperationContract]
        ActionResponse Execute(ActionBase command);

        [OperationContract]
        void ExecuteAsync(ActionBase command);

        [OperationContract]
        bool IsConnected();

    }
}
