const getAttributes = require('./getAttributes');

function getRegex(){
    return /<repeat(?:\s([^>]*))>(.+?)<\/repeat>/gis;
}

function replace(strMatch) {
    const arrMatches = getRegex().exec(strMatch);

    if(arrMatches.length < 1)
    {
        return  strMatch;
    }

    const attributes = getAttributes(arrMatches[1]);
    const includeRepeat = [];
    const intCount = attributes["count"] ?? 1;
    const arrWords = attributes["words"]?.split(',') ?? null;
    const strHtml = arrMatches[2];
    const intMod = attributes["index"] ?? 1;

    for( let i = 0; i < intCount; i++ )
    {
        const strWord = arrWords && arrWords[i] ? arrWords[i] : '';
        includeRepeat.push( strHtml.replace(/{i}/gi, (i + parseInt(intMod))).replace(/{word}/gi, strWord) );
    }

    return includeRepeat.join('');
}


function bsRewrite() {
    return {
        match: getRegex(),
        fn: (_req, _res, strMatch) =>
        {
            return replace(strMatch);
        }
    }
}

module.exports = {bsRewrite, replace, getRegex};
