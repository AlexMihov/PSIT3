"use strict";

var async = require('async');


var connection = {};



exports.router = function (router, connection) {

  var async = require('async');
  var crypto = require('crypto');



  router.post('/friend', function(req, res) {
    printLogStart("insert friend", req);
    var playerId = req.user.id;
    var friendId = parseInt(req.body.friendId, 10);

    var sql = "INSERT INTO friend (player_player_id, player_friend_id) " +
                   "VALUES (" + connection.escape(playerId) + ", " + connection.escape(friendId) +")";


    connection.query(sql, function(err, rows, fields) {
      if(err) {
        if (err.code == 'ER_DUP_ENTRY') {
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
  *      ______________________________
  *     /  _____/\_   _____/\__    ___/
  *    /   \  ___ |    __)_   |    |
  *    \    \_\  \|        \  |    |
  *     \______  /_______  /  |____|
  *            \/        \/
  */
  router.get('/player/:playerID', function(req, res) {
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
     var input = [req.user.id, "%" + req.params.playername + "%", req.user.id];

     var sql = "SELECT player_id as id, name, origin as location, status " +
                 "FROM player " +
                "WHERE player_id != ? " +
                  "AND name LIKE ? " +
                  "AND player_id NOT IN ("+
                      "SELECT p.player_id " +
                        "FROM friend f " +
                  "INNER JOIN player p ON (p.player_id = f.player_friend_id)" +
                       "WHERE f.player_player_id = ?)";

     connection.query(sql, input, function(err, rows, fields) {
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
          callback(null, item);
        });
      };

      async.map(questions, getAnswers, function(err, results){
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
    var sql =   'SELECT p2.name AS FromFriend , n.message AS Message '+
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


  router.get('/challenges/:status', function(req, res, next){
    printLogStart("get open challenges for user", req);
    if(req.params == null || req.params.status == null){
      console.log("status equals null or undefined");
      getChallenge(req.user.id, connection, function(err, result){
        if(err) next(err);
        res.json(result);
      });
    }
    else { 
      var status = req.params.status;
      getChallenge(req.user.id, status, connection, function(err, result){
        if(err) next(err);
        console.log('result in route1: ', result);
        res.json(result);
      });
    }
  });


 /***
  *    __________________    ____________________
  *    \______   \_____  \  /   _____/\__    ___/
  *     |     ___//   |   \ \_____  \   |    |
  *     |    |   /    |    \/        \  |    |
  *     |____|   \_______  /_______  /  |____|
  *                      \/        \/
  */

  router.post('challenge', function(req, res){


  });



  router.post('/player', function(req, res) {
    printLogStart("insert player", req);
    
    var sha256 = crypto.createHash("sha256");
    sha256.update(req.body.password, "utf8");
    var hashed = sha256.digest("base64");
    var input = [req.body.name, hashed, req.body.email, req.body.status ,req.body.origin];

    var sql = "INSERT INTO player  (name, password, email, status ,origin)" +
                     " VALUES (?, ?, ?, ?, ?)";


    connection.query(sql, input, function(err, rows, fields) {
      if(err) {
        if (err.code == 'ER_DUP_ENTRY') {
          res.send('ER_DUP_ENTRY');
          res.status(409);
          err = null;
        } else throw err;
      } else {
        res.json({
          status: "OK",
          affectedRows: rows.affectedRows,
        });
        printLogSuccess("player successfully added");
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

  router.put('/profile', function(req, res) {
    printLogStart("Update userdata", req);
    var input = [req.body.name
               , req.body.email
               , req.body.status
               , req.body.location
               , req.user.id];

    var sql = "UPDATE player " +
                 "SET name = ?, email = ?, status = ?, origin = ? " + 
               "WHERE player_id = ?";

    connection.query(sql, input, function(err, rows, fields) {
      if (err) throw err;
      res.json({
        status: "OK",
        affectedRows: rows.affectedRows,
        changedRows: rows.changedRows
      });
      printLogSuccess("Profile successfully updated");
    });
  });

  router.put('/profile/password', function(req, res) {
    printLogStart("Update userdata", req);
    var sha256 = crypto.createHash("sha256");
  	sha256.update(req.body.password, "utf8");
  	var hashed = sha256.digest("base64");
    var input = [hashed, req.user.id];

    var sql = "UPDATE player " +
                 "SET password = ? " + 
               "WHERE player_id = ?";

    connection.query(sql, input, function(err, rows, fields) {
      if (err) throw err;
      res.json({
        status: "OK",
        affectedRows: rows.affectedRows,
        changedRows: rows.changedRows
      });
      printLogSuccess("Password successfully updated");
    });
  });


  router.put('/ranking', function(req, res) {
    printLogStart("updating ranking data", req);

    var sqlSelect = "SELECT points FROM ranking WHERE player_id = ?";

    connection.query(sqlSelect, [req.user.id], function(err, rows, fields){
      if (err) throw err;
      var sql = "";
      var input = [];
      if(rows.length === 0) {
        var sql = "INSERT INTO ranking  (player_id, points)" +
                   " VALUES (?, ?)";
        var input = [req.user.id, req.body.toAdd];
      }
      else {
        var newPoints = rows[0].points + req.body.toAdd;
        var input = [req.user.id, newPoints];

        var sql = "UPDATE ranking SET points = ?" + 
                  " WHERE player_id = ?";
      }
      connection.query(sql, input, function(err, rows, fields) {
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

/***
 *    ________  ___________.____     _________________________________
 *    \______ \ \_   _____/|    |    \_   _____/\__    ___/\_   _____/
 *     |    |  \ |    __)_ |    |     |    __)_   |    |    |    __)_ 
 *     |    `   \|        \|    |___  |        \  |    |    |        \
 *    /_______  /_______  /|_______ \/_______  /  |____|   /_______  /
 *            \/        \/         \/        \/                    \/ 
 */

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

  router.delete('/player', function(req, res) {
    printLogStart("delete player", req);

    var input = [req.body.name];

      var sql = "DELETE FROM player " + 
                      "WHERE name = ? ";

    connection.query(sql, input, function(err, rows, fields) {
      if(err) {
          if (err.code == 'ER_DUP_ENTRY') {
            res.status(409).send({error: "There is already a player with this name!"});
            err = null;
          } else throw err;
      } else {
        res.json({
        status: "OK",
        affectedRows: rows.affectedRows,
        });
        printLogSuccess("player successfully deleted");
      }
    });
  });   
};

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

function getAnswer(item, callback){
  item.answers = [];
  var itemsql = 'SELECT a.answer_id as id, a.answer, q.value ' +
                  'FROM answer a ' +
            'INNER JOIN answerofquestion q ' +
                    'ON (a.answer_id = q.answer_answer_id)' +
                 'WHERE q.question_question_id = ?';
  connection.query(itemsql, [item.id], function(err, rows, fields){
    if(err) throw err;
    rows.forEach(function(answer){
    if(answer.value == 0){ answer.value = false;}
      else {answer.value = true;}
      item['answers'].push(answer);
    });
    callback(null, item);
  });
}

function getSimpleGame(gameId, con, callback){
    if(arguments.length == 3) {
    connection = con;
  } else if(arguments.length == 2) {
    callback = con;
  }

  var sql = 'SELECT g.game_id as id' +
                 ', g.quiz_id as quizId' +
                 ', g.player_id as playerId' +
                 ', g.time ' +
                 ', q.quiz_id as quizId' + 
                 ', q.title as qTitle' +
                 ', q.description as qDescription' +
                 ', c.category_id as categoryId' +
                 ', c.name as cName' +
                 ', c.description as cDescription' +
                 ', p.player_id as playerId' +
                 ', p.name as pName' +
             ' FROM game g ' +
            ' INNER JOIN player p ON (p.player_id = g.player_id) ' +
            ' INNER JOIN quiz q ON (q.quiz_id = g.quiz_id) ' +
            ' INNER JOIN category c ON (c.category_id = q.category_category_id) ' +
            ' WHERE g.game_id = ?'

  connection.query(sql, [gameId], function(err, rows, fields){
    if(err) throw err;
    

    var user = {'id': rows[0].playerId, 'name': rows[0].pName, 'status':rows[0].pStatus, 'location':rows[0].pLocation};
    var quiz = {'id': rows[0].quizId, 'title': rows[0].qTitle, 'description': rows[0].qDescription};
    var category = {'id': rows[0].categoryId, 'name': rows[0].cName, 'description': rows[0].cDescription};
    var result = { 'id': gameId, 'user': user, 'quiz': quiz, 'category': category, 'time': rows[0].time};
  });
};

/*
function getGame(gameId, con, callback){
    if(arguments.length == 3) {
    connection = con;
  } else if(arguments.length == 2) {
    callback = con;
  }

  var sql = 'SELECT g.game_id as id' +
                 ', g.quiz_id as quizId' +
                 ', g.player_id as playerId' +
                 ', g.time ' +
                 ', q.quiz_id as quizId' + 
                 ', q.title as qTitle' +
                 ', q.description as qDescription' +
                 ', c.category_id as categoryId' +
                 ', c.name as cName' +
                 ', c.description as cDescription' +
                 ', p.player_id as playerId' +
                 ', p.name as pName' +
             ' FROM game g ' +
            ' INNER JOIN player p ON (p.player_id = g.player_id) ' +
            ' INNER JOIN quiz q ON (q.quiz_id = g.quiz_id) ' +
            ' INNER JOIN category c ON (c.category_id = q.category_category_id) ' +
            ' WHERE g.game_id = ?'

  connection.query(sql, [gameId], function(err, rows, fields){
    if(err) throw err;
    

    var player = {'id': rows[0].playerId, 'name': rows[0].pName};
    var quiz = {'id': rows[0].quizId, 'title': rows[0].qTitle, 'description': rows[0].qDescription};
    var category = {'id': rows[0].categoryId, 'name': rows[0].cName, 'description': rows[0].cDescription};
    var result = { 'id': gameId, 'user': player, 'quiz': quiz, 'category': category, 'time': rows[0].time};
    getRounds(gameId, function(err, answers){
      result.rounds = answers;
      callback(null, result);
    });  
  });
};*/





var getRounds = function(gameId, con, callback){
  if(arguments.length == 3) {
    connection = con;
  } else if(arguments.length == 2) {
    callback = con;
  }

  var sql = 'SELECT q.question_id AS id, q.question, q.hint, a.answer_id as answerId, a.answer, aq.value, ga.answer_id as givenAnswerId '+
              'FROM given_answer ga ' + 
             'INNER JOIN question q ON (q.question_id = ga.question_id) '+ 
             'INNER JOIN answerofquestion aq ON ( ga.question_id = aq.question_question_id ) '+ 
             'INNER JOIN answer a ON a.answer_id = aq.answer_answer_id ' + 
             'WHERE ga.game_id =?';

  connection.query(sql, [gameId], function(err, rows, fields){
    if(err) {
      callback(err);
    }
    var result = [];
    var last = {'question': {'id':rows[0].id, 'question': rows[0].question, 'hint': rows[0].hint, 'answers': []}, 'givenAnswerId':rows[0].givenAnswerId};
    rows.forEach(function(item){
      var value = true;
      if(item.value == 0){ value = false;}
      if(item.id === last.question.id){
        last.question.answers.push({'id':item.answerId, 'answer': item.answer, 'value': value});
      } else {
        result.push(last);
        last = {'question': {'id':item.id, 'question': item.question, 'hint': item.hint, 'answers': []}, 'givenAnswerId':item.givenAnswerId};
        last.question.answers.push({'id':item.answerId, 'answer': item.answer, 'value': value});
      }
    });
    result.push(last);
    callback(null, result);
  });
};



function getChallenge(userId, status, con, callback){

  var sql = 'SELECT c.challenge_id as id' +
                 ', c.challenge_text as text ' +
                 ', c.status' +
                 ', g.game_id as gameid' +
                 ', g.player_id as gamePlayerId' +
                 ', gp.name as gamePlayerName' +
                 ', g.time ' +
                 ', q.quiz_id as quizId' + 
                 ', q.title as qTitle' +
                 ', q.description as qDescription' +
                 ', ca.category_id as categoryId' +
                 ', ca.name as cName' +
                 ', ca.description as cDescription' +
                 ', c.challenged_player_id as challengedPayerId' +
                 ', cp.name as challengedPlayerName ' +
              'FROM challenge c ' +
             'INNER JOIN game g ON (g.game_id = c.challenge_game_id) ' +
             'INNER JOIN player gp ON (g.player_id = gp.player_id) ' +
             'INNER JOIN player cp ON (cp.player_id = c.challenged_player_id) ' +
             'INNER JOIN quiz q ON (g.quiz_id = q.quiz_id) ' +
             'INNER JOIN category ca ON (ca.category_id = q.category_category_id) ' +
             'WHERE challenged_player_id = ? ';

  var input = [userId];

  if(arguments.length == 4) {
    connection = con;
    sql += 'AND c.status = ?'
    input.push(status);
  } else if(arguments.length == 3) {
    var connection = status;
    var callback = con;
  } else if(arguments.length == 2) {
    var callback = status;
  }

  connection.query(sql, input, function(err, rows, fields){
    if(err) callback(err);
    var challenges = [];
    if(rows != null){
      rows.forEach(function(item){
        var user = {'id': item.gamePlayerId, 'name': item.gamePlayerName };
        var quiz = {'id': item.quizId, 'title': item.qTitle, 'description': item.qDescription};
        var category = {'id': item.categoryId, 'name': item.cName, 'description': item.cDescription};

        var challenge = { 'id': item.gameid, 'user': user, 'time': item.time, 'quiz': quiz, 'category': category};
        var challengedPlayer = { 'id':item.challengedPayerId, 'name': item.challengedPlayerName};
        var tempChallenge = { 'id': item.id, 'status': item.status, 'test': item.text, 'challange': challenge, 'challengedPlayer': challengedPlayer};
        challenges.push(tempChallenge);
      }); 
    }
    callback(null, challenges);
  });
};

/*
function getOpenChallenge(userId, con, callback) {

  if(arguments.length == 3) {
    connection = con;
  } else if(arguments.length == 2) {
    var callback = con;
  }

  var sql = 'SELECT c.challenge_id as id, c.challenge_game_id as challengeId , c.challenge_text as text ' +
              'FROM challenge c ' +
             'WHERE challenged_player_id = ? ' + 
               'AND status = ?';

  connection.query(sql, [userId, 'OPEN'], function(err, rows, fields){
    if(err) throw err;
    async.map(rows, addGameToChallenge, function(err, result){        
      callback(null, result);
    });

  });
};*/

function addGameToChallenge(challenge, callback){
  getGame(challenge.challengeId, function(err, result){
    if(err) callback(err);
    challenge.challange = result;
    delete challenge.challengeId;
    callback(null, challenge);
  });
};




