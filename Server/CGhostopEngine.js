/// <summary>
/// 맞고의 룰을 구현한 클래스.
/// </summary>
class CGostopEngine
{
	// 전체 카드를 보관할 컨테이너.
	card_manager;
	card_queue = [];

	// 플레이어들.
	first_player_index;
	player_agents = [];

	floor_manager;

	// 게임 진행시 카드 정보들을 저장해놓을 임시 변수들.
	// 한턴이 끝나면 모두 초기화 시켜줘야 한다.
	turn_result_type;
    card_from_player;
	selected_slot_index;
	card_from_deck;
    bomb_cards_from_player;
	target_cards_to_choice;
	same_card_count_with_player;
	same_card_count_with_deck
	card_event_type;
	flipped_card_event_type = [];
	cards_to_give_player = [];
	cards_to_floor       = [];


	other_cards_to_player = [];
	shaking_cards = [];
	// 두개의 카드중 하나를 선택하는 경우는 두가지가 있는데(플레이어가 낸 경우, 덱에서 뒤집은 경우),
	// 서버에서 해당 상황에 맞는 타입을 들고 있다가
	// 클라이언트로부터 온 타입과 맞는지 비교하는데 사용한다.
	// 틀리다면 오류 또는 어뷰징이다.
	expected_result_type;

	current_player_index;

	// history.
	distributed_floor_cards = [];
	distributed_players_cards = [];


	constructor()
	{
		this.first_player_index = 0;
		this.current_player_index = 0;

		clear_turn_data();
	}


	/// <summary>
	/// 게임 한판 시작 전에 초기화 해야할 데이터.
	/// </summary>
	reset()
	{
		this.player_agents.ForEach(obj => obj.reset());
		this.first_player_index = 0;
		this.current_player_index = this.first_player_index;
		this.card_manager.make_all_cards();
		this.distributed_floor_cards.Clear();
		this.distributed_players_cards.Clear();
		this.floor_manager.reset();

		clear_turn_data();
	}


	/// <summary>
	/// 매 턴 진행하기 전에 초기화 해야할 데이터.
	/// </summary>
	clear_turn_data()
	{
		this.turn_result_type = TURN_RESULT_TYPE.RESULT_OF_NORMAL_CARD;
		this.card_from_player = null;
		this.selected_slot_index = byte.MaxValue;
		this.card_from_deck = null;
		this.target_cards_to_choice.Clear();
		this.same_card_count_with_player = 0;
		this.same_card_count_with_deck = 0;
		this.card_event_type = CARD_EVENT_TYPE.NONE;
		this.flipped_card_event_type.Clear();
		this.cards_to_give_player.Clear();
		this.cards_to_floor.Clear();
		this.other_cards_to_player.Clear();
		this.bomb_cards_from_player.Clear();
		this.expected_result_type = PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD;
		this.shaking_cards.Clear();
	}


	// 게임 시작.
	start(players)
	{
		this.player_agents.Clear();
		for (let i = 0; i < players.Count; ++i)
		{
            var cardlist = [];

			this.player_agents.push(players[i].agent);
			this.player_agents[i].reset();
			this.distributed_players_cards.push(cardlist);
		}

		shuffle();
		distribute_cards();

		for (let i = 0; i < this.player_agents.Count; ++i)
		{
			this.player_agents[i].sort_player_hand_slots();
		}
	}


	// 카드 섞기.
	shuffle()
	{
		this.card_manager.shuffle();

		this.card_queue.Clear();
		this.card_manager.fill_to(this.card_queue);
	}


	// 카드 분배.
	distribute_cards()
	{
		player_index = this.first_player_index;
		floor_index = 0;
		// 2번 반복하여 바닥에는 8장, 플레이어에게는 10장씩 돌아가도록 한다.
		for (let count = 0; count < 2; ++count)
		{
			// 바닥에 4장.
			for (let i = 0; i < 4; ++i)
			{
				var card = pop_front_card();
				this.distributed_floor_cards.push(card);

				this.floor_manager.put_to_begin_card(card);
				++floor_index;
			}

			// 1p에게 5장.
			for (let i = 0; i < 5; ++i)
			{
				var card = pop_front_card();
				this.distributed_players_cards[player_index].push(card);

				this.player_agents[player_index].add_card_to_hand(card);
			}

			player_index = find_next_player_index_of(player_index);

			// 2p에게 5장.
			for (let i = 0; i < 5; ++i)
			{
				var card = pop_front_card();
				this.distributed_players_cards[player_index].push(card);

				this.player_agents[player_index].add_card_to_hand(card);
			}

			player_index = find_next_player_index_of(player_index);
		}

		check_bonus_cards();
		this.floor_manager.refresh_floor_cards();
		if (!this.floor_manager.validate_floor_card_counts())
		{
			//todo:fatal!!
			UnityEngine.Debug.LogError("Invalid floor card count!!");
			return;
		}
	}


	MAX_PLAYER_COUNT = 2;
	get_next_player_index()
	{
		if (this.current_player_index < MAX_PLAYER_COUNT - 1)
		{
			return (byte)(this.current_player_index + 1);
		}

		return 0;
	}


	 move_to_next_player()
	{
		this.current_player_index = get_next_player_index();
	}


	find_next_player_index_of(prev_player_index)
	{
		if (prev_player_index < MAX_PLAYER_COUNT - 1)
		{
			return (byte)(prev_player_index + 1);
		}

		return 0;
	}


	check_bonus_cards()
	{
		bonus_cards = this.floor_manager.pop_bonus_cards();
		while (bonus_cards.Count > 0)
		{
			for (let i = 0; i < bonus_cards.Count; ++i)
			{
				// 선에게 지급.
				this.player_agents[0].add_card_to_floor(bonus_cards[i]);

				var card = pop_front_card();
				this.distributed_floor_cards.push(card);
				this.floor_manager.put_to_begin_card(card);
			}

			bonus_cards.Clear();
			bonus_cards = this.floor_manager.pop_bonus_cards();
		}
	}


	pop_front_card()
	{
		return this.card_queue.Dequeue();
	}


	/// <summary>
	/// 플레이어가 카드를 낼 때 호출된다.
	/// </summary>
	/// <param name="player_index"></param>
	/// <param name="card_number"></param>
	/// <param name="pae_type"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	player_put_card(player_index,
		card_number,
		pae_type,
		position,
		slot_index,
		is_shaking)
	{
		this.selected_slot_index = slot_index;

		//UnityEngine.Debug.Log(string.Format("recv {0}, {1}, {2}",
		//	card_number, pae_type, position));

		// 클라이언트가 보내온 카드 정보가 실제로 플레이어가 들고 있는 카드인지 확인한다.
		var card = this.player_agents[player_index].pop_card_from_hand(
			card_number, pae_type, position);
		if (card == null)
		{
			UnityEngine.Debug.LogError(string.Format("invalid card! {0}, {1}, {2}",
				card_number, pae_type, position));
			// error! Invalid slot index.
			return PLAYER_SELECT_CARD_RESULT.ERROR_INVALID_CARD;
		}

		this.card_from_player = card;

		// 바닥 카드중에서 플레이어가 낸 카드와 같은 숫자의 카드를 구한다.
		var same_cards = this.floor_manager.get_cards(card.number);
		if (same_cards != null)
		{
			this.same_card_count_with_player = same_cards.Count;
		}
		else
		{
			this.same_card_count_with_player = 0;
		}

		//UnityEngine.Debug.Log("same card(player) " + this.same_card_count_with_player);
		switch (this.same_card_count_with_player)
		{
			case 0:
				{
					if (is_shaking == 1)
					{
						var count_from_hand = this.player_agents[player_index].get_same_card_count_from_hand(this.card_from_player.number);

						if (count_from_hand == 2)
						{
							this.card_event_type = CARD_EVENT_TYPE.SHAKING;
							this.player_agents[player_index].plus_shaking_count();

							// 플레이어에게 흔든 카드 정보를 보내줄 때 사용하기 위해서 리스트에 보관해 놓는다.
							this.shaking_cards =
								this.player_agents[player_index].find_same_cards_from_hand(
								this.card_from_player.number);
							this.shaking_cards.push(this.card_from_player);
						}
					}
				}
				break;

			case 1:
				{
					// 폭탄인 경우와 아닌 경우를 구분해서 처리 해 준다.
					count_from_hand = this.player_agents[player_index].get_same_card_count_from_hand(this.card_from_player.number);
					if (count_from_hand == 2)
					{
						this.card_event_type = CARD_EVENT_TYPE.BOMB;

						get_current_player().plus_shaking_count();

						// 플레이어가 선택한 카드와, 바닥 카드, 폭탄 카드를 모두 가져 간다.
						this.cards_to_give_player.push(this.card_from_player);
						this.cards_to_give_player.push(same_cards[0]);
						this.bomb_cards_from_player.push(this.card_from_player);
						var bomb_cards = this.player_agents[player_index].pop_all_cards_from_hand(this.card_from_player.number);
						for (let i = 0; i < bomb_cards.Count; ++i)
						{
							this.cards_to_give_player.push(bomb_cards[i]);
							this.bomb_cards_from_player.push(bomb_cards[i]);
						}

						take_cards_from_others(1);
						this.player_agents[player_index].add_bomb_count(2);
					}
					else
					{
						// 뒤집이서 뻑이 나오면 못가져갈 수 있으므로 일단 임시변수에 넣어 놓는다.
						this.cards_to_give_player.push(this.card_from_player);
						this.cards_to_give_player.push(same_cards[0]);
					}
				}
				break;

			case 2:
				{
					if (same_cards[0].pae_type != same_cards[1].pae_type)
					{
						// 카드 종류가 다르다면 플레이어가 한장을 선택할 수 있도록 해준다.
						this.target_cards_to_choice.Clear();
						for (let i = 0; i < same_cards.Count; ++i)
						{
							this.target_cards_to_choice.push(same_cards[i]);
						}

						this.expected_result_type = PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER;
						return PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER;
					}

					// 같은 종류의 카드라면 플레이어가 선택할 필요가 없으므로 첫번째 카드를 선택해 준다.
					this.cards_to_give_player.push(this.card_from_player);
					this.cards_to_give_player.push(same_cards[0]);
				}
				break;

			case 3:
				{
					//todo:자뻑인지 구분하여 처리하기.
					this.card_event_type = CARD_EVENT_TYPE.EAT_PPUK;

					// 쌓여있는 카드를 모두 플레이어에게 준다.
					this.cards_to_give_player.push(card);
					for (let i = 0; i < same_cards.Count; ++i)
					{
						this.cards_to_give_player.push(same_cards[i]);
					}

					//todo:상대방 카드 한장 가져오기. 자뻑이었다면 두장 가져오기.
					take_cards_from_others(1);
				}
				break;
		}

		return PLAYER_SELECT_CARD_RESULT.COMPLETED;
	}


	/// <summary>
	/// 가운데 덱에서 카드를 뒤집어 그에 맞는 처리를 진행한다.
	/// </summary>
	/// <param name="player_index"></param>
	/// <returns></returns>
	flip_process(player_index, turn_result_type)
	{
		this.turn_result_type = turn_result_type;

		card_number_from_player = byte.MaxValue;
		if (this.turn_result_type == TURN_RESULT_TYPE.RESULT_OF_NORMAL_CARD)
		{
			card_number_from_player = this.card_from_player.number;
		}
		result = flip_deck_card(card_number_from_player);
		if (result != PLAYER_SELECT_CARD_RESULT.COMPLETED)
		{
			return result;
		}

		after_flipped_card(player_index);
		return PLAYER_SELECT_CARD_RESULT.COMPLETED;
	}


	/// <summary>
	/// 카드를 뒤집은 후 처리해야할 내용들을 진행한다.
	/// </summary>
	/// <param name="player_index"></param>
	after_flipped_card(player_index)
	{
		// 플레이어가 가져갈 카드와 바닥에 내려놓을 카드를 처리한다.
		give_floor_cards_to_player(player_index);
		sort_player_pae();
		calculate_players_score();


		// 폭탄으로 뒤집는 경우에는 플레이어가 낸 카드가 없으므로 처리를 건너 뛴다.
		if (this.card_from_player != null)
		{
			// 플레이어가 가져갈 카드중에 냈던 카드가 포함되어 있지 않으면 바닥에 내려 놓는다.
			is_exist_player = this.cards_to_give_player.Exists(obj => obj.is_same(
				this.card_from_player.number,
				this.card_from_player.pae_type,
				this.card_from_player.position));
			if (!is_exist_player)
			{
				this.floor_manager.puton_card(this.card_from_player);
				this.cards_to_floor.push(this.card_from_player);
			}
		}


		// 뒤집은 카드에 대해서도 같은 방식으로 처리한다.
		is_exist_deck_card = this.cards_to_give_player.Exists(obj => obj.is_same(
			this.card_from_deck.number,
			this.card_from_deck.pae_type,
			this.card_from_deck.position));
		if (!is_exist_deck_card)
		{
			this.floor_manager.puton_card(this.card_from_deck);
			this.cards_to_floor.push(this.card_from_deck);
			calculate_players_score();
		}

		// 싹쓸이 체크.
		if (this.floor_manager.is_empty())
		{
			this.flipped_card_event_type.push(CARD_EVENT_TYPE.CLEAN);
			take_cards_from_others(1);
		}
	}


	flip_deck_card(card_number_from_player)
	{
		this.card_from_deck = pop_front_card();

		var same_cards = this.floor_manager.get_cards(this.card_from_deck.number);
		if (same_cards != null)
		{
			this.same_card_count_with_deck = same_cards.Count;
		}
		else
		{
			this.same_card_count_with_deck = 0;
		}

		//UnityEngine.Debug.Log(string.Format("flipped card {0},  same card count {1}",
		//	this.card_from_deck.number, this.same_card_count_with_deck));

		switch (this.same_card_count_with_deck)
		{
			case 0:
				if (card_number_from_player == this.card_from_deck.number)
				{
					// 쪽.
					this.flipped_card_event_type.push(CARD_EVENT_TYPE.KISS);

					this.cards_to_give_player.Clear();
					this.cards_to_give_player.push(this.card_from_player);
					this.cards_to_give_player.push(this.card_from_deck);

					take_cards_from_others(1);
				}
				break;

			case 1:
				{
					if (card_number_from_player == this.card_from_deck.number)
					{
						// 뻑.
						this.flipped_card_event_type.push(CARD_EVENT_TYPE.PPUK);
						get_current_player().plus_ppuk_count();

						// 플레이어에게 주려던 카드를 모두 취소한다.
						this.cards_to_give_player.Clear();
					}
					else
					{
						this.cards_to_give_player.push(this.card_from_deck);
						this.cards_to_give_player.push(same_cards[0]);
					}
				}
				break;

			case 2:
				{
					if (this.card_from_deck.number == card_number_from_player)
					{
						// 따닥.
						this.flipped_card_event_type.push(CARD_EVENT_TYPE.DDADAK);

						// 플레이어가 4장 모두 가져간다.
						this.cards_to_give_player.Clear();
						for (let i = 0; i < same_cards.Count; ++i)
						{
							this.cards_to_give_player.push(same_cards[i]);
						}
						this.cards_to_give_player.push(this.card_from_deck);
						this.cards_to_give_player.push(this.card_from_player);

						take_cards_from_others(2);
					}
					else
					{
						if (same_cards[0].pae_type != same_cards[1].pae_type)
						{
							// 뒤집었는데 타입이 다른 카드 두장과 같다면 한장을 선택하도록 한다.
							this.target_cards_to_choice.Clear();
							for (let i = 0; i < same_cards.Count; ++i)
							{
								this.target_cards_to_choice.push(same_cards[i]);
							}

							this.expected_result_type = PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
							return PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK;
						}

						// 카드 타입이 같다면 첫번째 카드를 선택해 준다.
						this.cards_to_give_player.push(this.card_from_deck);
						this.cards_to_give_player.push(same_cards[0]);
					}
				}
				break;

			case 3:
				// 플레이어가 4장 모두 가져간다.
				for (let i = 0; i < same_cards.Count; ++i)
				{
					this.cards_to_give_player.push(same_cards[i]);
				}
				this.cards_to_give_player.push(this.card_from_deck);

				//todo:자뻑이라면 두장.
				take_cards_from_others(1);
				break;
		}

		return PLAYER_SELECT_CARD_RESULT.COMPLETED;
	}


	on_choose_card(player_index, result_type, choice_index)
	{
		if (result_type != this.expected_result_type)
		{
			//todo:error! 기대했던 타입과 다르다! 오류이거나 어뷰징이거나.
			UnityEngine.Debug.LogError(string.Format("Invalid result type. client {0}, expected {1}",
				result_type, this.expected_result_type));
		}


		// 클라이언트에서 엉뚱한 값을 보내올 수 있으므로 검증 후 이상이 있으면 첫번째 카드를 선택한다.
		player_choose_card = null;
		if (this.target_cards_to_choice.Count <= choice_index)
		{
			// Error! Invalid list index. Choice first card.
			player_choose_card = this.target_cards_to_choice[0];
		}
		else
		{
			player_choose_card = this.target_cards_to_choice[choice_index];
		}


		ret = PLAYER_SELECT_CARD_RESULT.COMPLETED;
		switch (this.expected_result_type)
		{
			case PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_PLAYER:
				this.cards_to_give_player.push(this.card_from_player);
				this.cards_to_give_player.push(player_choose_card);
				return this.expected_result_type;

			case PLAYER_SELECT_CARD_RESULT.CHOICE_ONE_CARD_FROM_DECK:
				this.cards_to_give_player.push(this.card_from_deck);
				this.cards_to_give_player.push(player_choose_card);
				after_flipped_card(player_index);
				break;
		}

		return ret;
	}


	give_floor_cards_to_player(player_index)
	{
		for (let i = 0; i < this.cards_to_give_player.Count; ++i)
		{
			//UnityEngine.Debug.Log("give player " + this.cards_to_give_player[i].number);
			this.player_agents[player_index].add_card_to_floor(this.cards_to_give_player[i]);

			this.floor_manager.remove_card(this.cards_to_give_player[i]);
		}
	}


	sort_player_pae()
	{
	}


	calculate_players_score()
	{
		for (let i = 0; i < this.player_agents.Count; ++i)
		{
			this.player_agents[i].calculate_score();
		}
	}


	take_cards_from_others(pee_count)
	{
		attacker = this.player_agents[this.current_player_index];

		next_player = get_next_player_index();
		victim = this.player_agents[next_player];

		//todo:코드를 좀더 명확하게 수정하기.
		if (!this.other_cards_to_player.ContainsKey(next_player))
		{
            var cardlist = [];
			this.other_cards_to_player.push(next_player, cardlist);
		}

		cards = victim.pop_card_from_floor(pee_count);
		if (cards == null)
		{
			return;
		}

		for (let i = 0; i < cards.Count; ++i)
		{
			attacker.add_card_to_floor(cards[i]);
			this.other_cards_to_player[next_player].push(cards[i]);
		}
	}


	is_time_to_ask_gostop()
	{
		return this.player_agents[this.current_player_index].can_finish();
	}

	is_last_turn()
	{
		return this.card_queue.Count <= 1;
	}

	is_finished()
	{
		return this.card_queue.Count <= 0;
	}


	has_kookjin(player_index)
	{
		if (this.player_agents[player_index].is_used_kookjin)
		{
			return false;
		}

		kookjin = this.player_agents[player_index].get_card_count(PAE_TYPE.YEOL, CARD_STATUS.KOOKJIN);
		//UnityEngine.Debug.Log(string.Format("player {0},  Kookjin count {1}", player_index, kookjin));
		if (kookjin <= 0)
		{
			return false;
		}

		return true;
	}


	get_current_player()
	{
		return this.player_agents[this.current_player_index];
	}


	get_random_cards(n)
	{
		var clone_cards = [];
		for (let i = 0; i < this.card_manager.cards.Count; ++i)
		{
			clone_cards.push(this.card_manager.cards[i]);
		}

		result = [];

		for (let i = 0; i < n; ++i)
		{
			index = Math.random() * clone_cards.Count;
			result.push(clone_cards[index]);

			clone_cards.RemoveAt(index);
		}

		return result;
	}
}
