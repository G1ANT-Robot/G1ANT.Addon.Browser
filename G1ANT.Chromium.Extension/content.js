chrome = (chrome && chrome.i18n && chrome) || (browser && browser.i18n && browser);

chrome.runtime.onMessage.addListener(function (message, sender, responseCallback) {
	if (sender.tab) {
		console.log("message from content script, msg=" + message);
	}
	else {
		if (message.Command !== undefined) {
			var command = g1ContentCommands[message.Command];
			if (command !== undefined) {
				command(message.Args)
					.then((result) => {
						responseCallback(GetContentResponse(true, result));
					})
					.catch((error) => {
						responseCallback(GetContentResponse(false, GetErrorResult(error)));
					});
			}
			else {
				responseCallback(false, GetErrorResult("Command is not implemented"));
			}
		}
	}
	return true;
});

function GetContentResponse(succeeded, data) {
	return {
		"succeeded": succeeded,
		"data": data
	};
}

