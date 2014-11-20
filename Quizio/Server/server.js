 /***
  *      ___________________________________ _____________
  *     /   _____/\_   _____/\__    ___/    |   \______   \
  *     \_____  \  |    __)_   |    |  |    |   /|     ___/
  *     /        \ |        \  |    |  |    |  / |    |
  *    /_______  //_______  /  |____|  |______/  |____|
  *            \/         \/
  */

 var express = require('express'); // call express
 var app = express(); // define our app using express
 var bodyParser = require('body-parser');
 var mysql = require('mysql');
 var async = require('async');

 app.use(bodyParser.json());
 app.use(bodyParser.urlencoded({
     extended: true
 }));
 app.use('/', express.static(__dirname));

 //mysql configuration

 var connection = mysql.createConnection({
     host: 'www.mihov.ch',
     user: 'quiz_admin',
     password: 'quizio12345',
     database: 'Quizio'
 });

 connection.connect();
 var port = process.env.PORT || 10300;
 var router = express.Router();


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

  router.get('/getCategories', function(req, res) {
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

	router.get('/getRankings', function(req, res) {
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




 /***
  *    __________________    ____________________
  *    \______   \_____  \  /   _____/\__    ___/
  *     |     ___//   |   \ \_____  \   |    |
  *     |    |   /    |    \/        \  |    |
  *     |____|   \_______  /_______  /  |____|
  *                      \/        \/
  */

 router.post('/insertPlayer', function(req, res) {
     printLogStart("insert player", req);
     var playerName = req.body.name;
     var playerPW = req.body.password;
     var playerEmail = req.body.email;
     var playerOrigin = req.body.origin;

     /*var sql = "INSERT INTO player  ('player_id`,
         `name`,
         `password`,
         `email`,
         `origin`)
     VALUES
         (null, " + connection.escape(playerName) + ", " + connection.escape(playerPW) + ", " + connection.escape(playerEmail) + ", " + connection.escape(playerOrigin) +")
     ";*/
     var sql = "Select * from player"

     connection.query(sql, function(err, rows, fields) {
         if (err) throw err;
         res.json({
             status: "OK",
             affectedRows: rows.affectedRows,
         });
         printLogSuccess("player successfully added");
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

 router.put('/updatePlayer/:id', function(req, res) {
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

 router.put('/updateRanking/:id/:toAdd', function(req, res) {
     printLogStart("updating ranking data", req);
     var playerID = req.params.id;
     var toAdd = req.params.toAdd | 0;

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



 // REGISTER OUR ROUTES -------------------------------
 // all of our routes will be prefixed with /api
 app.use('/api', router);

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
