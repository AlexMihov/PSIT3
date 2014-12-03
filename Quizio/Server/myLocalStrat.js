"use strict";

var 
  crypto = require('crypto'),
  mysql = require('mysql'),
  config = require('./config');

var MyLocalStrategy = function(){
  this.connection = mysql.createConnection(config.db);
  
};

MyLocalStrategy.prototype.setUp = function(username, password, done) {

  this.connection.connect(); 

  var sql = 'SELECT player_id, name, password, email, origin, status FROM player WHERE name= ?';

  this.connection.query(sql, [username], function(err, rows, fields) {
    if (err) { return done(err); }

    if (rows[0] == null) {
      return done(null, false, { message: 'Incorrect username.' });
    }
      
    if (hash(password) !== rows[0].password) {
      return done(null, false, { message: 'Incorrect password.' });
    }
      
    return done(null, { id: rows[0].player_id, 
                        name: username, 
                        location: rows[0].origin,
                        email: rows[0].email,
                        status: rows[0].status 
                      });
  });
}

var hash = function(password){
  var sha256 = crypto.createHash("sha256");
  sha256.update(password, "utf8");
  return sha256.digest("base64");
}



exports.MyLocalStrategy = new MyLocalStrategy();