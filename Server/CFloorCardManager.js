const CFloorSlot = require("./CFloorSlot.js");
const Util = require("./Util");
class CFloorCardManager
{
	// 처음 바닥에 놓을 카드를 보관할 컨테이너.
	begin_cards = [];

	// 같은 번호의 카드를 하나로 묶어서 보관하는 컨테이너. 바닥 카드 정렬 이후에는 이 컨테이너를 사용한다.
	slots = [];
	
	constructor()
	{
		// 바닥 초기화.
		this.slots = [];
		this.begin_cards = [];
        for (let position = 0; position < 12; ++position)
        {
            this.slots.push(new CFloorSlot(position));
        }
	}


    reset()
    {
        this.begin_cards = [];
        for (let position = 0; position < 12; ++position)
        {
            this.slots[position].reset();
        }
    }


	put_to_begin_card(card)
	{
		this.begin_cards.push(card);
	}


	find_empty_slot()
	{
		// var slot = this.slots.Find(obj => obj.is_empty());
		// return slot;
		var result = this.slots.filter(function(value, index, arr){ 
			return value.is_empty() == true;
		});
		return result[0];
	}


	find_slot(card_number)
	{
		var slot = this.slots.filter(function(value, index, arr){ 
			return value.is_same(card_number);
		});
		return slot[0];		
		// var slot = this.slots.Find(obj => obj.is_same(card_number));
		// return slot;
	}


	// 해당번호와 동일한 위치에 카드를 놓는다.
	puton_card(card)
	{
		var slot = this.find_slot(card.number);
		if (slot == undefined )
		{
			slot = this.find_empty_slot();
			slot.add_card(card);
			return;
		}

		this.slots[slot.slot_position].add_card(card);
	}


	remove_card(card)
	{
		var slot = this.find_slot(card.number);
		if (slot != undefined)
		{
			slot.remove_card(card);
			//UnityEngine.Debug.Log(string.Format("removed card. {0}, {1}, {2}, remain {3}",
			//	card.number, card.pae_type, card.position,
			//	slot.cards.length));
		}
	}


    get_same_number_card_count(number)
    {
		var slot = this.find_slot(number);
		if (slot.length <= 0)
		{
			return 0;
		}
		return slot.cards.length;
    }


    get_first_card(number)
    {
		var slot = this.find_slot(number);
		if (slot.length <= 0)
		{
			return undefined;
		}
		return slot.cards[0];
    }


    get_cards(number)
    {
		var slot = this.find_slot(number);
		if (slot == undefined)
		{
			return undefined;
		}
		return slot.cards;
    }


    pop_bonus_cards()
    {
        var bonus_cards = [];
        for (let i = 0; i < this.begin_cards.length; ++i)
        {
            if (this.begin_cards[i].number == 13)
            {
                bonus_cards.push(this.begin_cards[i]);
            }
        }

        for (let i = 0; i < bonus_cards.length; ++i)
        {
            //this.begin_cards.Remove(bonus_cards[i]);
			this.begin_cards = Util.deleteArrV(this.begin_cards, bonus_cards[i]);
        }

        return bonus_cards;
    }


	/// <summary>
	/// 바닥에 깔린 카드중 동일한 카드들은 하나의 슬롯으로 쌓는다.
	/// </summary>
	refresh_floor_cards()
	{
		for (let i = 0; i < this.begin_cards.length; ++i)
		{
			this.puton_card(this.begin_cards[i]);
		}
		this.begin_cards = [];
	}


    validate_floor_card_counts()
    {
        var floor_card_count = 0;
        for (let i = 0; i < this.slots.length; ++i)
        {
            floor_card_count += this.slots[i].cards.length;
        }

        if (floor_card_count != 8)
        {
            return false;
        }

        return true;
    }


	is_empty()
	{
		for (let i = 0; i < this.slots.length; ++i)
		{
			if (!this.slots[i].is_empty())
			{
				return false;
			}
		}

		return true;
	}
}
module.exports = CFloorCardManager;