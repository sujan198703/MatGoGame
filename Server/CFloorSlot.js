class CFloorSlot
{
	slot_position;
	cards = [];
	
	constructor(position)
	{
		this.slot_position = position;

        reset();
	}


    reset()
    {
        this.cards.Clear();
    }


	is_same(number)
	{
		if (this.cards.Count <= 0)
		{
			return false;
		}

		return this.cards[0].number == number;
	}


	add_card(card)
	{
		this.cards.Add(card);
	}


	remove_card(card)
	{
		this.cards.Remove(card);
	}

	
	is_empty()
	{
		return this.cards.Count <= 0;
	}
}
