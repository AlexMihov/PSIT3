var request = require('request');

var requrl = "http://localhost:10300/friend"
var jseron = {"playerId":1, "friendId":2};

/*
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
*/

/*
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
*/
/*
var get = "http://localhost:10300/player/by-name/e"
request({
  method: 'GET',
  url: get,
  },
  function(err, res, body){
    if(err){
      throw Error(err);
    }
    console.log(res.statusCode, body);
  });
*/
var reqPostPlayer = 'http://localhost:10300/player';
var json = {'name': 'test', 'password': 'test', 'email':'adam.riese@gmail.com','status': 'Did you know ...?', 'origin':'Zürich'};
request({
  method: 'DELETE',
  url: reqPostPlayer,
  json: json
  },
  function(err, res, body){
    if(err){
      throw Error(err);
    }
    console.log(res.statusCode, body);
  });