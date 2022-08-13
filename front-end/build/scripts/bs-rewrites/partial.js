const fs = require('fs');
const getAttributes = require('./getAttributes');

function getRegex() {
	return /<partial(?:\s([^>]*))?\/>/ig;
}

function replace(strMatch) {
	const arrMatches = getRegex().exec(strMatch);

    if(arrMatches.length < 1)
    {
        return "";
    }

    const attributes = getAttributes(arrMatches[1]);

    const file = attributes["name"] ?? false;
    const replacements = attributes["replace"] ?? false;

	if (!file) {
		console.log("Include failed, file not specified")
		return "";
	}

	try {
		let data = fs.readFileSync(`./src/views/partials/${file}.html`, "utf8");

		if (replacements) {
			replacements.split(';').filter(x => x).forEach(r => {
				const keyValue = r.split(':');
				data = data.replace(new RegExp(`{${keyValue[0].trim()}}`, 'gi'), keyValue[1].trim());
			});
		}

		return data;
	}
	catch (error) {
		console.log(`Include failed with error: ${error}`);
		return "";
	}

	return "";
}

function bsRewrite() {
	return {
		match: getRegex(),
		fn: (_req, _res, strMatch) => {
			return replace(strMatch);
		}
	}
}

module.exports = {bsRewrite, replace, getRegex};
