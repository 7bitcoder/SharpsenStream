const NodeMediaServer = require('node-media-server');
const fetch = require('node-fetch'); // npm install node-fetch
const https = require('https');

const config = {
  rtmp: {
    port: 1935,
    chunk_size: 60000,
    gop_cache: true,
    ping: 30,
    ping_timeout: 60
  },
  http: {
    port: 8000,
    allow_origin: '*'
  }
};

var nms = new NodeMediaServer(config)
nms.run();

// npm install node-fetch
nms.on('prePublish', async (id, StreamPath, args) => {
  try {
	tokenDto = {
		token: args.token
	}
	const httpsAgent = new https.Agent({
      rejectUnauthorized: false,
    });
	const streamName = StreamPath.replace(/^\/live\//, '');
    const response = await fetch(`https://localhost:5001/Stream/${streamName}/authenticate`,
	{
		headers: {
			'Accept': 'application/json, text/plain',
			'Content-Type': 'application/json;charset=UTF-8'
		},
		mode: 'no-cors',
		method: 'POST', 
		body: JSON.stringify(tokenDto),
		agent: httpsAgent
	});
	if (response.status !== 200)
		throw new Exception();
  } catch (error) {
	let session = nms.getSession(id);
	session.reject();
  }
});