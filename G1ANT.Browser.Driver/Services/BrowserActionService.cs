using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;
using System.ServiceModel;

namespace G1ANT.Browser.Driver.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public abstract class BrowserActionService : IBrowserAction
    {
        public BrowserActionService()
        {
        }

        public ActionResponse Execute(ActionBase command)
        {
            return ProcessCommand(command);
        }

        public void ExecuteAsync(ActionBase command)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IBrowserActionCallback>();
            ProcessCommandAsync(command, callback);
        }

        public abstract bool IsConnected();

        protected abstract ActionResponse ProcessCommand(ActionBase command);

        protected abstract void ProcessCommandAsync(ActionBase command, IBrowserActionCallback callback);
    }
}
