const rewrites = require("./bs-rewrites/index");
const mkdirp = require('mkdirp');

function getFiles(dir) {
  let paths = [];
  const files = fs.readdirSync(dir);

  files.forEach(file => {
    const path = `${dir}/${file}`;
    if(fs.statSync(path).isDirectory()) {
      let folderName = path.split('/').at(-1);
      if (folderName !== "partials") {
        paths = paths.concat(getFiles(path));
      }
    }
    else if(path.endsWith('.html')) {
      paths.push({
        name: file,
        path: path
      });
    }
  });

  return paths;
}


function run(entryPoint, outDir, outPath) {
	let html = fs.readFileSync(entryPoint, 'utf-8');

	html = html.replace(rewrites.views.getRegex(), match => {
    return rewrites.views.replace(match);
  });
  html = html.replace(rewrites.partial.getRegex(), match => {
    return rewrites.partial.replace(match)
  });
  html = html.replace(rewrites.repeat.getRegex(), match => {
    return rewrites.repeat.replace(match)
  });
  html = html.replace(rewrites.lorem.getRegex(), match => {
    return rewrites.lorem.replace(match)
  });

  mkdirp(outDir).then(() => {
    fs.writeFileSync(outPath, html, (err, result) => {});
  });

}

function buildMain() {
  run('./index.html', './dist/html', './dist/html/index.html');	
}

function buildViews() {
  	getFiles('./src/views').forEach(file => {
      const outDir = file.path.replace('src/views', 'dist/html/views').replace(file.name, '');
      run(file.path, outDir, `${outDir}/${file.name}`);
    });
}

function publishHtml () {
  buildMain();
	buildViews();
}

module.exports = {publishHtml};