/**
 * Build config: Copy - If a production directory path has been set, copy dist folder there.
 *
 * @category Build Scripts
 * @module build/scripts/copy
 *
 * @author Matthew Care
 *
 * @name buildCopy
 *
 * @since 1.0.0
 * @version 1.0.0
 *
 * @example node ./build/scripts/copy.js
 *
 * @requires NPM:fs-extra
 * @requires NPM:log-symbols
 * @requires /build/config
 */

// Dependencies
// =============================================================================

// Native
const fs = require('fs-extra');
const logSymbols = require('log-symbols');

const config = require('./config');

/**
 * Check if the productionDir path has been set, if so copy the dist folder there.
 * This should be the CMS folder. This task only runs when you do a production build.
 *
 * @example node -e 'require(\"./build/scripts/copy\").copyDist()'
 *
 */
function copyDist() {
    if (config.productionDir !== '') {
        // Copy dist to productionDir
        fs.copy(config.distDir, config.productionDir, err => {
            if (err) {
                console.log(logSymbols.error, 'An error occured while copying the dist folder.');
                return console.error(logSymbols.error, err);
            }

            console.log(logSymbols.success, 'dist folder copied successfully');
        });
    } else {
        console.error(logSymbols.error, 'No production directory has been set. Please set this in the /build/scripts/config file (serverSettings.productionDir)');
    }
}

/**
 * Copy the contents of the src/favicons folder to the dist folder. This runs on a dev build.
 * When watching files, this task will also be run with any changes to src/favicons directory.
 *
 * @example node -e 'require(\"./build/scripts/copy\").copyFavicons()'
 *
 */
function copyFavicons() {
    fs.copy(config.faviconsDir, config.faviconsDistDir, err => {
        if (err) {
            console.log(logSymbols.error, 'An error occured while copying the favicons folder.');
            return console.error(logSymbols.error, err);
        }

        console.log(logSymbols.success, 'Favicons folder copied successfully');
    });
}

/**
 * Copy the contents of the src/fonts folder to the dist folder. This runs on a dev build.
 * When watching files, this task will also be run with any changes to src/fonts directory.
 *
 * @example node -e 'require(\"./build/scripts/copy\").copyFonts()'
 *
 */
function copyFonts() {
    fs.copy(config.fontDir, config.fontDistDir, err => {
        if (err) {
            console.log(logSymbols.error, 'An error occured while copying the fonts folder.');
            return console.error(logSymbols.error, err);
        }

        console.log(logSymbols.success, 'Fonts folder copied successfully');
    });
}


/**
 * Copy the contents of the src/img folder to the dist folder. This runs on a dev build.
 * When watching files, this task will also be run with any changes to src/img directory.
 *
 * @example node -e 'require(\"./build/scripts/copy\").copyImages()'
 *
 */
function copyImages() {
    fs.copy(config.imgDir, config.imgDistDir, err => {
        if (err) {
            console.log(logSymbols.error, 'An error occured while copying the images folder.');
            return console.error(logSymbols.error, err);
        }

        console.log(logSymbols.success, 'Images folder copied successfully');
    });
}

// Exports
// =============================================================================

module.exports = {copyFavicons, copyFonts, copyDist, copyImages};
