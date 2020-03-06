var io = require('socket.io')(process.env.PORT || 3000)
var short = require("shortid")
var mongoose = require('mongoose');
var db = require('./helper/database');
console.log('Server Connected')
var Players = 0;
var clientPlayer

//connect to mongoose
mongoose.connect(db.mongoURI ,{
    useNewUrlParser:true,
    useUnifiedTopology:true
}).then(function(){
    console.log('mongodb connected');
}).catch(function(err){
    console.log(err);
});


require('./models/User');
var User = mongoose.model('users');

io.on('connection', function(socket)
{
    var id = short.generate()
    console.log(id);

    socket.emit('open', {status:"Connected"});    

    socket.on('Registered', function(data){
       console.log("Yolo");
        console.log(data);
        var newUser = new User({
            name:data.Username,
            email:data.Email,
            password:data.Password,
            Wins:0,
            Losses:0
        });
       
        newUser.save();
    });

socket.on('AttemptLogin',function(data){
    User.findOne({name:data.Username}).then(function(user){
       
        if(user.password == data.Password)
        {
            console.log(user.Username + " has connected. Waiting for 1 more player")
            
            clientPlayer = user;
            socket.emit("LoginAccepted", clientPlayer)
            Players++;
        }else 
        {
            console.log(user.password)
            console.log(data.Password)   
            console.log("password denied")

            socket.emit("LoginDenied")
        }
    })
})

    if(Players == 2)
    {
         socket.on('startGame')
    }



})