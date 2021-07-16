chrome = (chrome && chrome.i18n && chrome) || (browser && browser.i18n && browser);

chrome.runtime.onMessage.addListener(function (msg, sender, responseCallback) {
	if (sender.tab) {
		console.log("message from content script, msg=" + msg);
	}
	else {
		if (msg.command !== undefined) {
			var command = g1ContentCommands[msg.command];
			if (command !== undefined) {
				command(msg.args, responseCallback);
			}
			else {
				responseCallback(false, { "error": "Command is not implemented" });
			}
		}
	}
});


