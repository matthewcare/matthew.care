function getAttributes(inputString) {
      let attributes = {};
     [...inputString.matchAll(/([\w-]+)="(.*?)"/gi)].forEach(a => attributes[a[1]] = a[2]);

     return attributes;
}

module.exports = getAttributes;