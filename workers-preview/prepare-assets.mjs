import { cp, readFile, rm, writeFile } from "node:fs/promises";
import { basename } from "node:path";

const source = new URL("../src/MatthewDotCare.Web/App_Data/xStatic/output/2/", import.meta.url);
const assets = new URL("./assets/", import.meta.url);
const headerRules = (await readFile(new URL("_headers", source), "utf8")).trimEnd();
const previewHeaders = `${headerRules}\n\nhttps://:version.:subdomain.workers.dev/*\n  X-Robots-Tag: noindex`;

// Rebuild staging on every push so deleted pages cannot linger.
await rm(assets, { recursive: true, force: true });
await cp(source, assets, {
  recursive: true,
  // Netlify's redirects use production hostnames and Netlify-only syntax.
  filter: (file) => !["_redirects", ".gitkeep"].includes(basename(file))
});
await writeFile(
  new URL("_headers", assets),
  `${previewHeaders}\n`
);
