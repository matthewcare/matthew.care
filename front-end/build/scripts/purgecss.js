/**
 * Build config: PurgeCSS - Remove any unused rules from compiled CSS.
 *
 * @see https://www.purgecss.com/cli
 * @see https://www.purgecss.com/configuration
 * @see https://www.purgecss.com/whitelisting
 *
 * @author Matthew Care
 *
 * @version 1.0.0
 */

// Dependencies
// =============================================================================

const config = require('./config');

// Exports
// =============================================================================

module.exports = {
    content: ['./*.html', `${config.viewsDir}/**/*.html`],
    css: [`${config.distDir}/**/*.css`],
    // Add any you may need to this list
    safelist: {
        greedy: [/^uk-icon/]
    },
    keyframes: true,
    defaultExtractor: content => content.match(/[A-z0-9-:\/@]+/g) || []
};
