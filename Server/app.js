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
var clientPlayer
var bodyParser = require('body-parser');

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
    console.log(id);

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
                if (Players > 2) {
                    console.log(user.Username + " has connected. Waiting for 1 more player")
                }
                clientPlayer = user;
                socket.emit("LoginAccepted", clientPlayer)
                Players++;
            } else {
                console.log(user.password)
                console.log(data.Password)
                console.log("password denied")

                socket.emit("LoginDenied")
            }
        })
    })

    if (Players == 2) {
        socket.on('startGame')
    }

    //----------------------------------------------------------------------
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
        else if(user.password != req.body.password) {
            console.log("User: " + user.name +  " failed to log into site " + "User Password: " + user.password + " Entered Password: " + req.body.password)
        }
    })
});

app.listen(WebSitePort, function () {
    console.log("Website running on port 4000");
});

