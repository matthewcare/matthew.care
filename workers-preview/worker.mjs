export default {
  async fetch(request, env) {
    const asset = await env.ASSETS.fetch(request);
    if (asset.status !== 200) return asset;

    const headers = new Headers(asset.headers);
    headers.set("Accept-Ranges", "bytes");
    const range = /^bytes=(\d*)-(\d*)$/i.exec(request.headers.get("Range") ?? "");
    const ifRange = request.headers.get("If-Range");
    const validatorMatches = !ifRange ||
      (!ifRange.startsWith("W/") && ifRange === headers.get("ETag")) ||
      ifRange === headers.get("Last-Modified");

    if (request.method !== "GET" || !range || (!range[1] && !range[2]) || !validatorMatches) {
      return new Response(asset.body, { headers });
    }

    const audio = await asset.arrayBuffer();
    const size = audio.byteLength;
    const start = range[1] ? Number(range[1]) : Math.max(0, size - Number(range[2]));
    const end = range[1] && range[2] ? Math.min(size - 1, Number(range[2])) : size - 1;
    headers.delete("Content-Encoding");

    if (start >= size || start > end) {
      headers.set("Content-Range", `bytes */${size}`);
      headers.set("Content-Length", "0");
      return new Response(null, { status: 416, headers });
    }

    headers.set("Content-Range", `bytes ${start}-${end}/${size}`);
    headers.set("Content-Length", String(end - start + 1));
    return new Response(audio.slice(start, end + 1), { status: 206, headers });
  },
};
