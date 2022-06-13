var db = require("./database");
//var email = require("./email");
var comm = require("./comm");

async function init_server() {
    console.log("Initialization started.");

    if(await db.connect()) { //email)) {
        console.log("MySQL connection OK.");
    } else {
        console.log("MySQL connection failed.");
    }
    await db.init();

    //await email.init(db);
    //console.log("Mail initialization done.");
   
    await comm.init(8080, 8099, db);//, email);
    console.log("WebSocket server started.");

    console.log("Initialization done.");
    comm.doloop();
}

init_server();

function log(str) {
    var d = new Date(Date.now());
    console.log(d.toLocaleString(), str);
}

