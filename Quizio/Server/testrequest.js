var request = require('request');

var requrl = "http://localhost:10300/login"
var jseron = {"username":"tester", "password":"test"};

request({
  method: 'POST',
  url: requrl,
  json: jseron
  },
  function(err, res, body){
    if(err){
      throw Error(err);
    }
    console.log(res.statusCode, body);
  });