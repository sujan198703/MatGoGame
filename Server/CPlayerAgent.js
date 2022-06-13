class CPlayerAgent
{
	player_index;

    floor_pae   = [];
    hand_pae    = [];

	score;
	prev_score;

	go_count;
	shaking_count;
	ppuk_count;

    remain_bomb_count;
	is_used_kookjin;

    constructor(player_index)
    {
        this.player_index = player_index;
		this.score = 0;
		this.prev_score = 0;
        this.remain_bomb_count = 0;
    }


	/// <summary>
	/// 매판 시작 전 초기화 해야 할 변수들.
	/// </summary>
	reset()
	{
		this.score = 0;
		this.prev_score = 0;
		this.go_count = 0;
		this.shaking_count = 0;
		this.ppuk_count = 0;
		this.hand_pae.Clear();
		this.floor_pae.Clear();
        this.remain_bomb_count = 0;
		this.is_used_kookjin = false;
	}


    add_card_to_hand(card)
    {
		//UnityEngine.Debug.Log(string.Format("add to hand. player {0},   {1}, {2}, {3}",
		//	this.player_index, card.number, card.pae_type, card.position));
		this.hand_pae.Add(card);
    }


	pop_card_from_hand(
		card_number,
		pae_type,
		position)
    {
		card = this.hand_pae.Find(obj =>
		{
			return obj.number == card_number &&
				obj.pae_type == pae_type &&
				obj.position == position;
		});

		if (card == null)
		{
			return null;
		}

		this.hand_pae.Remove(card);
		return card;
    }


	pop_all_cards_from_hand(card_number)
	{
		cards = this.hand_pae.FindAll(obj => obj.is_same_number(card_number));
		result = [];
		for (let i = 0; i < cards.Count; ++i)
		{
			//UnityEngine.Debug.Log(string.Format("pop bomb card {0}, {1}, {2}", cards[i].number,
			//	cards[i].pae_type, cards[i].position));

			result.Add(cards[i]);
			this.hand_pae.Remove(cards[i]);
		}

		return result;
	}

    add_card_to_floor(card)
    {
		if (!this.floor_pae.ContainsKey(card.pae_type))
		{
            var card_list = [];
			this.floor_pae.Add(card.pae_type, card_list);
		}
		this.floor_pae[card.pae_type].Add(card);
    }

	pop_card_from_floor(pee_count_to_want)
	{
		// 피가 한장도 없다면 줄 수 있는게 없다.
		if (!this.floor_pae.ContainsKey(PAE_TYPE.PEE))
		{
			return null;
		}

		player_pees = this.floor_pae[PAE_TYPE.PEE];
		if (player_pees.Count <= 0)
		{
			return null;
		}

		result = [];
		if (player_pees.Count == 1)
		{
			// 갖고 있는 피가 한장밖에 없는 경우에는 그것밖에 줄게 없다.
			result.Add(player_pees[0]);
		}
		else
		{
			if (pee_count_to_want == 1)
			{
				onepee_card = player_pees.Find(obj => obj.status != CARD_STATUS.TWO_PEE);
				if (onepee_card != null)
				{
					result.Add(onepee_card);
				}
				else
				{
					result.Add(player_pees[0]);
				}
			}
			else if (pee_count_to_want == 2)
			{
				// 쌍피짜리 한장이 있다면 쌍피를 내어준다.
				twopee_card = player_pees.Find(obj => obj.status == CARD_STATUS.TWO_PEE);
				if (twopee_card != null)
				{
					result.Add(twopee_card);
				}
				else
				{
					for (let i = 0; i < pee_count_to_want; ++i)
					{
						result.Add(player_pees[i]);
					}
				}
			}
		}


		// 플레이어의 바닥패에서 제거.
		for (let i = 0; i < result.Count; ++i)
		{
			player_pees.Remove(result[i]);
		}
		if (player_pees.Count <= 0)
		{
			this.floor_pae.Remove(PAE_TYPE.PEE);
		}

		return result;
	}

	pop_specific_card_from_floor(pae_type, status)
	{
		if (!this.floor_pae.ContainsKey(pae_type))
		{
			return null;
		}

		card = this.floor_pae[pae_type].Find(obj => obj.status == status);
		this.floor_pae[pae_type].Remove(card);
		return card;
	}

	find_cards(pae_type)
	{
		if (this.floor_pae.ContainsKey(pae_type))
		{
			return this.floor_pae[pae_type];
		}

		return null;
	}

	get_card_count(pae_type, status)
	{
		if (!this.floor_pae.ContainsKey(pae_type))
		{
			return 0;
		}

		targets = this.floor_pae[pae_type].FindAll(obj => obj.is_same_status(status));
		if (targets == null)
		{
			return 0;
		}

		return targets.Count;
	}


	get_same_card_count_from_hand(number)
	{
		same_cards = find_same_cards_from_hand(number);
		if (same_cards == null)
		{
			return 0;
		}

		return same_cards.Count;
	}


	find_same_cards_from_hand(number)
	{
		return this.hand_pae.FindAll(obj => obj.is_same_number(number));
	}


	get_pee_count()
	{
		var cards = find_cards(PAE_TYPE.PEE);
		if (cards == null)
		{
			return 0;
		}

		twopee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
		total_pee_count = (byte)(cards.Count + twopee_count);
		return total_pee_count;
	}


	get_score_by_type(pae_type)
	{
		pae_score = 0;

		cards = find_cards(pae_type);
		if (cards == null)
		{
			return 0;
		}

		switch (pae_type)
		{
			case PAE_TYPE.PEE:
				{
					twopee_count = get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
					total_pee_count = (byte)(cards.Count + twopee_count);
					//UnityEngine.Debug.Log(string.Format("[SCORE] Player {0}, total pee {1}, twopee {2}",
					//	this.player_index, total_pee_count, twopee_count));
					if (total_pee_count >= 10)
					{
						pae_score = (short)(total_pee_count - 9);
					}
				}
				break;

			case PAE_TYPE.TEE:
				if (cards.Count >= 5)
				{
					pae_score = (short)(cards.Count - 4);
				}
				break;

			case PAE_TYPE.YEOL:
				if (cards.Count >= 5)
				{
					pae_score = (short)(cards.Count - 4);
				}
				break;

			case PAE_TYPE.KWANG:
				if (cards.Count == 5)
				{
					pae_score = 15;
				}
				else if (cards.Count == 4)
				{
					pae_score = 4;
				}
				else if (cards.Count == 3)
				{
					// 비광이 포함되어 있으면 2점. 아니면 3점.
					is_exist_beekwang = cards.Exists(obj => obj.is_same_number(CCard.BEE_KWANG));
					if (is_exist_beekwang)
					{
						pae_score = 2;
					}
					else
					{
						pae_score = 3;
					}
				}
				break;
		}

		//UnityEngine.Debug.Log(string.Format("{0} {1} score", pae_type, pae_score));
		return pae_score;
	}

	calculate_score()
	{
		this.score = 0;
		this.score += get_score_by_type(PAE_TYPE.PEE);
		this.score += get_score_by_type(PAE_TYPE.TEE);
		this.score += get_score_by_type(PAE_TYPE.YEOL);
		this.score += get_score_by_type(PAE_TYPE.KWANG);

		// 고도리
		godori_count = get_card_count(PAE_TYPE.YEOL, CARD_STATUS.GODORI);
		if (godori_count == 3)
		{
			this.score += 5;
			//UnityEngine.Debug.Log("Godori 5 score");
		}

		// 홍단, 초단, 청단
		cheongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHEONG_DAN);
		hongdan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.HONG_DAN);
		chodan_count = get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHO_DAN);
		if (cheongdan_count == 3)
		{
			this.score += 3;
			//UnityEngine.Debug.Log("Cheongdan 3 score");
		}

		if (hongdan_count == 3)
		{
			this.score += 3;
			//UnityEngine.Debug.Log("Hongdan 3 score");
		}

		if (chodan_count == 3)
		{
			this.score += 3;
			//UnityEngine.Debug.Log("Chodan 3 score");
		}

		//UnityEngine.Debug.Log(string.Format("[SCORE] player {0},  score {1}", this.player_index, this.score));
	}


	/// <summary>
	/// 플레이어의 패를 번호 순서에 따라 오름차순 정렬 한다.
	/// </summary>
	/// <param name="player_index"></param>
	sort_player_hand_slots()
	{
        this.hand_pae.sort(function(lhs, rhs) {
			if (lhs.card.number < rhs.card.number)
			{
				return -1;
			}
			else if (lhs.card.number > rhs.card.number)
			{
				return 1;
			}

			return 0;
          });

		var debug = string.Format("player [{0}] ", this.player_index);
		for (let i = 0; i < this.hand_pae.Count; ++i)
		{
			debug += string.Format("{0}, ",
				this.hand_pae[i].number);
		}
		//UnityEngine.Debug.Log(debug);
	}

    add_bomb_count(count)
    {
        this.remain_bomb_count += count;
    }

    decrease_bomb_count()
    {
        if (this.remain_bomb_count > 0)
        {
            --this.remain_bomb_count;
            return true;
        }

        return false;
    }

	can_finish()
	{
		if (this.score < 7)
		{
			return false;
		}

		if (this.prev_score >= this.score)
		{
			return false;
		}

		this.prev_score = this.score;
		return true;
	}


	plus_go_count()
	{
		++this.go_count;
	}


	plus_shaking_count()
	{
		++this.shaking_count;
	}


	plus_ppuk_count()
	{
		++this.ppuk_count;
	}


    kookjin_selected()
	{
		this.is_used_kookjin = true;
	}

	move_kookjin_to_pee()
	{
		card = pop_specific_card_from_floor(PAE_TYPE.YEOL, CARD_STATUS.KOOKJIN);
		if (card == null)
		{
			// 국진이 없다!??
			UnityEngine.Debug.LogError("Cannot find kookjin!!  player : " + this.player_index);
			return;
		}

		card.change_pae_type(PAE_TYPE.PEE);
		card.set_card_status(CARD_STATUS.TWO_PEE);

		add_card_to_floor(card);
		calculate_score();
	}


	is_empty_on_hand()
	{
		return this.hand_pae.Count <= 0;
	}
}
