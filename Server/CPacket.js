const TURN_RESULT_TYPE = require("./const/TURN_RESULT_TYPE");
const PLAYER_SELECT_CARD_RESULT = require("./const/PLAYER_SELECT_CARD_RESULT");
const CARD_EVENT_TYPE = require("./const/CARD_EVENT_TYPE");
class CPacket
{
    protocol_id;
    buf = [];
    constructor(){
        this.protocol_id = 0;
    }
    
    create(protocol_id){
        var packet = new CPacket();
        packet.set_protocol(protocol_id);
		return packet;
    }
    set_protocol(protocol_id)
	{
		this.protocol_id = protocol_id;
	}
    push(data){
        this.buf.push(data);
    }
    pop_byte(){
        return this.buf.shift();
    }
    pop_int(){
        pop_bype();
    }
    setAllData(data){
        // data.forEach(obj => {
        //     this.buf.push(obj);
        // })
        this.buf = data;
    }
    pop_protocol_id(){
        return this.protocol_id;
    }
}
module.exports = CPacket;