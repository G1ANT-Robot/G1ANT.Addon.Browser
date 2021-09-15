var g1ContentCommands = new Object();	

function FindElement(search, by) {
	return new Promise((resolve, reject) => {
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
			return resolve(element);
		else if (element == null)
			throw "Value for argument 'By' was not recognized.";
		else
			throw "Element does not exist.";
	});
}

function GetElementsByXpath(path) {
	return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
}

g1ContentCommands["click"] = function(args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			element.click();
			return null;
		});
};

g1ContentCommands["getattribute"] = function(args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			var val = element.getAttribute(args.Name);
			return GetValueResult(val);
		});
}

g1ContentCommands["setattribute"] = function (args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			element.setAttribute(args.Name, args.Value);
			return null;
		});
}

g1ContentCommands["gethtml"] = function (args) {
	return new Promise((resolve, reject) => {
		var html = document.documentElement.outerHTML;
		resolve(GetValueResult(html));
	});
}

g1ContentCommands["getouterhtml"] = function(args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			var html = element.outerHTML;
			return GetValueResult(html);
		});
}

g1ContentCommands["getinnerhtml"] = function (args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			var html = element.innerHTML;
			return GetValueResult(html);
		});
}

g1ContentCommands["gettext"] = function(args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			var text = element.innerText;
			return GetValueResult(text);
		});
}

g1ContentCommands["setfocus"] = function (args) {
	return FindElement(args.Search, args.By)
		.then((element) => {
			element.focus();
			if (document.activeElement == element)
				return null;
			else
				throw "Cannot set focus on element.";
		});
}
