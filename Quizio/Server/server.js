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
