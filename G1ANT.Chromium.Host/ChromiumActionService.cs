using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Services;
using G1ANT.Chromium.Host;
using G1ANT.Chromium.Host.Data;
using System.Threading.Tasks;

namespace G1ANT.Chrome.Host
{
    public abstract class ChromiumActionService : BrowserActionService
    {
        protected readonly ChromiumHost host;

        public ChromiumActionService(ChromiumHost host)
        {
            this.host = host;
        }

        protected bool IsExtensionAlive()
        {
            var msg = new ChromiumCommandMessage()
            {
                Command = "ping"
            };
            host.SendMessage(msg.ToJson());
            var response = host.WaitForCommandResponse(msg.Id, 1000);
            if (response == null)
            {
                return false;
            }
            return true;
        }

        protected abstract void ExitExtension();

        protected ChromiumCommandMessage CallCommand(ActionBase command)
        {
            var msg = new ChromiumCommandMessage()
            {
                Command = command.CommandName.ToLower(),
                Args = command.ToJObject()
            };
            host.SendMessage(msg.ToJson());
            return msg;
        }

        public override bool IsConnected()
        {
            var result = IsExtensionAlive();
            if (!result)
                ExitExtension();
            return result;
        }

        protected override ActionResponse ProcessCommand(ActionBase command)
        {
            var result = new ActionResponse();
            if (IsExtensionAlive())
            {
                var msg = CallCommand(command);
                var response = host.WaitForCommandResponse(msg.Id, command.Timeout);
                result.Succeedded = response != null ? response.Succeeded : false;
                result.JsonData = response != null ? response.Data.ToString() : null;
            }
            else
            {
                ExitExtension();
                result.Succeedded = false;
            }
            return result;
        }

        protected override void ProcessCommandAsync(ActionBase command, IBrowserActionCallback callback)
        {
            if (IsExtensionAlive())
            {
                var msg = CallCommand(command);
                Task.Run(() => {
                    var response = host.WaitForCommandResponse(msg.Id, command.Timeout);
                    var result = new ActionResponse()
                    {
                        Succeedded = response != null ? response.Succeeded : false,
                        JsonData = response != null ? response.Data.ToString() : null
                    };
                    callback.CommandFinished(result);
                });
            }
            else
            {
                ExitExtension();
                var result = new ActionResponse()
                {
                    Succeedded = false
                };
                callback.CommandFinished(result);
            }
        }
    }
}
