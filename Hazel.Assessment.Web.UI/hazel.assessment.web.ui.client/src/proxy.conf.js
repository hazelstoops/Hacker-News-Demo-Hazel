const { env } = require('process');
import { environment } from './environments/environment';

//const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
//  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5022';

const target = environment.apiUrl;

const PROXY_CONFIG = [
  {
    context: [
      "/story",
      "/teststory"
    ],
    target,
    secure: false
  },
  {
    "/*": {
      "target": target,
      "secure": false,
      "changeOrigin": true
    }
  }
]



module.exports = PROXY_CONFIG;
