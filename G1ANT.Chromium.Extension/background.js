
chrome = (chrome && chrome.i18n && chrome) || (browser && browser.i18n && browser);

importScripts("background_cmds.js", "utils.js");

chrome.runtime.onInstalled.addListener(({ reason, version }) => {
	if (reason === chrome.runtime.OnInstalledReason.INSTALL) {
		console.log("runtime.onInstalled");
	}
});

chrome.runtime.onStartup.addListener(() => {
	console.log("runtime.onStartup");
	SendBrowserEvent("runtime.onStartup", null);
});

chrome.runtime.onSuspend.addListener(() => {
	console.log("runtime.onSuspend");
	SendBrowserEvent("runtime.onSuspend", null);
});

chrome.windows.onCreated.addListener((window) => {
	console.log("windows.onCreated");
});

chrome.tabs.onCreated.addListener((tab) => {
	SendBrowserEvent("tabs.onCreated", GetTabResult(tab));
});

chrome.tabs.onUpdated.addListener((tabid, changeinfo, tab) => {
	SendBrowserEvent("tabs.onUpdated", GetTabResult(tab));
});

function SendBrowserEvent(eventName, data) {
	if (nativeMessaging !== undefined) {
		nativeMessaging.SendMessage("browserEvent", {
			"event": eventName,
			"data": data
		});
	}
}

let nativeMessaging = function() {
	let hostName = "com.g1ant.chromium.messaging";
	port = chrome.runtime.connectNative(hostName);
	port.onMessage.addListener(onNativeMessage);
	port.onDisconnect.addListener(onDisconnected);
	console.info("Connection to " + hostName + " has been established");

	function onNativeMessage(message) {
		console.info("Received message: " + JSON.stringify(message));
		if (ProcessMessage(message) == false)
			console.error("Command " + message.command + " cannot be processed");
	}
	
	function onDisconnected() {
		console.error("Failed to connect: " + chrome.runtime.lastError.message);
		port = null;
	}
	
	return {
		SendMessage: function (message, data) {
			var msg = new Object();
			msg.message = message;
			msg.data = data;
			port.postMessage(msg);
		}
	}
}();

