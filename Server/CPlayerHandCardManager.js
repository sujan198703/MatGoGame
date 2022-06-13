const Util = require("./Util");
class CPlayerHandCardManager
{
	cards = [];

	constructor()
	{
	}


	reset()
	{
		this.cards = [];
	}


	add(card_picture)
	{
		this.cards.push(card_picture);
	}


	remove(card_picture)
	{
		//let result = this.cards.remove(card_picture);
		this.cards = Util.deleteArrV(this.cards, card_picture);
		if (!result)
		{
			console.log("Cannot remove the card!");
		}
	}


	get_card_count()
	{
		return this.cards.Count;
	}


	get_card(index)
	{
		return this.cards.indexOf(index);
	}


	find_card(number, pae_type, position)
	{
		//return this.cards.find(obj => obj.card.is_same(number, pae_type, position));
		var result = this.cards.filter(function(value, index, arr){ 
			return value.is_same(number, pae_type, position) == true;
		});
		return result[0];
	}


	get_same_number_count(number)
	{
		//List<CCardPicture> same_cards = this.cards.FindAll(obj => obj.is_same(number));
		var result = this.cards.filter(function(value, index, arr){ 
			return value.is_same(number) == true;
		});
		return result.length;
	}


	sort_by_number()
	{
        this.cards.sort((lhs, rhs) => {
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
	}


	enable_all_colliders(flag)
	{
		for (let i = 0; i < this.cards.Count; ++i)
		{
			this.cards[i].enable_collider(flag);
		}
	}
}
