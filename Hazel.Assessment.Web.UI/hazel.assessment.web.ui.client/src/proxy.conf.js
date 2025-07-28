const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:5022';

const PROXY_CONFIG = [
  {
    context: [
      "/story"
    ],
    target,
    secure: true
  },
  {
    "/*": {
      "target": target,
      "secure": true,
      "changeOrigin": true
    }
  }
]

module.exports = PROXY_CONFIG;
