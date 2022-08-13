const getAttributes = require('./getAttributes');

const wordsPerSentenceAvg = 24.460;
const wordsPerSentenceStd = 5.080;
const allWords = ['lorem', 'ipsum', 'dolor', 'sit', 'amet', 'consectetur', 'adipiscing', 'elit', 'curabitur', 'vel', 'hendrerit', 'libero', 'eleifend', 'blandit', 'nunc', 'ornare', 'odio', 'ut', 'orci', 'gravida', 'imperdiet', 'nullam', 'purus', 'lacinia', 'a', 'pretium', 'quis', 'congue', 'praesent', 'sagittis',  'laoreet', 'auctor', 'mauris', 'non', 'velit', 'eros', 'dictum', 'proin', 'accumsan', 'sapien', 'nec', 'massa', 'volutpat', 'venenatis', 'sed', 'eu', 'molestie', 'lacus', 'quisque', 'porttitor', 'ligula', 'dui', 'mollis', 'tempus', 'at', 'magna', 'vestibulum', 'turpis', 'ac', 'diam', 'tincidunt', 'id', 'condimentum', 'enim', 'sodales', 'in', 'hac', 'habitasse', 'platea', 'dictumst', 'aenean', 'neque', 'fusce', 'augue', 'leo', 'eget', 'semper', 'mattis',  'tortor', 'scelerisque', 'nulla', 'interdum', 'tellus', 'malesuada', 'rhoncus', 'porta', 'sem', 'aliquet', 'et', 'nam', 'suspendisse', 'potenti', 'vivamus', 'luctus', 'fringilla', 'erat', 'donec', 'justo', 'vehicula', 'ultricies', 'varius', 'ante', 'primis', 'faucibus', 'ultrices', 'posuere', 'cubilia', 'curae', 'etiam', 'cursus', 'aliquam', 'quam', 'dapibus', 'nisl', 'feugiat', 'egestas', 'class', 'aptent', 'taciti', 'sociosqu', 'ad', 'litora', 'torquent', 'per', 'conubia', 'nostra', 'inceptos', 'himenaeos', 'phasellus', 'nibh', 'pulvinar', 'vitae', 'urna', 'iaculis', 'lobortis', 'nisi', 'viverra', 'arcu', 'morbi', 'pellentesque', 'metus', 'commodo', 'ut', 'facilisis', 'felis', 'tristique', 'ullamcorper', 'placerat', 'aenean', 'convallis', 'sollicitudin', 'integer', 'rutrum', 'duis', 'est', 'etiam', 'bibendum', 'donec', 'pharetra', 'vulputate', 'maecenas', 'mi', 'fermentum', 'consequat', 'suscipit', 'aliquam', 'habitant', 'senectus', 'netus', 'fames', 'quisque', 'euismod', 'curabitur', 'lectus', 'elementum', 'tempor', 'risus', 'cras'];

function generateWords(numWords, withPunctuation) {

	numWords = numWords || 100;
	
	let words = [];

	for (let i = 0; i < numWords; i++) {
		const position = Math.floor(Math.random() * allWords.length);
		const word = allWords[position];
		
		if (i > 0 && words[i - 1] === word) {
			i -= 1;			
		} else {
			words[i] = word;
		}
	}

	words[0] = words[0].charAt(0).toUpperCase() + words[0].slice(1);

	if (!withPunctuation) {
		return words.join(' ');
	}

	return punctuate(words).join(' ');
}

function generateSentences(numSentences) {
	
	let sentences = [];
	
	for (let i = 0; i < numSentences; i++) {
		const sentenceLength = getRandomSentenceLength();
		sentences.push(generateWords(sentenceLength, true));
	}
	
	return sentences.join(' ');
}

function punctuate(sentence) {

	const wordLength = sentence.length;
	sentence[wordLength - 1] += '.';
	
	if (wordLength < 4) {
		return sentence;
	}
	
	const numCommas = getRandomCommaCount(wordLength);
	
	for (let i = 0; i <= numCommas; i++) {
		const position = Math.round(i * wordLength / (numCommas + 1));
		
		if (position < (wordLength - 1) && position > 0) {
			sentence[position] += ',';
		}
	}

	sentence[0] = sentence[0].charAt(0).toUpperCase() + sentence[0].slice(1);
	
	return sentence;
};

function getRandomCommaCount(wordLength) {
	const base = 6;	
	const average = Math.log(wordLength) / Math.log(base);
	const standardDeviation = average / base;
	
	return gaussMS(average, standardDeviation);
};

function getRandomSentenceLength() {
	return gaussMS(wordsPerSentenceAvg, wordsPerSentenceStd);
};

function gaussMS(mean, standardDeviation) {
	let gauss = (Math.random() * 2 - 1) + (Math.random() * 2 - 1) + (Math.random() * 2 - 1);
	return Math.round(gauss * standardDeviation + mean);
};

function getRegex() {
	return /<lorem(?:\s([^>]*))?\/>/ig;
}

function replace(strMatch) {
	const arrMatches = getRegex().exec(strMatch);

    if(arrMatches.length < 1)
    {
        return  strMatch;
    }


	function getNumberOfWordsToGenerate() {
		if (!wordCount && !maxWordCount) {
			useSentences = true;
		} else {
			wordsToGenerate = wordCount ? wordCount : Math.floor(Math.random() * (maxWordCount - minWordCount + 1)) + parseInt(minWordCount);
		}
	}

    const attributes = getAttributes(arrMatches[1]);

	const tag = attributes['tag'] ?? false;
	const tagCount = attributes['tag-count'] ?? 1;
	const wordCount = attributes['words'] ?? false;
	const minWordCount = attributes['min-words'] ?? 1;
	const maxWordCount = attributes['max-words'] ?? false;
	const sentenceCount = attributes['sentences'] ?? 1;
	const punctuation = attributes['punctuation'] ?? false;

	let useSentences = false;
	let wordsToGenerate = wordCount;
	let returnValue = "";

	if (tag) {
		for (let i = 0; i < tagCount; i++) {
			getNumberOfWordsToGenerate(wordCount, maxWordCount, minWordCount);
			const words = useSentences ? generateSentences(sentenceCount) : generateWords(wordsToGenerate, punctuation);
			returnValue += `<${tag}>${words}</${tag}>`;
		}
	} else {
		getNumberOfWordsToGenerate(wordCount, maxWordCount, minWordCount);
		returnValue = useSentences ? generateSentences(sentenceCount) : generateWords(wordsToGenerate, punctuation);
	}

	return returnValue;
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
