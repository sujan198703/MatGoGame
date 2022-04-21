var mysql = require("mysql");
var verify = require("./verify");

var countries;
var cities;
var languages;
var email;
// var expire;

var dbcon = mysql.createConnection( {
    host: "localhost",
    user: "root",
    password: "",
    database: "matgo",
});

var connected = false;
// var max_nr_groupchat;

function query(q, v) {
    //console.log(q);
    return new Promise(r => {
        dbcon.query(q, v, function(err, result, fields) {
            if(err) throw err;
            r(result);
        });
    });
}

exports.connect = async function connect(em) {
    // email = em;
    // return new Promise(r => {
    //     dbcon.connect(function(err) {
    //         if(err) {
    //             r(false);
    //         } else {
    //             r(true);
    //         }
    //     });

    // });
    return await handleDisconnect(dbcon, em);
};

function handleDisconnect(conn, em) {
    email = em;
    return new Promise(r=> {
        conn.on('error', function(err) {
            if (!err.fatal) {
              return;
            }
        
            if (err.code !== 'PROTOCOL_CONNECTION_LOST') {
              throw err;
            }
        
            console.log('Re-connecting lost connection: ' + err.stack);
        
            dbcon = mysql.createConnection(conn.config);
            handleDisconnect(dbcon, em);
            //dbcon.connect();
        });
        conn.connect(function(err) {
            if(err) {
                r(false);
            } else {
                r(true);
            }
        })
    });
}

exports.on_login = async function(data) {
    var q, r, vc, v;
    q = "SELECT * FROM `adw_member` WHERE `name`='"+ data.pname +"' OR `email`='"+ data.pname +"';";
    r = await query(q);
    if(r.length < 1) return { code : -1, message : "Login failed. You are not registered." };

    q = "SELECT * FROM `adw_member` WHERE (`name`='"+ data.pname +"' OR `email`='"+ data.pname +"') AND `password`='"+ data.pass +"';";
    r = await query(q);
    if(r.length < 1) return { code : -1, message : "Invalid password. Please input correct password." };

    return { code : 0, message : ""+r[0].id };
}
  
exports.on_profile = async function(data) {
    var q, r, vc, v;
    q = "SELECT * FROM `adw_member` WHERE `id`="+ data +";";
    r = await query(q);
    if(r.length < 1) return { code : -1, message : "Invalid request. You are not registered." };

    return { code : 0, message : JSON.stringify({
        id : r[0].id,
        pname : r[0].name,
        avatar : r[0].avatar,
        guild : r[0].guild,
        coins : r[0].coins
    }) };
}
  
exports.on_update = async function(data) {
    var q, r, vc, v;
    q = "UPDATE `adw_member` SET ? WHERE `id`="+ data.id +";";
    v = {
        avatar : data.avatar,
        guild : data.guild
    }
    r = await query(q, v);
    if(r.affectedRows < 1) return { code : -1, message : "Profile update failed." };

    return { code : 0, message : "" };
}
  
exports.on_start_game = async function(data) {
    var q, r, vc, v;

    q = "SELECT * from `game` WHERE ((`user1`="+data.user1+" AND `user2`="+data.user2+") OR (`user1`="+data.user2+" AND `user2`="+data.user1+")) AND `end` IS NULL";
    r = await query(q);
    if(r.length > 0) {
        game_id = r[0].id;

        if(r[0].user1 == data.user1) {
            q = "UPDATE `game` SET ? WHERE `id`="+ game_id +";";
            v = {
                coin1 : data.coin1,
                coin2 : data.coin2,
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }
    
        if(r[0].user1 == data.user2) {
            q = "UPDATE `game` SET ? WHERE `id`="+ game_id +";";
            v = {
                coin1 : data.coin2,
                coin2 : data.coin1,
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }
    
        q = "UPDATE `game` SET `start`=NOW() WHERE `id`=" + game_id + ";";
        r = await query(q);
        if(r.affectedRows < 1) return { code : -1, message : "Can't store game start in server." };

        return { code : 0, message : "" + game_id };
    }

    q = "INSERT INTO `game` SET ?";
    v = {
        user1 : data.user1,
        user2 : data.user2,
        coin1 : data.coin1,
        coin2 : data.coin2
    }
    r = await query(q, v);
    if(r.affectedRows < 1) return { code : -1, message : "Can't store game start in server." };

    q = "SELECT LAST_INSERT_ID();";
    r = await query(q);
    if(r.length < 1) return { code : -1 };
    game_id = r[0]['LAST_INSERT_ID()'];

    q = "UPDATE `game` SET `start`=NOW() WHERE `id`=" + game_id + ";";
    r = await query(q);
    if(r.affectedRows < 1) return { code : -1, message : "Can't store game start in server." };

    return { code : 0, message : "" + game_id };
}
  
exports.on_end_game = async function(data) {
    var q, r0, r1, r2, r3;

    q = "SELECT * from `game` WHERE `id`=" + data.gameid + ";";
    r0 = await query(q);
    if(r0.length < 1) return { code : -1, message : "Invalid game data." };

    q = "UPDATE `game` SET `end`=NOW() WHERE `id`=" + data.gameid + ";";
    r1 = await query(q);
    if(r1.affectedRows < 1) return { code : -1, message : "Internal error in server." };

    if(r0[0].user1 == data.user1) {
        q = "UPDATE `game` SET ? WHERE `id`="+ data.gameid +";";
        v = {
            winner : data.winner,
            earned : data.earned,
            coin3 : data.coin3,
            coin4 : data.coin4
        }
        r2 = await query(q, v);
        if(r2.affectedRows < 1) return { code : -1, message : "Internal error in server." };

        if(r0[0].user1 != -1) {
            q = "UPDATE `adw_member` SET ? WHERE `id`="+ r0[0].user1 +";";
            v = {
                coins : data.coin3
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }

        if(r0[0].user2 != -1) {
            q = "UPDATE `adw_member` SET ? WHERE `id`="+ r0[0].user2 +";";
            v = {
                coins : data.coin4
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }
    }

    if(r0[0].user2 == data.user1) {
        q = "UPDATE `game` SET ? WHERE `id`="+ data.gameid +";";
        v = {
            winner : data.winner,
            earned : data.earned,
            coin3 : data.coin4,
            coin4 : data.coin3
        }
        r2 = await query(q, v);
        if(r2.affectedRows < 1) return { code : -1, message : "Internal error in server." };

        if(r0[0].user1 != -1) {
            q = "UPDATE `adw_member` SET ? WHERE `id`="+ r0[0].user1 +";";
            v = {
                coins : data.coin4
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }

        if(r0[0].user2 != -1) {
            q = "UPDATE `adw_member` SET ? WHERE `id`="+ r0[0].user2 +";";
            v = {
                coins : data.coin3
            }
            r3 = await query(q, v);
            if(r3.affectedRows < 1) return { code : -1, message : "Internal error in server." };
        }
    }

    return { code : 0, message : "" };
}
  
exports.on_guild_ranking = async function(data) {
    var q;

    var ranking = []

    for(i=0; i<12; i++) {
        var rank = 0;
        q = "SELECT SUM(`coins`) FROM `adw_member` WHERE `guild`=" + i + ";";
        r0 = await query(q);
        rank = r0[0]["SUM(`coins`)"];
        if(rank == null) rank = 0;

        ranking.push(rank);
    }

    return { code : 0, message : JSON.stringify({
        ranking : ranking
    }) };
}

exports.on_leaderboard = async function(data) {
    var q;

    var lb = []

    q = "SELECT * FROM `adw_member` WHERE `guild`=" + data.guild + " ORDER BY `coins` DESC;";
    r = await query(q);

    if(r.length < 6) {
        for(i=0; i<r.length; i++) {
            lb.push({
                guild : i+1,
                id : r[i].id,
                avatar : r[i].avatar,
                pname : r[i].name,
                coins : r[i].coins
            });
        }
    } else {
        var checked = false;
        for(i=0; i<3; i++) {
            lb.push({
                guild : i+1,
                id : r[i].id,
                avatar : r[i].avatar,
                pname : r[i].name,
                coins : r[i].coins
            });
            if(r[i].id == data.id) checked = true;
        }
        if(checked) {
            for(i=3; i<6; i++) {
                lb.push({
                    guild : i+1,
                    id : r[i].id,
                    avatar : r[i].avatar,
                    pname : r[i].name,
                    coins : r[i].coins
                });
            }
        } else {
            for(i=3; i<r.length; i++) {
                if(r[i].id == data.id) {
                    if(i == 3) {
                        lb.push({
                            guild : i+1,
                            id : r[i].id,
                            avatar : r[i].avatar,
                            pname : r[i].name,
                            coins : r[i].coins
                        });
                        lb.push({
                            guild : i+2,
                            id : r[i+1].id,
                            avatar : r[i+1].avatar,
                            pname : r[i+1].name,
                            coins : r[i+1].coins
                        });
                        lb.push({
                            guild : i+3,
                            id : r[i+2].id,
                            avatar : r[i+2].avatar,
                            pname : r[i+2].name,
                            coins : r[i+2].coins
                        });
                    } else if(i == r.length - 1) {
                        lb.push({
                            guild : i-1,
                            id : r[i-2].id,
                            avatar : r[i-2].avatar,
                            pname : r[i-2].name,
                            coins : r[i-2].coins
                        });
                        lb.push({
                            guild : i,
                            id : r[i-1].id,
                            avatar : r[i-1].avatar,
                            pname : r[i-1].name,
                            coins : r[i-1].coins
                        });
                        lb.push({
                            guild : i+1,
                            id : r[i].id,
                            avatar : r[i].avatar,
                            pname : r[i].name,
                            coins : r[i].coins
                        });
                    } else{
                        lb.push({
                            guild : i,
                            id : r[i-1].id,
                            avatar : r[i-1].avatar,
                            pname : r[i-1].name,
                            coins : r[i-1].coins
                        });
                        lb.push({
                            guild : i+1,
                            id : r[i].id,
                            avatar : r[i].avatar,
                            pname : r[i].name,
                            coins : r[i].coins
                        });
                        lb.push({
                            guild : i+2,
                            id : r[i+1].id,
                            avatar : r[i+1].avatar,
                            pname : r[i+1].name,
                            coins : r[i+1].coins
                        });
                    }
                    break;
                }   
            }
        }
    }

    return { code : 0, message : JSON.stringify({
        lb : lb
    }) };
}

exports.get_config_val = async function get_config_val(key) {
    var q = "select `value` from config where `key`='" + key + "';";
    var result = await query(q);
    return result[0].value;
};

exports.init = async function init() {
    // expire = await get_config_val("expire_time");
};

