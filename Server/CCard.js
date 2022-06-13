var Enum = require('enum');
var PAE_TYPE = new Enum({
    'PEE'   : 0, 
    'KWANG' : 1, 
    'TEE'   : 2,
    'YEOL'  : 3});

var CARD_STATUS = new Enum({
    'NONE'      : 0, 
    'GODORI'    : 1, 
    'TWO_PEE'   : 2,
    'CHEONG_DAN'  : 3,
    'HONG_DAN'  : 4,
    'CHO_DAN'   : 5,
    'KOOKJIN'   : 6});

class CCard
{
	// Æ¯Á¤ Ä«µå¿¡ ´ëÇÑ µðÆÄÀÎ.
	static BEE_KWANG = 11;

	// 0 ~ 11 number.
	number;

	// pae type.
    pae_type;
	
	// 1,2,3,4 position.
	position;

	status;

	constructor(number, pae_type, position)
	{
		this.number = number;
        this.pae_type = pae_type;
		this.position = position;
		this.status = CARD_STATUS.NONE;
	}

	set_card_status(status)
	{
		this.status = status;
	}

	change_pae_type(pae_type_to_change)
	{
		this.pae_type = pae_type_to_change;
	}

    is_bonus_card(number)
    {
        return number == 13;
    }

	is_same(number, pae_type, position)
	{
		return this.number == number &&
			this.pae_type == pae_type &&
			this.position == position;
	}

	is_same_number(number)
	{
		return this.number == number;
	}

	is_same_status(status)
	{
		return this.status == status;
	}
}
module.exports = CCard;
