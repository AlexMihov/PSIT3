 /***
  *      ___________________________________ _____________
  *     /   _____/\_   _____/\__    ___/    |   \______   \
  *     \_____  \  |    __)_   |    |  |    |   /|     ___/
  *     /        \ |        \  |    |  |    |  / |    |
  *    /_______  //_______  /  |____|  |______/  |____|
  *            \/         \/
  */

var
  express = require('express'),
  bodyParser = require('body-parser'),
  session = require('express-session'),
  morgan = require('morgan'),
  cookieParser = require('cookie-parser'),
  passport = require('passport'),
  mysql = require('mysql'),
  LocalStrategy = require('passport-local').Strategy,
  config = require('./config');

var app = express(); // define our app using express
var router = express.Router();

app.use(cookieParser('niemertkenntmich'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(session());
app.use(passport.initialize());
app.use(passport.session());
app.use(morgan('dev'));

passport.serializeUser(function(user, done) {
  done(null, user);
});

passport.deserializeUser(function(user, done) {
  done(null, user);
});

var connection = mysql.createConnection(config.db);
connection.connect();

connection.on('error', function(err) {
  console.log(err.code); // 'ER_BAD_DB_ERROR'
  connection = mysql.createConnection(config.db);
  connection.connect();
});

//Polls the mysql db every 3 minutes to keep the connection alive.
setInterval(function(){
  connection.query('SELECT 1', function(err, result){
    if (err) {
      console.log('DB polling failed ', err);
      connection = mysql.createConnection(config.db);
      connection.connect();
    } else {
      console.log('Connection to db open.');
    }
  });
}, 180000)

var port = process.env.PORT || 10300;
var router = express.Router();

myLocalStrat = require('./myLocalStrat').MyLocalStrategy;
myLocalStrat.connect(connection);

passport.use(new LocalStrategy(function(username, password, done){ myLocalStrat.setUp(username, password, done) }));


app.post('/login',
  passport.authenticate('local', { successRedirect: '/loginsucces',
                                   failureRedirect: '/loginfaillure' })
);

app.use('/loginsucces', function(req, res){
  res.json(req.user);
});

app.use('/loginfaillure', function(req, res){
  res.send("403").status(403);
});

app.get('/logout', function(req, res) {
  req.logout();
  res.json({ok:true});
});

require('./routes').router(router, connection);



// START THE SERVER
// =============================================================================
app.use('/', isLoggedIn, router);
app.listen(port);
console.log('Server is running on port ' + port);

function isLoggedIn(req, res, next){
  if(req.isAuthenticated()){
    return next();
  }
  else {
    res.status(403).send({error: 'You have no permission! Please log in!'});
  }
}

 /*
  if (testing) {
      var resetQueries = {
      };
      for (var query in resetQueries) {
          connection.query(resetQueries[query], function(err, results) {
              if (err) throw err;
              printLogSuccess("Table " + query.replace("sqlReset", "") + " successfully reset for testing");
          });

      }


  }
 */
