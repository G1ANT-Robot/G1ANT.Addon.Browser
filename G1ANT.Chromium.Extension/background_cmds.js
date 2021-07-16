importScripts("content_cmds.js", "utils.js");

var g1antCommands = new Object();

function SendCommandResponse(message, succeeded, data) {
	nativeMessaging.SendMessage("commandResponse", { 
		"id": message.id, 
		"succeeded": succeeded,
		"data": data
	});
}

function ProcessMessage(message) {
	var processed = false;
	var command = g1antCommands[message.command];
	if (command !== undefined) {
		command(message, (succeeded, data) => {
			SendCommandResponse(message, succeeded, data);
		});
		processed = true;
	} 
	else {
		var command = g1ContentCommands[message.command];
		if (command !== undefined) {
			ProcessActiveTab(
				(tab) => {
					chrome.tabs.sendMessage(tab.id, message, (succeeded, data) => {
						SendCommandResponse(message, succeeded, data);
					});
				},
				(errorMsg) => {
					SendCommandResponse(message, false, GetErrorResult(errorMsg));
				}
			);
			processed = true;
		} 
	}
	return processed;
}

function ProcessActiveTab(successCallback, failedCallback) {
	chrome.tabs.query({active: true}, (tabs) => {
		if (tabs === undefined || tabs.length == 0) {
			failedCallback("Cannot find active tab.");
		}
		else if (tabs.length == 1) {
			successCallback(tabs[0]);
		}
		else {
			failedCallback("More than one tab are active.");
		}
	});
}

function NormalizeSearchQueryUrl(searchQuery) {
	var uri = parseUri(searchQuery);
	var result = "";
	if (uri.protocol == "") {
		result = "*://";
		if (!searchQuery.startsWith("*"))
			result += "*.";
		result += searchQuery;
	}
	else 
		result = searchQuery;
	if (uri.path == "")
		result += "/*";
	else if (uri.path.startsWith("/") && !uri.path.endsWith("*"))
		result += "*";
	return result;
}

function WaitToCompleteTabLoading(tabToWait, responseCallback) {
	chrome.tabs.onUpdated.addListener(function TabUpdated(tabId, changeInfo, tab) {
		if (tab.id === tabToWait.id && tab.status === "complete") {
			chrome.tabs.onUpdated.removeListener(TabUpdated);
			responseCallback(true, GetTabResult(tab));
		}
	});
}

g1antCommands["newtab"] = function (msg, responseCallback) {
	chrome.tabs.create({
		url: NormalizeUrl(msg.args.url)
	}, function(createdTab) {
		if (msg.args.nowait === true) {
			responseCallback(true, GetTabResult(tab));
		}
		else {
			WaitToCompleteTabLoading(createdTab, responseCallback);
		}
	});
};

g1antCommands["activatetab"] = function (msg, responseCallback) {
	var searchQuery;
	switch (msg.args.by.toLowerCase()) {
		case "title":
			searchQuery = { title: msg.args.search };
			break;
		case "url":
			searchQuery = { url: NormalizeSearchQueryUrl(msg.args.search) };
			break;
		default:
			responseCallback(false, { "error": "by argument is not recognized." });
			break;
	}
	if (searchQuery !== undefined) {
		chrome.tabs.query(searchQuery, (tabs) => {
			if (tabs === undefined || tabs.length == 0) {
				responseCallback(false, { "error": "Cannot find tab meets search criteria." });
			}
			else if (tabs.length == 1) {
				chrome.tabs.update(tabs[0].id, { active: true }, (tab) => {
					responseCallback(true, GetTabResult(tab));
				});
			}
			else {
				responseCallback(false, { "error": "More than one tab meets search criteria." });
			}
		});
	}
}

g1antCommands["closetab"] = function (msg, responseCallback) {
	if (msg.args.tabid === undefined || msg.args.tabid == "") {
		ProcessActiveTab(
			(tab) => {
				chrome.tabs.remove(tabs[0].id, () => {
					responseCallback(true, null);
				});
			},
			(errorMsg) => {
				responseCallback(false, GetErrorResult(errorMsg));
			}
		);
	} else {
		(tab) => {
			chrome.tabs.remove(msg.args.tabid, () => {
				responseCallback(true, null);
			});
		};
    }
}

g1antCommands["getactivetab"] = function(msg, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			responseCallback(true, GetTabResult(tab));
		},
		(errorMsg) => {
			responseCallback(false, GetErrorResult(errorMsg));
		}
	);
}

g1antCommands["refresh"] = function(msg, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			chrome.tabs.reload(tab.id, { bypassCache: msg.args.bypasscache }, () => {
				responseCallback(true, GetTabResult(tab));
			});
		},
		(errorMsg) => {
			responseCallback(false, GetErrorResult(errorMsg));
		}
	);
}

g1antCommands["seturl"] = function(msg, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			chrome.tabs.update(tab.id, { url: NormalizeUrl(msg.args.url) }, (tab) => {
				if (msg.args.nowait === true) {
					responseCallback(true, GetTabResult(tab));
				}
				else {
					WaitToCompleteTabLoading(tab, responseCallback);
				}
			});
		},
		(errorMsg) => {
			responseCallback(false, GetErrorResult(errorMsg));
		}
	);
}

g1antCommands["ping"] = function(msg, responseCallback) {
	responseCallback(true, null);
}
