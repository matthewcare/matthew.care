export default {
  fetch(request) {
    const target = new URL(request.url);
    target.protocol = "https:";
    target.hostname = "matthew.care";
    target.port = "";
    return Response.redirect(target, 308);
  },
};
