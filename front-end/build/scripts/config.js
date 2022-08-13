// Needed so that we can build our folder paths and not worry about cross platorm issues
const path = require('path');

// Folder names for all our src asset types
// We will use these to build our full paths to use in task files
// This way we only have to change these up here once.
// Add any more you may need - these are just needed for initial tasks

const folderNames = {
	src: 'src',
	dist: 'dist',
	sass: 'styles',
	js: 'scripts',
	img: 'static/img',
	favicons: 'static/favicons',
	svg: 'static/svg',
	fonts: 'static/fonts',
	partials: 'views/partials',
	views: 'views'
};

module.exports = {

	// Main base paths
	mainDir: folderNames.src,
	distDir: folderNames.dist,

	// Css paths
	sassDir: path.join(folderNames.src, folderNames.sass),

	// JS paths
	jsDir: path.join(folderNames.src, folderNames.js),

	// Image paths
	imgDir: path.join(folderNames.src, folderNames.img),
	imgDistDir: path.join(folderNames.dist, folderNames.img),
	svgDir: path.join(folderNames.src, folderNames.svg),
	svgDistDir: path.join(folderNames.dist, folderNames.svg),
	faviconsDir: path.join(folderNames.src, folderNames.favicons),
	faviconsDistDir: path.join(folderNames.dist, folderNames.favicons),

	// Font paths
	fontDir: path.join(folderNames.src, folderNames.fonts),
	fontDistDir: path.join(folderNames.dist, folderNames.fonts),

	// Static html view path
  	viewsDir: path.join(folderNames.src, folderNames.views),
	partialsHtmlDir: path.join(folderNames.src, folderNames.partials)

};
