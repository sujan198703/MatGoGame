
class CPlayerOrderManager
{
	random_cards  = [];
	reset(engine)
	{
		this.random_cards = engine.get_random_cards(2);
	}
}
module.exports = CPlayerOrderManager;