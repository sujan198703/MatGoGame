class CPlayRoomUI
{
    card_manager;

	// 플레이어가 먹은 카드 객체.
	player_card_manager = [];
	player_info_slots   = [];

	// 게임 플레이에 사용되는 객체들.
	player_me_index;
	floor_ui_slots		= [];
	// 가운데 쌓여있는 카드 객체.
	stack_cards			= [];
	player_hand_card_manager	= [];
	// 플레이어가 먹은 카드 객체.
	player_card_manager	= [];
	player_info_slots	= [];

    reset()
	{

		card_manager.make_all_cards();

		for (let i = 0; i < this.floor_ui_slots.Count; ++i)
		{
			this.floor_ui_slots[i].reset();
		}
		
		make_deck_cards();

		for (let i = 0; i < this.player_hand_card_manager.Count; ++i)
		{
			this.player_hand_card_manager[i].reset();
		}

		for (let i = 0; i < this.player_card_manager.Count; ++i)
		{
			this.player_card_manager[i].reset();
		}

		clear_ui();
	}

	make_deck_cards()
	{
		this.stack_cards.Clear();

		for (let i = 0; i < this.total_card_pictures.Count; ++i)
		{
			this.total_card_pictures[i].update_backcard(this.back_image);
			this.total_card_pictures[i].enable_collider(false);
			this.stack_cards.Push(this.total_card_pictures[i]);
		}
	}

	distribute_cards(floor_cards, player_cards)
	{
		begin_cards_picture = [];

		// [바닥 -> 1P -> 2P 나눠주기] 를 두번 반복한다.
		for (let looping = 0; looping < 2; ++looping)
		{
			// 바닥에는 4장씩 분배한다.
			for (let i = 0; i < 4; ++i)
			{
				card = floor_cards.pop();
				card_picture = this.stack_cards.Pop();
				card_picture.update_card(card, get_hwatoo_sprite(card));
				begin_cards_picture.Push(card_picture);

				card_picture.transform.localScale = SCALE_TO_FLOOR;
				move_card(card_picture, card_picture.transform.position, this.floor_slot_position[i + looping * 4]);
			}

			// 플레어이의 카드를 분배한다.
			for (const [key, value] of Object.entries(player_cards))
			{
				player_index = `${key}`;
				cards = `${value}`;

				ui_slot_index = (byte)(looping * 5);
				// 플레이어에게는 한번에 5장씩 분배한다.
				for (let card_index = 0; card_index < 5; ++card_index)
				{
					card_picture = this.stack_cards.Pop();
					card_picture.set_slot_index(ui_slot_index);
					this.player_hand_card_manager[player_index].add(card_picture);

					// 본인 카드는 해당 이미지를 보여주고,
					// 상대방 카드(is_nullcard)는 back_image로 처리한다.
					if (player_index == this.player_me_index)
					{
						card = cards.pop();
						card_picture.update_card(card, get_hwatoo_sprite(card));
						card_picture.transform.localScale = SCALE_TO_MY_HAND;
						move_card(card_picture, card_picture.transform.position,
							this.player_card_positions[player_index].get_hand_position(ui_slot_index));
					}
					else
					{
						card_picture.update_backcard(this.back_image);
						card_picture.transform.localScale = SCALE_TO_OTHER_HAND;
						move_card(card_picture, card_picture.transform.position,
							this.player_card_positions[player_index].get_hand_position(ui_slot_index));
					}

					++ui_slot_index;
				}
			}
		}

		sort_floor_cards_after_distributed(begin_cards_picture);
		sort_player_hand_slots(this.player_me_index);
	}

	sort_floor_cards_after_distributed(begin_cards_picture)
	{
		var slots = {}; 

		for (let i = 0; i < begin_cards_picture.Count; ++i)
		{
			number = begin_cards_picture[i].card.number;
			slot = this.floor_ui_slots.Find(obj => obj.is_same_card(number));
			to = Vector3.zero;
			if (slot == null)
			{
				to = this.floor_slot_position[i];

				slot = this.floor_ui_slots[i];
				slot.add_card(begin_cards_picture[i]);
			}
			else
			{
				to = get_ui_slot_position(slot);

				slot.add_card(begin_cards_picture[i]);
			}


			begin = this.floor_slot_position[i];
			move_card(begin_cards_picture[i], begin, to);
		}
	}

	sort_floor_cards_when_finished_turn()
	{
		for (let i = 0; i < this.floor_ui_slots.Count; ++i)
		{
			var slot = this.floor_ui_slots[i];
			if (slot.get_card_count() != 1)
			{
				continue;
			}

			var card_pic = slot.get_first_card();
			move_card( card_pic, 
				card_pic.transform.position, 
				this.floor_slot_position[slot.ui_slot_position]);
		}
	}
}