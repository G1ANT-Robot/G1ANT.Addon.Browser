importScripts("content_cmds.js", "utils.js");

var g1antCommands = new Object();

function SendCommandResponse(message, succeeded, data) {
	nativeMessaging.SendMessage("commandResponse", { 
		"Id": message.Id, 
		"Succeeded": succeeded,
		"Data": data
	});
}

function ProcessMessage(message) {
	var processed = false;
	var command = g1antCommands[message.Command];
	console.log("Processing command: " + message.Command);
	if (command !== undefined) {
		command(message, (succeeded, data) => {
			SendCommandResponse(message, succeeded, data);
		});
		processed = true;
	} 
	else {
		var command = g1ContentCommands[message.Command];
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
	chrome.tabs.query({ active: true, lastFocusedWindow: true }, (tabs) => {
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

function GetSearchQueryFromArgs(args, responseCallback) {
	switch (args.By.toLowerCase()) {
		case "title":
			return { title: args.Search };
		case "url":
			return { url: NormalizeSearchQueryUrl(args.Search) };
		default:
			responseCallback(false, GetErrorResult("by argument is not recognized."));
			return false;
	}
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

g1antCommands["newtab"] = function (message, responseCallback) {
	chrome.tabs.create({
		url: NormalizeUrl(message.Args.Url)
	}, function(createdTab) {
		if (message.Args.NoWait === true) {
			responseCallback(true, GetTabResult(tab));
		}
		else {
			WaitToCompleteTabLoading(createdTab, responseCallback);
		}
	});
};

g1antCommands["findtab"] = function (message, responseCallback) {
	var searchQuery = GetSearchQueryFromArgs(message.Args, responseCallback);
	if (searchQuery !== false) {
		chrome.tabs.query(searchQuery, (tabs) => {
			if (tabs === undefined || tabs.length == 0) {
				responseCallback(false, GetErrorResult("Cannot find tab meets search criteria."));
			}
			else if (tabs.length == 1) {
				console.log("findtab: found");
				responseCallback(true, GetTabResult(tab));
			}
			else {
				responseCallback(false, GetErrorResult("More than one tab meets search criteria."));
			}
		});
	}
}

g1antCommands["activatetab"] = function (message, responseCallback) {
	var searchQuery = GetSearchQueryFromArgs(message.Args, responseCallback);
	if (searchQuery !== false) {
		chrome.tabs.query(searchQuery, (tabs) => {
			if (tabs === undefined || tabs.length == 0) {
				responseCallback(false, GetErrorResult("Cannot find tab meets search criteria."));
			}
			else if (tabs.length == 1) {
				chrome.tabs.update(tabs[0].id, { active: true }, (tab) => {
					responseCallback(true, GetTabResult(tab));
				});
			}
			else {
				responseCallback(false, GetErrorResult("More than one tab meets search criteria."));
			}
		});
	}
}

g1antCommands["closetab"] = function (message, responseCallback) {
	if (message.Args.TabId === undefined || message.Args.TabId == null) {
		ProcessActiveTab(
			(tab) => {
				chrome.tabs.remove(tab.id, () => {
					responseCallback(true, null);
				});
			},
			(errorMsg) => {
				responseCallback(false, GetErrorResult(errorMsg));
			}
		);
	} else {
		chrome.tabs.remove(Number(message.Args.TabId), () => {
			responseCallback(true, null);
		});
    }
}

g1antCommands["getactivetab"] = function(message, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			responseCallback(true, GetTabResult(tab));
		},
		(errorMsg) => {
			responseCallback(false, GetErrorResult(errorMsg));
		}
	);
}

g1antCommands["refresh"] = function(message, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			chrome.tabs.reload(tab.id, { bypassCache: message.Args.BypassCache }, () => {
				responseCallback(true, GetTabResult(tab));
			});
		},
		(errorMsg) => {
			responseCallback(false, GetErrorResult(errorMsg));
		}
	);
}

g1antCommands["seturl"] = function(message, responseCallback) {
	ProcessActiveTab(
		(tab) => {
			chrome.tabs.update(tab.id, { url: NormalizeUrl(message.Args.Url) }, (tab) => {
				if (message.Args.NoWait === true) {
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

g1antCommands["ping"] = function(message, responseCallback) {
	responseCallback(true, null);
}
