var g1ContentCommands = new Object();	

function FindElement(search, by, responseCallback) {
	var element = null;
	switch (by.toLowerCase()) {
		case "id":
			element = document.getElementById(search);
			break;
		case "class":
		case "query":
			element = GetElementsByXpath("//*[@class='" + search + "']").singleNodeValue;
			break;
		case "cssselector":
			element = document.querySelector(search);
			break;
		case "tag":
			element = document.getElementsByTagName(search).singleNodeValue;
			break;
		case "xpath":
			element = GetElementsByXpath(search).singleNodeValue;
			break;
		case "name":
			element = document.getElementsByName(search).singleNodeValue;
			break;
		case "jquery":
			break;
	}
	if (element !== undefined)
		return element;
	else if (element == null)
		responseCallback(false, GetErrorResult("Value for argument 'By' was not recognized."));
	else
		responseCallback(false, GetErrorResult("Element does not exist."));
	return null;
}

function GetElementsByXpath(path) {
	return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
}

g1ContentCommands["click"] = function(args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		element.click();
		responseCallback(true, null);
	} 
};

g1ContentCommands["getattribute"] = function(args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		var val = element.getAttribute(args.Name);
		responseCallback(true, GetValueResult(val));
	} 
}

g1ContentCommands["setattribute"] = function (args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		var val = element.setAttribute(args.Name, args.Value);
		responseCallback(true, null);
	}
}

g1ContentCommands["gethtml"] = function(args, responseCallback) {
	var html = document.documentElement.outerHTML;
	responseCallback(true, GetValueResult(html));
}

g1ContentCommands["getouterhtml"] = function(args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		var html = element.outerHTML;
		responseCallback(true, GetValueResult(html));
	} 
}

g1ContentCommands["getinnerhtml"] = function (args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		var html = element.innerHTML;
		responseCallback(true, GetValueResult(html));
	}
}

g1ContentCommands["gettext"] = function(args, responseCallback) {
	var element = FindElement(args.Search, args.By);
	if (element !== null) {
		var text = element.innerText;
		responseCallback(true, GetValueResult(text));
	} 
}
