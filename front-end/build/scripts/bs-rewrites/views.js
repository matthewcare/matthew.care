const fs = require('fs');
const getAttributes = require('./getAttributes');
const path = require('path');

let folderLevel = 0;

const prod = (process.env.NODE_ENV || 'prod') == 'prod'

function getRegex() {
  return /<views(?:\s([^>]*))?\/>/ig;
}

function getFiles(dir) {
  const files = fs.readdirSync(dir);
  let html = "";

  files.forEach(file => {
    const filePath = `${dir}/${file}`;
    if(fs.statSync(filePath).isDirectory()) {
      folderLevel++;
      let folderName = filePath.split('/').at(-1);
      if (folderName === "partials") {
        folderLevel--;
        return;
      }
      html += `<li><h3>${folderName}</h3><ul class="uk-list">`;
        html += getFiles(filePath);
        html += "</ul></li>"
        folderLevel--;
    }
    else if(filePath.endsWith('.html')) {
      let path = filePath.substring(1);
      path = prod ? path.replace("/src", "") : path;
      const newItem = `<li><a href="${path}" target="_blank">${file}</a></li>`;
      if (folderLevel === 0) {
        html = `${newItem}${html}`;
      } else {
        html += newItem;
      }
    }
  });

  return html;
}

function replace(strMatch) {
  const arrMatches = getRegex().exec(strMatch);

  if(arrMatches.length < 1)
  {
    return "";
  }

  let html = `<ul class="uk-list">${getFiles('./src/views')}</ul>`;

  return html;
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
