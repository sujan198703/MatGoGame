const Util = require("./Util.js");
class CFloorSlot
{
	slot_position;
	cards;
	
	constructor(position)
	{
		this.slot_position = position;
		this.cards = [];
        this.reset();
	}


    reset()
    {
        this.cards = [];
    }


	is_same(number)
	{
		if (this.cards.length <= 0)
		{
			return false;
		}

		return this.cards[0].number == number;
	}


	add_card(card)
	{
		this.cards.push(card);
	}


	remove_card(card)
	{
		this.cards = Util.deleteArrV(this.cards, card);
		// this.cards = cards.filter(function(value, index, arr){ 
		// 	return value = card;
		// });
		// this.cards.Remove(card);
	}

	
	is_empty()
	{
		return this.cards.length <= 0;
	}
}
module.exports = CFloorSlot;
