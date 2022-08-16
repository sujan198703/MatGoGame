const CGostopEngine = require("./CGostopEngine.js");
const TURN_RESULT_TYPE = require("./const/TURN_RESULT_TYPE");
const PLAYER_SELECT_CARD_RESULT = require("./const/PLAYER_SELECT_CARD_RESULT");
const CARD_EVENT_TYPE = require("./const/CARD_EVENT_TYPE");
const PROTOCOL = require("./const/PROTOCOL");
const Packet = require("./CPacket.js");
const CPlayerOrderManager = require("./CPlayerOrderManager.js");
class CGameRoom
{
	//CLocalServer server;
    engine;
    players = [];
	CPacket;
	order_manager;
    received_protocol = {};
	current_turn_player;
	timer = {};
	//CPlayerOrderManager order_manager;

    /*CGameRoom(CLocalServer server)
    {
		this.server = server;
        this.engine = new CGostopEngine();
        this.players = new List<CPlayer>();
        this.received_protocol = new Dictionary<byte, PROTOCOL>();
		this.order_manager = new CPlayerOrderManager();
    }*/

	constructor()
	{
		this.current_turn_player = 0;
		this.engine = new CGostopEngine();
		this.CPacket = new Packet();
		this.order_manager = new CPlayerOrderManager();
	}


	start_time_counting(player_index){
		if(this.timer != undefined)
		 	clearTimeout(this.timer);
			this.timer = setTimeout(() => {
			if( this.engine.current_player_index == player_index){
				var card = this.players[player_index].agent.random_card_from_hand();
				if(card.number == undefined)
					console.log("");
				var time_out_msg = this.CPacket.create(PROTOCOL.TIME_OUT);
				time_out_msg.push(card.number);
				time_out_msg.push(card.pae_type);
				time_out_msg.push(card.position);
				time_out_msg.push(card.slot_index);
				time_out_msg.push(0);
				this.players[player_index].send_protocol(this.players[player_index], time_out_msg);
			}
		}, 7000);
	}
	cancel_time_counting(){
		if(this.timer != undefined)
			clearTimeout(this.timer);
	}
    add_player(newbie)
    {
        this.players.push(newbie);
    }


    reset()
    {
        this.current_turn_player = 0;
		this.engine.reset();
		// for (let i = 0; i < this.players.length; ++i)
		// {
		// 	this.players[i].reset();
		// }
		this.order_manager.reset(this.engine);
    }


    is_received(player_index, protocol)
    {
        if (this.received_protocol[player_index] == undefined)
        {
            return false;
        }

        return this.received_protocol[player_index] == protocol;
    }


    checked_protocol(player_index, protocol)
    {
        //UnityEngine.Debug.Log(player_index + ",  sent : " + protocol);
		if (this.received_protocol[player_index] != undefined)
		{
			console.log("Already contains player. Please call 'clear_received_protocol()' before send to client.");
			return;
		}

        this.received_protocol[player_index] = protocol;
    }


    all_received(protocol)
    {
        if (Object.keys(this.received_protocol).length < this.players.length)
        {
            return false;
        }

        Object.keys(this.received_protocol).forEach(key => {
			if (protocol != this.received_protocol[key])
            {
                return false;
            }
		});
		

		this.clear_received_protocol();
        return true;
    }


	clear_received_protocol()
	{
		this.received_protocol = [];
	}


    //--------------------------------------------------------
    // Handler.
    //--------------------------------------------------------
    on_receive(owner, msg)
    {

		//ServerQueue.instance.Pop();
		var protocol = msg.pop_protocol_id();
        if (this.is_received(owner.player_index, protocol))
        {
            //error!! already exist!!
            return;
        }

        this.checked_protocol(owner.player_index, protocol);

		//UnityEngine.Debug.Log("protocol " + protocol);
		switch (protocol)
		{
			case PROTOCOL.READY_TO_START:
				this.on_ready_req();
				break;

			case PROTOCOL.DISTRIBUTED_ALL_CARDS:
				{
					if (this.all_received(protocol))
					{
						var turn_msg = this.CPacket.create(PROTOCOL.START_TURN);
						turn_msg.push(0);
						this.players[this.engine.current_player_index].send_protocol(this.players[this.engine.current_player_index], turn_msg);
						//this.start_time_counting(this.engine.current_player_index);
						turn_msg = this.CPacket.create(PROTOCOL.START_TURN_OTHER);
						turn_msg.push(0);
						this.players[this.engine.get_next_player_index()].send_protocol(this.players[this.engine.get_next_player_index()], turn_msg);

						
					}
				}
				break;

			case PROTOCOL.SELECT_CARD_REQ:
				{
					//this.cancel_time_counting()
					var number = msg.pop_byte();
					var pae_type = msg.pop_byte();
					var position = msg.pop_byte();
					var slot_index = msg.pop_byte();
					var is_shaking = msg.pop_byte();
					//UnityEngine.Debug.Log("server. " + slot_index);
					this.on_player_put_card(
						owner.player_index, 
						number, pae_type, position, slot_index,
						is_shaking);
				}
				break;

			case PROTOCOL.CHOOSE_CARD:
				{
					this.clear_received_protocol();

					var client_result = msg.pop_byte();
					var choice_index = msg.pop_byte();
					var server_result =
						this.engine.on_choose_card(owner.player_index, client_result, choice_index);
					if (server_result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
					{
						var choose_result = 
							this.engine.flip_process(owner.player_index, 
							TURN_RESULT_TYPE.RESULT_OF_NORMAL_CARD);
							this.send_flip_result(choose_result, this.engine.current_player_index);
					}
					else
					{
						this.send_turn_result(this.engine.current_player_index);
					}
					//send_turn_result(this.engine.current_player_index, this.engine.selected_slot_index);
				}
				break;

            case PROTOCOL.FLIP_BOMB_CARD_REQ:
                {
                    this.clear_received_protocol();

                    if (this.engine.current_player_index != owner.player_index)
                    {
                        break;
                    }

                    if (!owner.agent.decrease_bomb_count())
                    {
                        // error!
                        console.log("Invalid bomb count!! player {0}" + owner.player_index);
                        break;
                    }

                    var choose_result = this.engine.flip_process(owner.player_index, TURN_RESULT_TYPE.RESULT_OF_BOMB_CARD);
					this.send_flip_result(choose_result, this.engine.current_player_index);
                }
                break;

			case PROTOCOL.FLIP_DECK_CARD_REQ:
				{
					this.clear_received_protocol();

					var result =
						this.engine.flip_process(this.engine.current_player_index, 
						TURN_RESULT_TYPE.RESULT_OF_NORMAL_CARD);
					this.send_flip_result(result, this.engine.current_player_index);
				}
				break;

			case PROTOCOL.TURN_END:
				{
					if (!this.all_received(PROTOCOL.TURN_END))
					{
						break;
					}

					if (this.engine.has_kookjin(this.engine.current_player_index))
					{
						var ask_msg = this.CPacket.create(PROTOCOL.ASK_KOOKJIN_TO_PEE);
						this.players[this.engine.current_player_index].send_protocol(this.players[this.engine.current_player_index], ask_msg);
					}
					else
					{
						this.check_game_finish();
					}
				}
				break;

			case PROTOCOL.ANSWER_KOOKJIN_TO_PEE:
				{
					this.clear_received_protocol();
					owner.agent.kookjin_selected();
					var answer = msg.pop_byte();
					if (answer == 1)
					{
						// 국진을 쌍피로 이동.
						owner.agent.move_kookjin_to_pee();
						this.send_player_statistics(owner.player_index);
						this.broadcast_move_kookjin(owner.player_index);
					}

					this.check_game_finish();
				}
				break;

			case PROTOCOL.ANSWER_GO_OR_STOP:
				{
					this.clear_received_protocol();
					// answer가 1이면 GO, 0이면 STOP.
					var answer = msg.pop_byte();
					if (answer == 1)
					{
						owner.agent.plus_go_count();
						this.broadcast_go_count(owner);
						this.next_turn();
					}
					else
					{
						this.broadcast_game_result();
					}
				}
				break;
		}
    }


	broadcast_go_count(player)
	{
        var delay = this.get_aiplayer_delay(player);

		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.NOTIFY_GO_length);
            msg.push(delay);
			msg.push(player.agent.go_length);
			this.players[i].send_protocol(this.players[i], msg);
		}
	}


	broadcast_game_result()
	{
		var winner = this.players[0];

		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.GAME_RESULT);
			msg.push(1);
			msg.push(15);
			msg.push(winner.agent.score);
			msg.push(2);
			msg.push(16);
			this.players[i].send_protocol(this.players[i], msg);
		}
	}


	broadcast_move_kookjin(who)
	{
		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = CPacket.create(PROTOCOL.MOVE_KOOKJIN_TO_PEE);
			msg.push(who);
			this.players[i].send_protocol(this.players[i], msg);
		}
	}


	check_game_finish()
	{
		if (this.engine.is_time_to_ask_gostop())
		{
			if (this.engine.is_last_turn())
			{
				// 막턴에 났으면 자동 스톱 처리.
				this.broadcast_game_result();
			}
			else
			{
				this.send_player_statistics(this.engine.current_player_index);
				this.send_go_or_stop();
			}
		}
		else
		{
			if (this.engine.is_finished())
			{
				//todo:게임이 끝났는데 나지 못했다면 나가리 처리.
				//todo: If the game is over and you can't play, deal with Nagari.
				this.broadcast_game_result();
				console.log("Nagari!!!!!!");
			}
			else
			{
				this.next_turn();
			}
		}
	}


	send_go_or_stop()
	{
		var msg = this.CPacket.create(PROTOCOL.ASK_GO_OR_STOP);
		this.players[this.engine.current_player_index].send_protocol(this.players[this.engine.current_player_index], msg);
	}


	next_turn()
	{
		this.send_player_statistics(this.engine.current_player_index);

		this.engine.clear_turn_data();
		this.engine.move_to_next_player();
		var turn_msg = this.CPacket.create(PROTOCOL.START_TURN);
		turn_msg.push(this.players[this.engine.current_player_index].agent.remain_bomb_count);

		// 바닥 카드 갱신을 위한 데이터.
		var slots = this.engine.floor_manager.slots;
		turn_msg.push(slots.length);
		for (let i = 0; i < slots.length; ++i)
		{
			turn_msg.push(slots[i].cards.length);
			for (let card_index = 0; card_index < slots[i].cards.lenght; ++card_index)
			{
				var card = slots[i].cards[card_index];
				turn_msg.push(card.number);
				turn_msg.push(card.pae_type);
				turn_msg.push(card.position);
			}
		}
		var turn_msg_other = this.CPacket.create(PROTOCOL.START_TURN_OTHER);
		turn_msg_other.push(0);
		this.players[this.engine.current_player_index].send_protocol(this.players[this.engine.current_player_index], turn_msg);
		//this.start_time_counting(this.engine.current_player_index);
		this.players[this.engine.get_next_player_index()].send_protocol(this.players[this.engine.get_next_player_index()], turn_msg_other);
	}


	on_ready_req()
	{
		if (!this.all_received(PROTOCOL.READY_TO_START))
		{
			return;
		}

		this.reset();
		var order_result = {};
		order_result.numbers = [];
		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.PLAYER_ORDER_RESULT);
			msg.push(this.players[i].player_index);

			msg.push(this.order_manager.random_cards.length);
			order_result.player_count = this.order_manager.random_cards.length;
			
			for (let slot_index = 0; slot_index < this.order_manager.random_cards.length; ++slot_index)
			{
				var card = this.order_manager.random_cards[slot_index];
				msg.push(slot_index);
				msg.push(card.number);
				msg.push(card.pae_type);
				msg.push(card.position);
				if( i == 0 )
					order_result.numbers.push(card.number);
			}
			this.players[i].send_protocol(this.players[i], msg);
		}

		this.engine.start(this.players, order_result);
		for (let i = 0; i < this.players.length; ++i)
		{
			this.send_cardinfo_to_player(this.players[i]);
		}
	}


    send_cardinfo_to_player(player)
    {
        var count = this.engine.distributed_floor_cards.length;

        var msg = this.CPacket.create(PROTOCOL.BEGIN_CARD_INFO);
		//msg.push(player.player_index);
        msg.push(count);
        for (let i = 0; i < count; ++i)
        {
            msg.push(this.engine.distributed_floor_cards[i].number);
            msg.push(this.engine.distributed_floor_cards[i].pae_type);
            msg.push(this.engine.distributed_floor_cards[i].position);
        }

		msg.push(this.players.length);
		for (let i = 0; i < this.players.length; ++i)
		{
			var player_index = this.players[i].player_index;
			var players_card_count = this.engine.distributed_players_cards[player_index].length;
			msg.push(player_index);
			msg.push(players_card_count);

			// 플레이어 본인의 카드정보만 실제 카드로 보내주고,
			// 다른 플레이어의 카드는 null카드로 보내줘서 클라이언트딴에서는 알지 못하게 한다.
			if (player.player_index == player_index)
			{
				for (let card_index = 0; card_index < players_card_count; ++card_index)
				{
					msg.push(this.engine.distributed_players_cards[player_index][card_index].number);
					msg.push(this.engine.distributed_players_cards[player_index][card_index].pae_type);
					msg.push(this.engine.distributed_players_cards[player_index][card_index].position);
				}
			}
			else
			{
				for (let card_index = 0; card_index < players_card_count; ++card_index)
				{
					// 다른 플레이어의 카드는 null카드로 보내준다.
					msg.push(255);
				}
			}
		}

        player.send_protocol(player, msg);
    }


    on_player_put_card(player_index, 
		card_number,
		pae_type, 
		position,
		slot_index,
		is_shaking)
    {
		var result = this.engine.player_put_card(
			player_index, card_number, pae_type, position, slot_index, is_shaking);

		if (result == PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD)
		{
			return;
		}
		this.clear_received_protocol();
		this.send_select_card_ack(result, player_index, slot_index);
    }


	/// <summary>
	/// 카드 선택에 대한 결과를 모든 플레이어에게 전송한다.
	/// </summary>
	/// <param name="current_turn_player_index"></param>
	/// <param name="slot_index"></param>
	send_select_card_ack(result,
		current_turn_player_index, slot_index)
	{
        var delay = this.get_aiplayer_delay(this.players[current_turn_player_index]);

		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.SELECT_CARD_ACK);

            msg.push(delay);

			// 플레이어 정보.
			msg.push(current_turn_player_index);

			// 낸 카드 정보.
			this.add_player_select_result_to(msg, slot_index);

			// 둘중 하나를 선택하는 경우 대상이 되는 카드 정보를 담는다.
			msg.push(result);
			if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER)
			{
				this.add_choice_card_info_to(msg);
			}

			this.players[i].send_protocol(this.players[i], msg);
		}
	}

	send_turn_result(current_turn_player_index)
	{
		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.TURN_RESULT);
			// 플레이어 정보.
			msg.push(current_turn_player_index);

			this.add_player_get_cards_info_to(msg);
			this.add_others_card_result_to(msg);
			this.add_turn_result_to(msg, current_turn_player_index);

			this.players[i].send_protocol(this.players[i], msg);
		}
	}


	send_flip_result(result, current_turn_player_index)
	{
		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.FLIP_DECK_CARD_ACK);
			// 플레이어 정보.
			msg.push(current_turn_player_index);

			this.add_flip_result_to(msg);

			msg.push(result);
			if (result == PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK)
			{
				this.add_choice_card_info_to(msg);
			}
			else
			{
				this.add_player_get_cards_info_to(msg);
				this.add_others_card_result_to(msg);
				this.add_turn_result_to(msg, current_turn_player_index);
			}

			this.players[i].send_protocol(this.players[i], msg);
		}

		for (let i = 0; i < this.players.length; ++i)
		{
			var msgScore = this.CPacket.create(PROTOCOL.GAME_SCORE);

			if( i == 0 )
			{
				msgScore.push(this.players[0].agent.PlayerScore);
				msgScore.push(this.players[1].agent.PlayerScore);
				msgScore.push(this.players[0].agent.HondDanPlayerScore);
				msgScore.push(this.players[1].agent.HondDanPlayerScore);
				msgScore.push(this.players[0].agent.GwangPlayerScore);
				msgScore.push(this.players[1].agent.GwangPlayerScore);
			}
			else
			{
				msgScore.push(this.players[1].agent.PlayerScore);
				msgScore.push(this.players[0].agent.PlayerScore);
				msgScore.push(this.players[1].agent.HondDanPlayerScore);
				msgScore.push(this.players[0].agent.HondDanPlayerScore);
				msgScore.push(this.players[1].agent.GwangPlayerScore);
				msgScore.push(this.players[0].agent.GwangPlayerScore);
			}

			this.players[i].send_protocol(this.players[i], msgScore);
console.log("-------------------- Player Score sending -----------------" + this.players[i].agent.PlayerScore);
console.log("-------------------- HongDan Score sending -----------------" + this.players[i].agent.HondDanPlayerScore);
console.log("-------------------- Gwang Score sending -----------------" + this.players[i].agent.GwangPlayerScore);
		}
	}


    add_player_select_result_to(msg, slot_index)
    {
        // 플레이어가 낸 카드 정보.
        msg.push(this.engine.card_from_player.number);
        msg.push(this.engine.card_from_player.pae_type);
        msg.push(this.engine.card_from_player.position);
        msg.push(this.engine.same_card_count_with_player);
        msg.push(slot_index);

        // 카드 이벤트.
        msg.push(this.engine.card_event_type);

        // 폭탄 카드 정보.
		switch (this.engine.card_event_type)
		{
			case CARD_EVENT_TYPE.BOMB:
				{
					var bomb_cards_count = this.engine.bomb_cards_from_player.length;
					msg.push(bomb_cards_count);
					for (let card_index = 0; card_index < bomb_cards_count; ++card_index)
					{
						msg.push(this.engine.bomb_cards_from_player[card_index].number);
						msg.push(this.engine.bomb_cards_from_player[card_index].pae_type);
						msg.push(this.engine.bomb_cards_from_player[card_index].position);
					}
				}
				break;

			case CARD_EVENT_TYPE.SHAKING:
				{
					var shaking_cards_count = this.engine.shaking_cards.length;
					msg.push(shaking_cards_count);
					for (let card_index = 0; card_index < shaking_cards_count; ++card_index)
					{
						msg.push(this.engine.shaking_cards[card_index].number);
						msg.push(this.engine.shaking_cards[card_index].pae_type);
						msg.push(this.engine.shaking_cards[card_index].position);
					}
				}
				break;
		}
    }


    add_flip_result_to(msg)
    {
        // 덱에서 뒤집은 카드 정보.
        msg.push(this.engine.card_from_deck.number);
        msg.push(this.engine.card_from_deck.pae_type);
        msg.push(this.engine.card_from_deck.position);
        msg.push(this.engine.same_card_count_with_deck);
    }


	add_player_get_cards_info_to(msg)
	{
		// 플레이어가 가져갈 카드 정보.
		var count_to_player = this.engine.cards_to_give_player.length;
		msg.push(count_to_player);
		for (let card_index = 0; card_index < count_to_player; ++card_index)
		{
			var card = this.engine.cards_to_give_player[card_index];
			msg.push(card.number);
			msg.push(card.pae_type);
			msg.push(card.position);
		}
	}


	add_others_card_result_to(msg)
	{
		msg.push(this.engine.other_cards_to_player.length);
		
		Object.keys(this.engine.other_cards_to_player).forEach(key => {
			msg.push(parseInt(key));

			var count = this.engine.other_cards_to_player[key].length;
			msg.push(count);
			for (let card_index = 0; card_index < count; ++card_index)
			{
				var card = this.engine.other_cards_to_player[key][card_index];
				msg.push(card.number);
				msg.push(card.pae_type);
				msg.push(card.position);
			}
		});
	}


	add_turn_result_to(msg, current_turn_player_index)
    {
        // 점수 정보.
        msg.push(this.players[current_turn_player_index].agent.score);

        // 기타 정보.
        msg.push(this.players[current_turn_player_index].agent.remain_bomb_count);

		// 카드 이벤트 정보.
		var count = this.engine.flipped_card_event_type.length;
		msg.push(count);
		for (let i = 0; i < count; ++i)
		{
			msg.push(this.engine.flipped_card_event_type[i]);
		}
    }


	add_choice_card_info_to(msg)
	{
		var target_cards = this.engine.target_cards_to_choice;
		var count = target_cards.length;
		msg.push(count);

		for (let i = 0; i < count; ++i)
		{
			var card = target_cards[i];
			msg.push(card.number);
			msg.push(card.pae_type);
			msg.push(card.position);
		}
	}


	send_player_statistics(player_index)
	{
		var target_player = this.players[player_index].agent;
		for (let i = 0; i < this.players.length; ++i)
		{
			var msg = this.CPacket.create(PROTOCOL.UPDATE_PLAYER_STATISTICS);
			msg.push(player_index);
			msg.push(target_player.score);
			msg.push(target_player.go_count);
			msg.push(target_player.shaking_count);
			msg.push(target_player.ppuk_count);
			msg.push(target_player.get_pee_count());
			this.players[i].send_protocol(this.players[i], msg);
		}
	}


    /// <summary>
    /// ai플레이일 경우 딜레이 값을 넣어줘서 너무 빨리 진행되지 않도록 한다.
    /// </summary>
    /// <param name="current_player"></param>
    /// <returns></returns>
    get_aiplayer_delay(current_player)
    {
        var delay = 0;
        // if (current_player.is_autoplayer())
        // {
        //     delay = 1;
        // }

        return delay;
    }
}
module.exports = CGameRoom;
