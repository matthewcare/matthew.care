const esbuild = require('esbuild');
const minify = require("terser");
const path = require('path');
const sep = path.sep === '\\' ? '\\\\' : '/';
const regex = new RegExp(`${sep}src${sep}scripts.*`, 'i');

const uikitUtilPlugin = {
  name: 'uikit-util',
  setup(build) {
    build.onResolve({filter: /^uikit-util$/}, args => {
      const alteredPath = args.resolveDir.replace(regex, `${sep}src${sep}scripts${sep}uikit${sep}util${sep}index.js`);
      return {path: alteredPath}
    });
  }
}

const prod = (process.env.NODE_ENV || 'prod') == 'prod'

function getFiles(dir) {
  let paths = [];
  const files = fs.readdirSync(dir);

  files.forEach(file => {
    const path = `${dir}/${file}`;
    if(fs.statSync(path).isDirectory()) {
      paths = paths.concat(getFiles(path));
    }
    else if(path.endsWith('.js')) {
      paths.push({
        name: file,
        path: path
      });
    }
  });

  return paths;
}

function terser(filePath) {
  if (!prod) {
    return;
  }

  const code = fs.readFileSync(filePath, 'utf-8');
  const promise = minify.minify(code, {compress: {drop_console: true}, mangle: true});
  const outPath = filePath.replace(/\.js$/, '.min.js');
  promise.then(result => {
    fs.writeFileSync(filePath, result.code, (err, result) => {});
    fs.rename(filePath, outPath, (err, result) => {});
  }).catch(error => {
    console.log(error);
  });
}

function run(entryPoint, bundle, outDir, outPath) {
    esbuild
    .build({
      entryPoints: [entryPoint],
      sourcemap: !prod,
      bundle: bundle,
      loader: {
        '.svg': 'text'
      },
      outdir: outDir,
      plugins: [uikitUtilPlugin]
    })
    .catch(() => process.exit(1))
    .then(() => terser(outPath));
}

function buildMain() {
  run('./src/scripts/main.js', true, './dist/scripts', './dist/scripts/main.js');
  run('./src/scripts/icons.js', false, './dist/scripts', './dist/scripts/icons.js');
}

function buildViews() {
  getFiles('./src/scripts/views').forEach(file => {
    const outDir = file.path.replace('src/scripts', 'dist/scripts').replace(file.name, '');
    run(file.path, false, outDir, `${outDir}/${file.name}`);
  });
}

function build () {
  buildMain();
  buildViews();
}

module.exports = {build};
