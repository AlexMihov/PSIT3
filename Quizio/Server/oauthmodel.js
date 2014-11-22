var mysql = require('mysql'),
    config = require('./config'),
    connection = mysql.createConnection(config.db),
    model = module.exports;


model.getAccessToken = function (bearerToken, callback) {
  var sql = 'SELECT access_token, client_id, expires, user_id FROM oauth_access_tokens ' +
             'WHERE access_token = ' + connection.escape(bearerToken);
  connection.query(sql, function(err, rows, fields){
    console.log('Result rows: ',rows);
    var token = rows[0];
    callback(null, {
      accessToken: token.access_token,
      clientId: token.client_id,
      expires: token.expires,
      userId: token.userId
    });
  });
};

model.getClient = function (clientId, clientSecret, callback) {
  var sql = 'SELECT client_id, client_secret, redirect_uri FROM oauth_clients WHERE ' +
      'client_id = ' + connection.escape(clientId);

  connection.query(sql, function(err, rows, fields){
    var client = rows[0];

    if (clientSecret !== null && client.client_secret !== clientSecret) return callback();

    callback(null, {
      clientId: client.client_id,
      clientSecret: client.client_secret
    });
  });
};

model.getRefreshToken = function (bearerToken, callback) {
  var sql = 'SELECT refresh_token, client_id, expires, user_id FROM oauth_refresh_tokens ' +
        'WHERE refresh_token =' + connection.escape(bearerToken);
  connection.query(sql, function(err, rows, fields){
    var token = rows[0];
    callback(err, token);
  });
};

model.saveRefreshToken = function (refreshToken, clientId, expires, userId, callback) {
  var sql = 'INSERT INTO oauth_refresh_tokens(refresh_token, client_id, user_id, ' +
        'expires) VALUES (' + connection.escape(refreshToken) + ', ' +
                              connection.escape(clientId) + ', ' + 
                              connection.escape(userId) + ', ' + 
                              connection.escape(expires) + ')';

  connection.query(sql, function(err, rows, fields) {
    if (err) callback(err);
  });
};

model.saveAccessToken = function (accessToken, clientId, expires, userId, callback) {
  sql = 'INSERT INTO oauth_access_tokens(access_token, client_id, user_id, expires) ' +
             'VALUES (' + connection.escape(accessToken) + ', ' +
                          connection.escape(clientId) + ', ' + 
                          connection.escape(userId) + ', ' + 
                          connection.escape(expires) + ')';
  connection.query(sql, function(err, rows, fields) {
    if (err) callback(err);
  });
};

model.getUser = function (username, password, callback) {
  sql = 'SELECT player_id FROM player WHERE name = '+ connection.escape(username) +
                                      ' AND password = ' + connection.escape(username);
  connection.query(sql, function(err, rows, fields) {
    callback(err, rows[0]);
  });
};

model.grantTypeAllowed = function (clientId, grantType, callback) {
  if (grantType === 'password') {
    return callback(null, true);
  }

  callback(false, true);
};