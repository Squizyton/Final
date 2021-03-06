var express = require('express');
var app = express();
var io = require('socket.io')(process.env.PORT || 5000)
var short = require("shortid")
var mongoose = require('mongoose');
var exphbs = require('express-handlebars');
var db = require('./helper/database');
var router = express.Router();
console.log('Server Connected')
var Players = 0;
var WebSitePort = 4000;
var bodyParser = require('body-parser');
var listPlayers = [];
var scores = []
var top10Scores = [10];
//this code sets up template engine as express handlebars
app.engine('handlebars', exphbs({ defaultLayout: 'main' }));
app.set('view engine', 'handlebars');

// create application/json parser
app.use(bodyParser.json());
// create application/x-www-form-urlencoded parser
app.use(bodyParser.urlencoded({ extended: false }));


//connect to mongoose
mongoose.connect(db.mongoURI, {
    useNewUrlParser: true,
    useUnifiedTopology: true
}).then(function () {
    console.log('mongodb connected');
}).catch(function (err) {
    console.log(err);
});

//Unity Connection!
require('./models/User');
var User = mongoose.model('users');

io.on('connection', function (socket) {
    var id = short.generate()
    // console.log(id);

    socket.emit('open', { status: "Connected" });

    socket.on('Registered', function (data) {
        console.log("Yolo");
        console.log(data);
        var newUser = new User({
            name: data.Username,
            email: data.Email,
            password: data.Password,
            Wins: 0,
            Losses: 0
        });

        newUser.save();
    });

    socket.on('AttemptLogin', function (data) {
        User.findOne({ name: data.Username }).then(function (user) {
            if (user.password == data.Password) {
                if (Players < 2) {
                    console.log(user.name + " has connected. Waiting for 1 more player")
                }
                if (!listPlayers.includes(user)) {
                    listPlayers.push(user);
                   
                }
               
                    Players++;
                    socket.emit("LoginAccepted",user)
                    socket.emit("GetPlayerNumber", {playerNumber: Players})
                if (Players === 2) {

                    console.log("Player Amount:" + listPlayers.length)
                    console.log('Sending Players')
                    for (var x = 0; x < listPlayers.length; x++) {
                                io.sockets.emit('MakePlayer', listPlayers[x])                    
                    }
                    io.sockets.emit('StartGame')
                    listPlayers = [];
                }

            } else {  
                console.log("password denied")
                socket.emit("LoginDenied")
            }
        })

    


    })

    socket.on('APLayerPicked',function(data){
        socket.broadcast.emit('OppenentPicked',data)
    })

    socket.on("Results",function(pResults){
        console.log(pResults);
        User.findOne({name:pResults.p1name}).then(function(user){
             user.Wins = pResults.p1Wins,
             user.Losses = pResults.p1Losses
             user.save();
        })
        
        User.findOne({name:pResults.p2name}).then(function(user){
            user.Wins = pResults.p2Wins,
            user.Losses = pResults.p2Losses
            user.save();
       })
       
       io.sockets.emit("Reset");
    })
    socket.on('Test', function (data) {
        console.log('Client Connected')
    })
    //-----------
})

//Website
//get route using express handlebars
app.get('/', function (req, res) {
    var title = "Welcome to the Stats Page!!"
    res.render('index', {
        title: title
    });
});

//Logging In ---------------------------
//Routes for Sign in
app.get('/users/login', function (req, res) {
    res.render('users/login');
});
app.post('/users/login', function (req, res, next) {
    User.findOne({ name: req.body.name }).then(function (user) {
        if (user === null) {
            console.log("User Not Found")
            return;
        }
        else if (user.password == req.body.password) {
            console.log("User logged in to site")
        }
        else if (user.password != req.body.password) {
            console.log("User: " + user.name + " failed to log into site " + "User Password: " + user.password + " Entered Password: " + req.body.password)
        }
    })
});

app.get('/users/register', function (req, res) {
    res.render('users/register');
});

app.post('/users/register', function(req,res){
    var newUser = new User({
        name:req.body.name,
        email:req.body.email,
        password:req.body.password,
        Wins: 0,
        Losses: 0
    });

    newUser.save()
    console.log("New User made")
    res.redirect('/users/listusers')
})
//List Users as a guest
app.get('/users/listusers', function (req, res) {
    User.find().then(function (user) {
        res.render('users/listusers', {
            user: user,
            name: user.name,
            wins: user.Wins,
            losses: user.Losses
        })

    })
});

app.get('/users/topten', function (req, res) {
    User.find().then(function (user) {
       // console.log(user)
     
        user.sort(function(a, b){return b.Wins-a.Wins});
        res.render('users/topten', {
            user:user,
            name: user.name,
            wins: user.Wins,
        })

    })

});


//List Users as Admin


app.listen(WebSitePort, function () {
    console.log("Website running on port 4000");
});

