 /***
  *      ___________________________________ _____________
  *     /   _____/\_   _____/\__    ___/    |   \______   \
  *     \_____  \  |    __)_   |    |  |    |   /|     ___/
  *     /        \ |        \  |    |  |    |  / |    |
  *    /_______  //_______  /  |____|  |______/  |____|
  *            \/         \/
  */

 var
   express = require('express'), // call express
   bodyParser = require('body-parser'),
   mysql = require('mysql'),
   async = require('async'),
   config = require('./config');
   session = require('express-session')
   morgan = require('morgan')
   cookieParser = require('cookie-parser');

var passport = require('passport')
  , LocalStrategy = require('passport-local').Strategy;

var app = express(); // define our app using express

app.use(morgan('dev'))

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
  extended: true
}));


app.use(cookieParser('niemertkenntmich'));
app.use(session());
app.use(passport.initialize());
app.use(passport.session());

passport.serializeUser(function(user, done) {
  console.log("serieal:",user);
  done(null, user);
});

passport.deserializeUser(function(user, done) {
  done(null, user);
});


 //app.use('/', express.static(__dirname));

 //mysql configuration

 var connection = mysql.createConnection(config.db);

 connection.connect();
 var port = process.env.PORT || 10300;
 var router = express.Router();


  passport.use(new LocalStrategy(
    function(username, password, done) {

      connection.query('SELECT player_id, name, password, email, origin, status FROM player WHERE name= ' + connection.escape(username), function(err, rows, fields) {
        if (err) { return done(err); }
        if (rows[0] == null) {
          return done(null, false, { message: 'Incorrect username.' });
        }
        if (password !== rows[0].password) {
          return done(null, false, { message: 'Incorrect password.' });
        }
        return done(null, {id: rows[0].player_id, 
                           name: username, 
                           location: rows[0].origin,
                           email: rows[0].email,
                           status: rows[0].status});
     });
    }
  ));

  app.post('/login',
    passport.authenticate('local', { successRedirect: '/loginsucces',
                                     failureRedirect: '/login'})
  );

  app.use('/loginsucces', function(req, res){
    res.json(req.user);
  });

 /***
  *      ______________________________
  *     /  _____/\_   _____/\__    ___/
  *    /   \  ___ |    __)_   |    |
  *    \    \_\  \|        \  |    |
  *     \______  /_______  /  |____|
  *            \/        \/
  */
 router.get('/getPlayer/:playerID', function(req, res) {
     printLogStart("get player", req);
     var player = req.params.player;

     connection.query('SELECT * FROM player WHERE player_id= ' + connection.escape(player), function(err, rows, fields) {
         if (err) throw err;
         res.json(rows);
         printLogSuccess("Plyer successfully fetched");
     });

 });

   router.get('/player/by-name/:playername', function(req, res) {
     printLogStart("get player by name", req);
     var name = "%" + req.params.playername + "%";

     console.log("hahaha:", req.user);
     console.log(req.session);

     var sql = "SELECT player_id as id, name, origin as location, status " +
                 "FROM player " +
                "WHERE name LIKE ?";

     connection.query(sql, [name], function(err, rows, fields) {
         if (err) throw err;
         res.json(rows);
         printLogSuccess("Plyers successfully fetched");
     });

 });

  router.get('/categories', function(req, res) {
     printLogStart("get categories", req);

     connection.query('SELECT category_id as id, name, description FROM category', function(err, rows, fields) {
         if (err) throw err;
           var result = rows;

           result.map(function(item){
            item.quizies = [];
           });
          
           connection.query('SELECT quiz_id as id, title, description, category_category_id as category FROM quiz', function(err, rows, fields) {
              if (err) throw err;
              rows.forEach(function(quiz){
                result.forEach(function(category){
                  if(category.id == quiz.category){
                    category.quizies.push({id:quiz.id,title:quiz.title,description:quiz.description});
                  }
                });
              });
              res.json(result);
              printLogSuccess("Categories successfully fetched");
            });
     });

 });

  router.get('/quiz/:quizID/questions', function(req,res){
    printLogStart("get questions from quiz "+ req.params.quizID, req);
    var sql = 'SELECT question_id as id, question, hint ' +
            'FROM question '+
            'WHERE quiz_quiz_id = ' + connection.escape(req.params.quizID);

    connection.query(sql, function(err, rows, fields){
      if(err) throw err;
      var questions = rows;

      var getAnswers = function(item, callback){
        item.answers = [];
        var itemsql = 'SELECT a.answer_id as id, a.answer, q.value ' +
                  'FROM answer a ' +
            'INNER JOIN answerofquestion q ' +
                    'ON (a.answer_id = q.answer_answer_id)' +
                 'WHERE q.question_question_id = ' + connection.escape(item.id);
        connection.query(itemsql, function(err, rows, fields){
          if(err) throw err;
          rows.forEach(function(answer){
            if(answer.value == 0){ answer.value = false;}
            else {answer.value = true;}
            item['answers'].push(answer);
          });
          console.log(item);
          callback(null, item);
        });
      };

      async.map(questions, getAnswers, function(err, results){
        console.log("results: " ,results);
        res.json(results);
        printLogSuccess("Questions successfully fetched");
      });
    });
  });


  router.get('/friends', function(req,res){
    printLogStart("get friends for user " + req.params.userID, req);

    var sql = 'SELECT  p.player_id as Id, p.name, p.origin as location, p.status ' +
              'FROM friend f ' +
              'INNER JOIN player p ' +
              'ON (p.player_id = f.player_friend_id) ' +
              'WHERE f.player_player_id = ' + connection.escape(req.user.id);

    connection.query(sql, function(err, rows, fields){
      if(err) throw err;
      res.json(rows);
      printLogSuccess("Friends successfully fetched");
    });
  });

	router.get('/rankings', function(req, res) {
		printLogStart("get rankings", req);

		connection.query('SELECT x.name as name, y.points as points '+
						'FROM Quizio.player as x '+
						'INNER JOIN Quizio.ranking as y '+
						'ON x.player_id = y.player_id '+
						'ORDER BY y.points DESC;', function(err, rows, fields) {

			if (err) throw err;
			var result = rows;
			var count = 0;
			result.map(function(item){
				item.position = ++count;
			});
			res.json(result);
			printLogSuccess("Rankings successfully fetched");
        });

	});

	router.get('/notifications', function(req,res){
    printLogStart("get notifications for user", req);
    var sql = 	'SELECT p2.name AS FromFriend , n.message AS Message '+
    			'FROM player p '+
    			'JOIN friend f '+
    			'ON (p.player_id = f.player_player_id) '+
    			'JOIN player p2 '+
    			'ON f.player_friend_id = p2.player_id '+
    			'JOIN notification n '+
    			'ON p2.player_id = n.player_player_id '+
    			'WHERE p.player_id ='+connection.escape(req.user.id)+
    			' ORDER BY n.date DESC LIMIT 10';

    connection.query(sql, function(err, rows, fields){
      if(err) throw err;
      res.json(rows);
      printLogSuccess("Notifications successfully fetched");
    });
  });




 /***
  *    __________________    ____________________
  *    \______   \_____  \  /   _____/\__    ___/
  *     |     ___//   |   \ \_____  \   |    |
  *     |    |   /    |    \/        \  |    |
  *     |____|   \_______  /_______  /  |____|
  *                      \/        \/
  */


 router.post('/player', function(req, res) {
     printLogStart("insert player", req);
     var playerName = req.body.name;
     var playerPW = req.body.password;
     var playerEmail = req.body.email;
     var playerOrigin = req.body.origin;

     var sql = "INSERT INTO player  ('player_id', 'name', 'password', 'email', 'origin')" +
                    "VALUES (null, " + connection.escape(playerName) + ", " + connection.escape(playerPW) + ", " + connection.escape(playerEmail) + ", " + connection.escape(playerOrigin) +")";

     connection.query(sql, function(err, rows, fields) {
         if (err) throw err;
         res.json({
             status: "OK",
             affectedRows: rows.affectedRows,
         });
         printLogSuccess("player successfully added");
     });
 });

  router.post('/friend', function(req, res) {
    printLogStart("insert friend", req);
    var playerId = req.user.id;
    var friendId = parseInt(req.body.friendId, 10);

    console.log(req.body);

    var sql = "INSERT INTO friend (player_player_id, player_friend_id) " +
                   "VALUES (" + connection.escape(playerId) + ", " + connection.escape(friendId) +")";


    connection.query(sql, function(err, rows, fields) {
      if(err) {
        if (err.code == 'ER_DUP_ENTRY') {
          console.log(err);
          res.status(409).send({error: "You have this friend already added!"});
          err = null;
        } else throw err;
      } else {
        res.json({
        status: "OK",
        affectedRows: rows.affectedRows,
        });
        printLogSuccess("friend successfully added");
      }
    });
  });



 /***
  *    __________ ____ ______________
  *    \______   \    |   \__    ___/
  *     |     ___/    |   / |    |
  *     |    |   |    |  /  |    |
  *     |____|   |______/   |____|
  *
  */

 router.put('/player/:id', function(req, res) {
     printLogStart("updating player data", req);
     var playerID = req.params.id;
     var sql = "UPDATE player SET name = 'Hans' WHERE questionID =" + connection.escape(playerID);

     connection.query(sql, function(err, rows, fields) {
         if (err) throw err;
         res.json({
             status: "OK",
             affectedRows: rows.affectedRows,
             changedRows: rows.changedRows
         });
         printLogSuccess("Player successfully updated");
     });

 });


 router.put('/ranking', function(req, res) {
     printLogStart("updating ranking data", req);
     var playerID = req.user.id;
     var toAdd = req.body.toAdd | 0;
     console.log("Player: ", playerID);

     var sqlSelect = "SELECT points FROM ranking WHERE player_id = " + connection.escape(playerID);

     connection.query(sqlSelect, function(err, rows, fields){
     	if (err) throw err;
     	var oldPoints = rows[0].points | 0;
     	var newPoints = oldPoints + toAdd;

     	var sqlUpdate = "UPDATE ranking SET points = " + connection.escape(newPoints) + 
     					" WHERE player_id = " + connection.escape(playerID);

     	connection.query(sqlUpdate, function(err, rows, fields) {
        if (err) throw err;
        res.json({
            status: "OK",
            affectedRows: rows.affectedRows,
            changedRows: rows.changedRows
        });
        printLogSuccess("Ranking successfully updated");
     	});

     });

 });

 //DELETE

  router.delete('/friend', function(req, res) {
    printLogStart("delte friend data", req);
    var playerId = req.user.id;
    var friendId = parseInt(req.body.friendId, 10);

    var sql = "DELETE FROM friend " + 
                    "WHERE player_player_id = ? " + 
                      "AND player_friend_id = ?";

    var input = [playerId, friendId]

    connection.query(sql, input, function(err, rows, fields){
      if (err) throw err;
      res.json({
        status: "OK",
        affectedRows: rows.affectedRows
      });
      printLogSuccess("friend successfully deleted");
    });
  });


 // REGISTER OUR ROUTES -------------------------------
 // all of our routes will be prefixed with /api
 app.use('/', router);

 // START THE SERVER
 // =============================================================================
 app.listen(port);
 console.log('Server is running on port ' + port);


 function printLogSuccess(text) {
     console.log("|");
     console.log(" ---------> " + text + "\n");
 }

 function printLogStart(text, req) {
     if (JSON.stringify(req.body) == "{}")
         console.log("Request to: " + text);
     else
         console.log("Request to: " + text + " with the following data: " + JSON.stringify(req.body));

 }


function isLoggedIn(req, res, next){
  if(req.isAuthenticated()){
    return next();
  }
  else {
    res.status(403).send({error: "You have no permission! Please log in!"});
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
