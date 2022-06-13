var fs = require("fs");
var websocket = require("nodejs-websocket")
const wss = require("ws");
var code = require("./const/PACKET_CODE");
const crypto = require("crypto");
const AESCrypto = require("./AESCrypto.js");
const json = require("body-parser/lib/types/json");
const CGameRoom = require("./CGameRoom.js");
const Packet = require("./CPacket.js");
const PROTOCOL = require("./const/PROTOCOL");
const CPlayerAgent = require("./CPlayerAgent.js");
const PlayerPrefs = require("./PlayerPrefs.js");
const Util = require("./Util.js")

// var WEBSOCKET_PORT = 8080;
var datas = [];
var reqs = [];
var db;
var em;
var gameRooms = {};
var CPacket = new Packet();

const options = { transports: ['websocket'], pingTimeout: 3000, pingInterval: 5000 };

exports.init = async function(port1, dbcon, email) {
    db = dbcon;
    em = email;

    wsserver.listen(port1);
    //var timeinterval = setInterval(timeout_check, 30 * 1000);
}

exports.doloop = async function process_datas() {
    while(true) {
        if(datas.length > 0) {
            var data = datas[0];
            datas = datas.slice(1);
            await on_data(data.conn, data.data);
        } 
        if(reqs.length > 0) {
            var req = reqs[0];
            reqs = reqs.slice(1);
            await on_request(req.conn, req.data);
        } 
        await snooze(10);
    }
}

var wsserver = websocket.createServer(options, function (conn) {
    console.log("New connection");
    conn.alive = true;
    //conn._socket.setKeepAlive(true);
    conn.on("binary", function(inStream) {
        var data = new Buffer(0);
        inStream.on("readable", function () {
            var newData = inStream.read();
            if (newData) data = Buffer.concat([data, newData], data.length+newData.length);
        });
        inStream.on("end", function () {
            datas.push({conn : conn, data : data.toString("utf8")});
            //on_data(conn, data.toString("utf8"));
        });
    });
    conn.on("close", function (code, reason) {
        on_close(conn, code, reason);
    });
    conn.on("error", function(error) {
        on_error(conn, error);
    });
    conn.on("end", function(code, reason) {
        on_end(conn, code, reason);
    });   
    // conn.on("text", function(data) {
    //     on_data(conn, data);
    // });
    // conn.on("listening", ()=> {
    //     console.log("Hello");
    // });
    // conn.on("message", function (data) {
    //     on_data(conn, data);
    //     console.log(data);
    // });
});

wsserver.on("connection",(ws)=> {
    const server_key = crypto.createECDH("secp256k1");
    server_key.generateKeys();
    const serverPublicKeyBase64 = server_key.getPublicKey().toString("base64");
    const serverSharedPublicKey = server_key.computeSecret(serverPublicKeyBase64, "base64", "hex");
    ws.send(serverSharedPublicKey);
});

const snooze = ms => new Promise(resolve => setTimeout(resolve, ms));

async function on_data(conn, data) {
    console.log(conn.id+ " <- "+data);
    conn.alive = true;
    var jdata = JSON.parse(data);
    if(jdata.isProtocol == undefined){
        switch( jdata.cmd )
        {
            case code.LOGIN:
            {
                var crypto = new AESCrypto();

                const userinfo = JSON.parse(jdata.data);
                var username = userinfo.pname;
                var password = userinfo.pass;

                var strUserName = crypto.decrypt(username);
                var strPassword = crypto.decrypt(password);

                userinfo.pname = strUserName;
                userinfo.pass  = strPassword;

                var result = await db.on_login(userinfo);

                if(result.code == 0) {
                    send_response(conn, { cmd : code.OK, data : result.message })
                } else {
                    send_response(conn, { cmd : code.ERROR, data : result.message });
                }

                break;
            }
            case code.PROFILE:
            {
                var result = await db.on_profile(jdata.data);
                if(result.code == 0) {
                    send_response(conn, { cmd : code.OK, data : result.message })
                } else {
                    send_response(conn, { cmd : code.ERROR, data : result.message });
                }
                return;
            }
            case code.OPEN:
            {
                var info = JSON.parse(jdata.data);
                conn.id = info.id;
                conn.info = info;
                conn.state = code.OPEN;
                var opp = find_broken(conn);

                if(opp != undefined) {
                    send(opp, { cmd : code.RECONNECT, data : "" });
                    //send(conn, { cmd : code.RECONNECT, data : "" });  
                } else {
                    send(conn, { cmd : code.OK, data : "" });
                }

                return;
            }

            case code.FIND:
            {
                if(conn.opponent == undefined) 
                {
                    conn.state = code.FIND;
                    var op = find_opponent(conn);
                    if(op != undefined) {
                        send(conn, { cmd : code.FIND_MATCHED, data : JSON.stringify(conn.opponent.info)})
                        send(conn.opponent, { cmd : code.FIND_MATCHED, data : JSON.stringify(conn.info)})
                        var start_info = new Object();
                        start_info.user1 = conn.id;
                        start_info.user2 = conn.opponent.id;
                        start_info.coin1 = conn.info.coins;
                        start_info.coin2 = conn.opponent.info.coins;
                        var result = await db.on_start_game(start_info);
                        if(result.code == 0) {
                            creatRoom(conn, conn.opponent, result.message);
                            send(conn, { cmd : PROTOCOL.GAME_SERVER_STARTED, data : result.message, isProtocol : 1});
                            send(conn.opponent, { cmd : PROTOCOL.GAME_SERVER_STARTED, data : result.message,  isProtocol : 1}); 
                        } else {
                            send(conn, { cmd : code.ERROR, data : result.message });
                        }
                    } else {
                        send(conn, { cmd : code.FIND_FAIL, data : "" });
                    }
                } else {
                    var op = conn.opponent.info;
                    send(conn, { cmd : code.FIND_OK, data : JSON.stringify(op) });
                }
                return;
            }
                
            case code.PLAY: 
            {
                conn.state = code.PLAY;
                //conn.startinfo = jdata.data;
                if(conn.opponent.state == undefined) {
                    send(conn, { cmd : code.GOOUT, data : "" });
                } else if(conn.opponent.state == code.PLAY) {
                    send(conn, { cmd : code.START_OK, data : jdata.data });
                    //conn.startinfo = undefined;
                    conn.state = code.WAIT;
                    send(conn.opponent, { cmd : code.START_OK, data : jdata.data });
                    //conn.opponent.startinfo = undefined;
                    conn.opponent.state = code.WAIT;
                }
                return;
            }

            case code.CARD:
            case code.GOSTOP:
            case code.SLOT:
            case code.BOX:
            case code.SPIN:
            case code.CHAT:
            case code.CHAT_OK:
            case code.TURN:
            case code.TURN_OK:
            case code.OK:
            case code.ALLDATA: {
                send(conn.opponent, jdata);
                return;
            }
            case code.CHECK: 
            {
                conn.alive = true;
                return;
            }
            case code.CLOSE:
            {
                conn.state = code.CLOSE;
                return;
            }
        }
    } else if(jdata.isProtocol != undefined && jdata.isProtocol == 1){
        console.log("protocol received <--" + JSON.parse(jdata.data)[0]);

        var arrData = JSON.parse(jdata.data);
        var packet= CPacket.create(jdata.cmd);
        var crypto = new AESCrypto();
        var i = 0;
        arrData.forEach(obj=> {
             arrData[i] =  Number(crypto.decrypt(obj));
             i++;
         });
        packet.setAllData(arrData);
        conn.gameRoom.on_receive(conn, packet);
    }
    send(conn, { cmd : code.UNKNOWN, data : "" });
};

var sendProtocol = function(conn, _packet){
    if(_packet.protocol_id == PROTOCOL.TIME_OUT){
        var packet= CPacket.create(PROTOCOL.SELECT_CARD_REQ);
        packet.setAllData(_packet.buf);
        conn.gameRoom.on_receive(conn, packet);
    }else{
        send(conn, { cmd : _packet.protocol_id, listData : _packet.buf, isProtocol : 1});
        console.log(conn + "--sendProtocol-->" + _packet.protocol_id);
    }
}

function find_broken(conn) {
    return undefined;

    for(i=0; i<wsserver.connections.length; i++) {
        c = wsserver.connections[i];
        if(c == conn) continue;
        if(c.info == undefined) continue;
        if(conn.info == undefined) continue;
        if(c.state == code.BREAK && c.opponent_id == conn.id) {
            c.opponent = conn;
            c.state = code.WAIT;
            conn.opponent = c;
            conn.state = code.WAIT;

            return c;
        }
    }
    return undefined;
}

function find_opponent(conn) {
    for(i=0; i<wsserver.connections.length; i++) {
        c = wsserver.connections[i];
        if(c == conn) continue;
        if(c.info == undefined) continue;
        if(conn.info == undefined) continue;
        //if(c.info.guild == conn.info.guild) continue;

        if(c.state == code.FIND) {
            c.opponent = conn;
            c.state = code.WAIT;

            conn.opponent = c;
            conn.state = code.WAIT;

            return c.info;
        }
    }
    return undefined;
}

function on_close(conn, code, reason) {
    console.log(conn.id+ "    CLOSED");
    close(conn);
}

function on_end(conn, code, reason) {
    console.log(conn.id+ "    LOST");
    close(conn);
}

function on_error(conn, error) {
    console.log(conn.id+ "    ERROR", error);
    //close(conn);
}

function close(conn) {
    conn.alive = false;
    // conn.terminate();
    //console.log(code.CLOSE, code.BREAK, code.GOOUT);

    for(i=0; i<wsserver.connections.length; i++) {
        c = wsserver.connections[i];
        if(c.opponent == conn) {
            // if(conn.state == code.CLOSE) {
                send(c, { cmd : code.GOOUT, data : "" });
            // } else {
            //     send(c, { cmd : code.BREAK, data : "" });
            //     c.state = code.BREAK;
            //     c.opponent_id = conn.id;
            // }
            c.opponent = undefined;
            c.gameRoom = undefined;
            c.sendProtocol = undefined;
            c.state = code.OPEN;
            break;
        }
    }
}

function send(conn, data) {
    if(conn == undefined) return;
    // if(conn.alive = false) return;
    if(conn.id == undefined) return;

    var str = JSON.stringify(data);
    console.log(conn.id+" --> "+str);
    var buf = Buffer.from(str, 'utf8');
    conn.send(buf);
}

function send_response(conn, data) {
    if(conn == undefined) return;
    // if(conn.alive = false) return;
    // if(conn.id == undefined) return;

    var str = JSON.stringify(data);
    console.log("RES --> "+str);
    var buf = Buffer.from(str, 'utf8');
    conn.send(buf);
}

async function timeout_check() {
    for(i=0; i<wsserver.connections.length; i++) {
        c = wsserver.connections[i];
        // send(c, { cmd : code.CHECK, data : ""});
        // if(c.alive == false) send(c, { cmd : code.CHECK, data : ""});
        // c.alive = false;
    }
    // wsserver.connections.forEach(function each(ws) {
    //     //if (ws.alive === false) return ws.close();
    
    //     ws.alive = false;
    //     ws.ping();
    // });
}

function creatRoom( player1, player2, roomID ){
    var gameRoom = new CGameRoom();
    gameRooms[roomID] = gameRoom;
    player1.player_index = 0;
    player2.player_index = 1;
    setGameStartInfo(player1, gameRoom);
    setGameStartInfo(player2, gameRoom);
}

function setGameStartInfo(player, gameRoom){
    var playerPrefs = new PlayerPrefs();
    player.playerPrefs = playerPrefs;
    player.gameRoom = gameRoom;
    gameRoom.add_player(player);
    player.send_protocol = sendProtocol;
    player.agent = new CPlayerAgent(player.player_index);
}
function sendToOthers(conn, user_ids, data) {
    if(user_ids == undefined) return;
    if(conn.user_id == undefined || conn.user_id == -1) return;
    
    clients.forEach(c => {
        uid = c.user_id;
        if(uid != undefined && c != conn && user_ids.indexOf(uid) != -1) {
            send(c, data);
        }
    });
}

function sendTo(user_ids, data) {
    if(user_ids == undefined) return;
    
    clients.forEach(c => {
        uid = c.user_id;
        if(uid != undefined && user_ids.indexOf(uid) != -1) {
            send(c, data);
        }
    });
}

// async function on_request(conn, data) {
//     console.log("req -> "+data);
    
//     var jdata = JSON.parse(data);
//     if(jdata.cmd == code.LOGIN) {
//         var data = JSON.parse(jdata.data);
//         var result = await db.on_login(data);

//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }

//     if(jdata.cmd == code.PROFILE) {
//         var result = await db.on_profile(jdata.data);
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }
    
//     if(jdata.cmd == code.UPDATE) {
//         var data = JSON.parse(jdata.data);
//         var result = await db.on_update(data);
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }
//     if(jdata.cmd == code.STARTGAME) {
//         var data = JSON.parse(jdata.data);
//         var result = await db.on_start_game(data);
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }
//     if(jdata.cmd == code.ENDGAME) {
//         var data = JSON.parse(jdata.data);
//         var result = await db.on_end_game(data);
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }
//     if(jdata.cmd == code.GUILDRANKING) {
//         //var data = JSON.parse(jdata.data);
//         var result = await db.on_guild_ranking();
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }
//     if(jdata.cmd == code.LEADERBOARD) {
//          var data = JSON.parse(jdata.data);
//          var result = await db.on_leaderboard(data);
//         if(result.code == 0) {
//             send_response(conn, { cmd : code.OK, data : result.message })
//         } else {
//             send_response(conn, { cmd : code.ERROR, data : result.message });
//         }
//         return;
//     }

//     send_response(conn, { cmd : code.ERROR, data : "Invalid request." });
// };
