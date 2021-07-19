
chrome = (chrome && chrome.i18n && chrome) || (browser && browser.i18n && browser);

importScripts("background_cmds.js", "utils.js");

chrome.runtime.onInstalled.addListener(({ reason, version }) => {
	if (reason === chrome.runtime.OnInstalledReason.INSTALL) {
		console.log("runtime.onInstalled");
	}
});

chrome.runtime.onStartup.addListener(() => {
	console.log("runtime.onStartup");
	nativeMessaging.SendBrowserEvent("runtime.onStartup", null);
});

chrome.runtime.onSuspend.addListener(() => {
	console.log("runtime.onSuspend");
	nativeMessaging.SendBrowserEvent("runtime.onSuspend", null);
});

chrome.windows.onCreated.addListener((window) => {
	console.log("windows.onCreated");
});

chrome.tabs.onCreated.addListener((tab) => {
	nativeMessaging.SendBrowserEvent("tabs.onCreated", GetTabResult(tab));
});

chrome.tabs.onUpdated.addListener((tabid, changeinfo, tab) => {
	nativeMessaging.SendBrowserEvent("tabs.onUpdated", GetTabResult(tab));
});

let nativeMessaging = function() {
	let hostName = "com.g1ant.chromium.messaging";
	port = chrome.runtime.connectNative(hostName);
	port.onMessage.addListener(onNativeMessage);
	port.onDisconnect.addListener(onDisconnected);
	console.info("Connection to " + hostName + " has been established");
	SendBrowserEvent("extension.connected", null);

	function onNativeMessage(message) {
		console.info("Received message: " + JSON.stringify(message));
		if (ProcessMessage(message) == false)
			console.error("Command " + message.command + " cannot be processed");
	}
	
	function onDisconnected() {
		console.error("Disconnected: " + chrome.runtime.lastError.message);
		SendBrowserEvent("extension.disconnected", null);
		port = null;
	}

	function SendMessage(message, data) {
		var msg = new Object();
		msg.message = message;
		msg.data = data;
		port.postMessage(msg);
	}

	function SendBrowserEvent(eventName, data) {
		SendMessage("browserEvent", {
			"event": eventName,
			"data": data
		});
	}

	return {
		SendMessage: function (message, data) {
			SendMessage(message, data);
		},
		SendBrowserEvent: function (eventName, data) {
			SendBrowserEvent(eventName, data);
		}
	}
}();

