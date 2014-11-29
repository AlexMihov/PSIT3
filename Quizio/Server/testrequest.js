var request = require('request');

var requrl = "http://localhost:10300/friend"
var jseron = {"playerId":1, "friendId":2};

request({
  method: 'DELETE',
  url: requrl,
  json: jseron
  },
  function(err, res, body){
    if(err){
      throw Error(err);
    }
    console.log(res.statusCode, body);
  });

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