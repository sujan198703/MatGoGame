class CPlayerHandCardManager
{
	cards = [];

	constructor()
	{
	}


	reset()
	{
		this.cards.Clear();
	}


	add(card_picture)
	{
		this.cards.push(card_picture);
	}


	remove(card_picture)
	{
		let result = this.cards.remove(card_picture);
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
		return this.cards.find(obj => obj.card.is_same(number, pae_type, position));
	}


	get_same_number_count(number)
	{
		let same_cards = this.cards.find(obj => obj.is_same(number));
		return same_cards.Count;
	}


	sort_by_number()
	{
        this.cards.sort(function(lhs, rhs) {
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
