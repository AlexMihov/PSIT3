"use strict";

var async = require('async');
var crypto = require('crypto');

exports.router = function (router, connection) {



 /***
  *      ______________________________
  *     /  _____/\_   _____/\__    ___/
  *    /   \  ___ |    __)_   |    |
  *    \    \_\  \|        \  |    |
  *     \______  /_______  /  |____|
  *            \/        \/
  */

  router.get('/player/by-name/:playername', function(req, res, next) {
    printLogStart('get player by name', req);
    var input = [req.user.id, '%' + req.params.playername + '%', req.user.id];

    var sql = 'SELECT player_id as id, name, origin as location, status ' +
                'FROM player ' +
               'WHERE player_id != ? ' +
                 'AND name LIKE ? ' +
                 'AND player_id NOT IN ('+
                     'SELECT p.player_id ' +
                       'FROM friend f ' +
                 'INNER JOIN player p ON (p.player_id = f.player_friend_id) ' +
                      'WHERE f.player_player_id = ?)';

    connection.query(sql, input, function(err, rows, fields) {
      if (err) next(err);
      res.json(rows);
      printLogSuccess('Plyers successfully fetched');
    });
  });

  router.get('/categories', function(req, res, next) {
    printLogStart('get categories', req);

    connection.query('SELECT category_id as id, name, description FROM category', function(err, rows, fields) {
      if (err) next(err);
      var result = rows;

      result.map(function(item){
        item.quizies = [];
      });
          
      connection.query('SELECT quiz_id as id, title, description, category_category_id as category FROM quiz', function(err, rows, fields) {
        if (err) next(err);
        rows.forEach(function(quiz){
          result.forEach(function(category){
            if(category.id == quiz.category){
              category.quizies.push({ 'id':quiz.id, 
                                      'title':quiz.title, 
                                      'description':quiz.description });
            }
          });
        });
        printLogSuccess('Categories successfully fetched');
        res.json(result);
      });
    });
  });

  router.get('/quiz/:quizID/questions', function(req,res, next){
    printLogStart('get questions from quiz ' + req.params.quizID, req);
    var sql = 'SELECT question_id as id, question, hint ' +
                'FROM question ' +
               'WHERE quiz_quiz_id = ?';

    connection.query(sql, [req.params.quizID], function(err, rows, fields){
      if(err) next(err);
      var questions = rows;

      var getAnswers = function(item, callback){
        item.answers = [];
        var itemsql = 'SELECT a.answer_id as id, a.answer, q.value ' +
                        'FROM answer a ' +
                  'INNER JOIN answerofquestion q ON (a.answer_id = q.answer_answer_id) ' +
                       'WHERE q.question_question_id = ?';
        connection.query(itemsql, [item.id], function(err, rows, fields){
          if(err) next(err);
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
        printLogSuccess('Questions successfully fetched');
      });
    });
  });


  router.get('/friends', function(req,res){
    printLogStart('get friends for user ', req);
    var sql = 'SELECT  p.player_id as Id, p.name, p.origin as location, p.status ' +
              'FROM friend f ' +
              'INNER JOIN player p ' +
              'ON (p.player_id = f.player_friend_id) ' +
              'WHERE f.player_player_id = ?' + connection.escape(req.user.id);

    connection.query(sql, [req.user.id], function(err, rows, fields){
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

  router.get('/challenges', function(req, res, next){
    printLogStart("get all challenges", req);
    getChallengesOfUser(req.user.id, connection, function(err, result){
        if(err) next(err);
        res.json(result);
      });
  });

  router.get('/challenges/:status', function(req, res, next){
    printLogStart("get open challenges for user", req);
      var status = req.params.status;
      getChallengesOfUser(req.user.id, status, connection, function(err, result){
        if(err) next(err);
        res.json(result);
      });
  });

  router.get('/challenge/:id', function(req, res, next){
    printLogStart("get challenge for user", req);

    getChallenge(req.params.id, connection, function(err, result){
      if(err) next(err);
      res.json(result);
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

  router.post('/challenge', function(req, res){
    var newChallenge = req.body;
    if(newChallenge.status == null){
      newChallenge.status = 'offen';
    }
    insertChallenge(req.body, connection, function(err, result){
      if(err) throw err;
      if(err) callback(err);
      console.log(result);
      res.json({'statis': 'OK'});
    });
  });

  router.post('/friend', function(req, res, next) {
    printLogStart('insert friend', req);
    var input = [req.user.id, parseInt(req.body.friendId, 10)];

    var sql = 'INSERT INTO friend (player_player_id, player_friend_id) ' +
                   'VALUES ( ?, ?)';

    connection.query(sql, input, function(err, rows, fields) {
      if(err) {
        if (err.code == 'ER_DUP_ENTRY') {
          res.status(409).send({ 'error': 'You have this friend already added!' });
          err = null;
        } else next(err);
      } else {
        res.json({
        'status': 'OK',
        'affectedRows': rows.affectedRows,
        });
        printLogSuccess('friend successfully added');
      }
    });
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

    connection.query(sqlSelect, [req.body.playerId], function(err, rows, fields){
      if (err) throw err;
      var sql = "";
      var input = [];
      if(rows.length === 0) {
        var sql = "INSERT INTO ranking  (player_id, points)" +
                   " VALUES (?, ?)";
        var input = [req.body.playerId, req.body.toAdd];
      }
      else {
        var newPoints = rows[0].points + req.body.toAdd;
        var input = [newPoints, req.body.playerId];

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

  router.put('/challenge', function(req, res){
    printLogStart("updating ranking data", req);
    var challenge = req.body;

    if(challenge.response != null){
      insertGame(challenge.response, connection, function(err, result){
        challenge.response.id = result;
        updateChallenge(challenge, connection, function(err, result){
          if(err) throw err;
          res.send({'status': 'OK'});
        });
      });

    } else {
      updateChallenge(challenge, connection, function(err, result){
        if(err) callback(err);
        res.send({'status': 'OK'});
      });
    }
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
};





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



function getChallenge(challengeId, connection, callback){

  var sql = 'SELECT c.challenge_id as id' +
                 ', c.challenge_text as text ' +
                 ', c.status' +
                 ', c.response_game_id as responseGameId' +
                 ', g.game_id as gameId' +
                 ', g.time ' +
                 ', q.quiz_id as quizId' + 
                 ', q.title as qTitle' +
                 ', q.description as qDescription' +
                 ', ca.category_id as categoryId' +
                 ', ca.name as cName' +
                 ', ca.description as cDescription' +
                 ', gp.player_id as playerId' +
                 ', gp.name as pName' +
                 ', c.challenged_player_id as challengedPayerId' +
                 ', cp.name as challengedPlayerName ' +
             ' FROM challenge c ' +
             ' INNER JOIN game g ON (c.challenge_game_id = g.game_id) ' +
             'INNER JOIN player cp ON (cp.player_id = c.challenged_player_id) ' +
            ' INNER JOIN player gp ON (gp.player_id = g.player_id) ' +
            ' INNER JOIN quiz q ON (q.quiz_id = g.quiz_id) ' +
            ' INNER JOIN category ca ON (ca.category_id = q.category_category_id) ' +
            ' WHERE c.challenge_id = ?'

  connection.query(sql, [challengeId], function(err, rows, fields){
    if(err) throw err;
    



    var player = {'id': rows[0].playerId, 'name': rows[0].pName};
    var quiz = {'id': rows[0].quizId, 'title': rows[0].qTitle, 'description': rows[0].qDescription};
    var category = {'id': rows[0].categoryId, 'name': rows[0].cName, 'description': rows[0].cDescription};
    var game = { 'id': rows[0].gameId, 'user': player, 'quiz': quiz, 'category': category, 'time': rows[0].time};


    var challengedPlayer = { 'id':rows[0].challengedPayerId, 'name': rows[0].challengedPlayerName};
    var challenge = { 'id': rows[0].id, 'status': rows[0].status, 'text': rows[0].text, 'challenge': game, 'challengedPlayer': challengedPlayer};  

    if(rows[0].responseGameId != null){
      getGame(rows[0].responseGameId, connection, function(err, result){
        if(err) callback(err);
        else {
          challenge.response = result;
          getRounds(challenge.challenge.id, connection, function(err, answers){
            challenge.challenge.rounds = answers;
            callback(null, challenge);
          }); 
        }
      });
    } else {
      getRounds(challenge.challenge.id, connection, function(err, answers){
        challenge.challenge.rounds = answers;
        callback(null, challenge);
      });  
    }
  });
};


function getChallengesOfUser(userId, status, connection, callback){

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
    sql += 'AND c.status = ? ';
    input.push(status);
  } else if(arguments.length == 3) {
    sql += 'OR g.player_id = ? ';
    sql += 'LIMIT 20';
    input.push(userId);
    var callback = connection;
    var connection = status;
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
        var tempChallenge = { 'id': item.id, 'status': item.status, 'text': item.text, 'challenge': challenge, 'challengedPlayer': challengedPlayer};
        challenges.push(tempChallenge);
      }); 
    }
    callback(null, challenges);
  });
};

function updateChallenge(challenge, con, callback){

  if(challenge.response != null){
    var sql = 'UPDATE challenge ' +
                 'SET response_game_id = ? ' +
                   ', status = ? ' +
               'WHERE challenge_id = ?';

    var input = [challenge.response.id, challenge.status, challenge.id];

    con.query(sql, input, function(err, result){
      if(err) throw err;
      console.log(result);
      callback(null, result);
    });
  } else {
    var sql = 'UPDATE challenge ' +
                 'SET status = ? ' +
               'WHERE challenge_id = ?';

    var input = [challenge.status, challenge.id];

    con.query(sql, input, function(err, result){
      if(err) throw err;
      console.log(result);
      callback(null, result);
    });
  }
}

function addGameToChallenge(challenge, callback){
  getGame(challenge.challengeId, function(err, result){
    if(err) callback(err);
    challenge.challenge = result;
    delete challenge.challengeId;
    callback(null, challenge);
  });
};


function insertChallenge(challenge, con, callback){
  var sql = 'INSERT INTO challenge (status, challenge_game_id, challenged_player_id, challenge_text) ' +
                'VALUES (?, ?, ?, ?)';

  insertGame(challenge.challenge, con, function(err, result){
    console.log(result)
    var input = [challenge.status, result, challenge.challengedPlayer.id, challenge.text];

    con.query(sql, input, function(err, result){
      if(err) throw err;
      printLogSuccess('challenge inserted');
      callback(null, {'status': 'OK'});
    });
  });
};

function insertGame(game, con, callback){
  var sql = 'INSERT INTO game (date, quiz_id, player_id, time) ' +
                 'VALUES (CURRENT_DATE, ?, ?, ?)';

  var input = [game.quizId, game.playerId, game.time];

  con.query(sql, input, function(err, result){
    if(err) throw err;
    var gameId = result.insertId;

    insertRounds(gameId, game.givenAnswers, con, function(err){
      if(err) callback(err);
      else {
        printLogSuccess('game inserted');
        callback(null, gameId);
      }
    });
  }); 
};

function insertRounds(gameId, rounds, con, callback){
  if(rounds == null){ 
    callback('rounds is not defined');
  } else if(rounds.length == null || rounds.length === 0){ callback(null, null)}
  else {
    var sql = 'INSERT INTO given_answer (game_id, question_id, answer_id) VALUES';
    var input = [];

    rounds.forEach(function(item){
      sql += '(?, ?, ?),'
      input.push(gameId);
      input.push(item.questionId);
      input.push(item.answerId);
    });
    sql = sql.slice(0,-1);

    con.query(sql, input, function(err, result){
      if(err) throw err;
      else {
        printLogSuccess('rounds inserted');
        callback(null, {'status': 'ok'});
      }
    });
  }
};

