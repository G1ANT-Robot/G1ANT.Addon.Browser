importScripts("content_cmds.js", "utils.js");

var g1antCommands = new Object();
var g1antExtensionVersion = "1.0"

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
		command(message)
			.then((result) => {
				SendCommandResponse(message, true, result);
			})
			.catch((error) => {
				SendCommandResponse(message, false, GetErrorResult(error));
			});
		processed = true;
	} 
	else {
		var command = g1ContentCommands[message.Command];
		if (command !== undefined) {
			FindActiveTab()
				.then((tab) => {
					chrome.tabs.sendMessage(tab.id, message, (response) => {
						SendCommandResponse(message, response.succeeded, response.data);
					});
				})
				.catch((error) => {
					SendCommandResponse(message, false, GetErrorResult(error));
				});
			processed = true;
		} 
	}
	return processed;
}

function FindActiveTab() {
	return chrome.tabs.query({ active: true, lastFocusedWindow: true })
		.then((tabs) => {
			if (tabs === undefined || tabs.length == 0) {
				throw ("Cannot find active tab.");
			}
			else if (tabs.length == 1) {
				return (tabs[0]);
			}
			else {
				throw ("More than one tab are active.");
			}
		});
}

function GetSearchQueryFromArgs(args) {
	return new Promise((resolve, reject) => {
		switch (args.By.toLowerCase()) {
			case "title":
				resolve({ title: args.Search });
				break;
			case "url":
				resolve({ url: NormalizeSearchQueryUrl(args.Search) });
				break;
			default:
				throw "by argument is not recognized.";
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

function WaitToCompleteTabLoading(tabToWait) {
	return new Promise((resolve, reject) => {
		chrome.tabs.onUpdated.addListener(function TabUpdated(tabId, changeInfo, tab) {
			if (tab.id === tabToWait.id && tab.status === "complete") {
				chrome.tabs.onUpdated.removeListener(TabUpdated);
				resolve(GetTabResult(tab));
			}
		});
	});
}

g1antCommands["newtab"] = function (message) {
	return chrome.tabs.create({ url: NormalizeUrl(message.Args.Url) })
		.then((createdTab) => {
			if (message.Args.NoWait === true) {
				return GetTabResult(tab);
			}
			else {
				return WaitToCompleteTabLoading(createdTab);
			}
		});
};

g1antCommands["findtab"] = function (message) {
	return GetSearchQueryFromArgs(message.Args)
		.then((searchQuery) =>
			chrome.tabs.query(searchQuery)
				.then((tabs) => {
					if (tabs === undefined || tabs.length == 0)
						throw "Cannot find tab meets search criteria.";
					else if (tabs.length == 1)
						return GetTabResult(tab);
					else
						throw "More than one tab meets search criteria.";
				})
		);
}

g1antCommands["activatetab"] = function (message) {
	return GetSearchQueryFromArgs(message.Args)
		.then((searchQuery) =>
			chrome.tabs.query(searchQuery)
				.then((tabs) => {
					if (tabs === undefined || tabs.length == 0)
						throw "Cannot find tab meets search criteria.";
					else if (tabs.length == 1)
						return chrome.tabs.update(tabs[0].id, { active: true })
							.then((tab) => GetTabResult(tab));
					else
						throw "More than one tab meets search criteria.";
				})
		);
}

g1antCommands["closetab"] = function (message) {
	if (message.Args.TabId === undefined || message.Args.TabId == null) {
		return FindActiveTab()
			.then((tab) => chrome.tabs.remove(tab.id)
				.then(() => { return GetTabResult(tab); })
			)
	} else {
		var tabId = parseInt(message.Args.TabId);
		return chrome.tabs.get(tabId)
			.then((tab) => {
				return chrome.tabs.remove(tabId)
					.then(() => { return GetTabResult(tab); })
			});
	}
}

g1antCommands["getactivetab"] = function (message) {
	return FindActiveTab()
		.then((tab) => {
			return (GetTabResult(tab));
		});
}

g1antCommands["refresh"] = function(message) {
	return FindActiveTab()
		.then((tab) => {
			chrome.tabs.reload(tab.id, { bypassCache: message.Args.BypassCache })
				.then(() => GetTabResult(tab))
		})
}

g1antCommands["seturl"] = function(message) {
	return FindActiveTab()
		.then((tab) => {
			chrome.tabs.update(tab.id, { url: NormalizeUrl(message.Args.Url) })
				.then((tab) => {
					if (message.Args.NoWait === true) {
						return GetTabResult(tab);
					}
					else {
						return WaitToCompleteTabLoading(tab);
					}
				});
		})
}

g1antCommands["ping"] = function (message) {
	return new Promise((resolve, reject) => {
		resolve(GetValueResult(g1antExtensionVersion));
	});
}
