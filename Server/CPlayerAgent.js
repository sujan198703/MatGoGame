const { UTF8_LITHUANIAN_CI } = require("mysql/lib/protocol/constants/charsets");
const Util = require("./Util.js");
const PAE_TYPE = require("./const/PAE_TYPE")
const CARD_STATUS = require("./const/CARD_STATUS");

class CPlayerAgent
{
	player_index;

    floor_pae;
    hand_pae;

	score;
	prev_score;

	go_count;
	shaking_count;
	ppuk_count;

    remain_bomb_count;
	is_used_kookjin;
	LightCardsScore = 0;
	PlayerScore = 0;
	HondDanPlayerScore = 0;
	GwangPlayerScore = 0;

    constructor(player_index)
    {
        this.player_index = player_index;
		this.hand_pae = [];
		this.floor_pae = {};
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
		this.hand_pae = [];
		this.floor_pae ={};
        this.remain_bomb_count = 0;
		this.is_used_kookjin = false;
	}


    add_card_to_hand(card)
    {
		//UnityEngine.Debug.Log(string.Format("add to hand. player {0},   {1}, {2}, {3}",
		//	this.player_index, card.number, card.pae_type, card.position));
		this.hand_pae.push(card);
    }


	pop_card_from_hand(
		card_number,
		pae_type,
		position)
    {
		// var card = this.hand_pae.Find(obj =>
		// {
		// 	return obj.number == card_number &&
		// 		obj.pae_type == pae_type &&
		// 		obj.position == position;
		// });
		
		var card = this.hand_pae.filter(function(value, index, arr){ 
			return value.number == card_number && value.pae_type == pae_type && value.position == position;
		});
		
		if (card.length <= 0)
		{
			return undefined;
		}

		//this.hand_pae.Remove(card);
		this.hand_pae = Util.deleteArrV(this.hand_pae, card[0]);
		return card[0];
    }

	random_card_from_hand()
    {
		// var card = this.hand_pae.Find(obj =>
		// {
		// 	return obj.number == card_number &&
		// 		obj.pae_type == pae_type &&
		// 		obj.position == position;
		// });
		if(this.hand_pae.length == 0)
			return undefined;
		var k =  Math.floor(Math.random() * (this.hand_pae.length-1));
		var card = this.hand_pae.filter(function(index){ 
			return index = k;
		});
		
		if (card.length <= 0)
		{
			return undefined;
		}

		//this.hand_pae.Remove(card);
		card[0].slot_index = k;
		return card[0];
    }


	pop_all_cards_from_hand(card_number)
	{
		//cards = this.hand_pae.FindAll(obj => obj.is_same_number(card_number));
		var cards = this.hand_pae.filter(function(value, index, arr){ 
			return value.is_same_number(card_number) == true;
		});
		var result = [];
		for (let i = 0; i < cards.length; ++i)
		{
			//UnityEngine.Debug.Log(string.Format("pop bomb card {0}, {1}, {2}", cards[i].number,
			//	cards[i].pae_type, cards[i].position));

			result.push(cards[i]);
			//this.hand_pae.Remove(cards[i]);
			this.hand_pae = Util.deleteArrV(this.hand_pae, cards[i]);
		}

		return result;
	}

    add_card_to_floor(card)
    {
		if (this.floor_pae[card.pae_type] == undefined)
		{
            var card_list = [];
			this.floor_pae[card.pae_type] = card_list;
		}
		this.floor_pae[card.pae_type].push(card);
    }

	pop_card_from_floor(pee_count_to_want)
	{
		// 피가 한장도 없다면 줄 수 있는게 없다.
		if (this.floor_pae[PAE_TYPE.PEE] == undefined)
		{
			return [];
		}

		var player_pees = this.floor_pae[PAE_TYPE.PEE];
		if (player_pees.length <= 0)
		{
			return [];
		}

		var result = [];
		if (player_pees.length == 1)
		{
			// 갖고 있는 피가 한장밖에 없는 경우에는 그것밖에 줄게 없다.
			result.push(player_pees[0]);
		}
		else
		{
			if (pee_count_to_want == 1)
			{
				//onepee_card = player_pees.Find(obj => obj.status != CARD_STATUS.TWO_PEE);
				var onepee_card = player_pees.filter(function(value, index, arr){ 
					return value.status != CARD_STATUS.TWO_PEE;
				});
				if (onepee_card.length > 0)
				{
					result.push(onepee_card[0]);
				}
				else
				{
					result.push(player_pees[0]);
				}
			}
			else if (pee_count_to_want == 2)
			{
				// 쌍피짜리 한장이 있다면 쌍피를 내어준다.
				//twopee_card = player_pees.Find(obj => obj.status == CARD_STATUS.TWO_PEE);
				//?????
				//sdfsdfsdf
				var twopee_card = player_pees.filter(function(value, index, arr){ 
					return value.status == CARD_STATUS.TWO_PEE;
				});
			
				if (twopee_card.length > 0)
				{
					result.push(twopee_card[0]);
				}
				else
				{
					for (let i = 0; i < pee_count_to_want; ++i)
					{
						result.push(player_pees[i]);
					}
				}
			}
		}


		// 플레이어의 바닥패에서 제거.
		for (let i = 0; i < result.length; ++i)
		{
			//player_pees.Remove(result[i]);
			player_pees = Util.deleteArrV(player_pees, result[i]);
		}
		if (player_pees.length <= 0)
		{
			//this.floor_pae.Remove(PAE_TYPE.PEE);
			this.floor_pae = Util.deleteArrK(this.floor_pae, PAE_TYPE.PEE);
		}

		return result;
	}

	pop_specific_card_from_floor(pae_type, status)
	{
		if (this.floor_pae[pae_type] != undefined )
		{
			return undefined;
		}

		//card = this.floor_pae[pae_type].Find(obj => obj.status == status);
		//this.floor_pae[pae_type].Remove(card);
		this.floor_pae[pae_type] = Util.deleteArrK(this.floor_pae[pae_type], PAE_TYPE.PEE);
		return card;
	}

	find_cards(pae_type)
	{
		if (this.floor_pae[pae_type] != undefined)
		{
			return this.floor_pae[pae_type];
		}

		return undefined;
	}

	get_card_count(pae_type, status)
	{
		if (this.floor_pae[pae_type] == undefined)
		{
			return 0;
		}

		//targets = this.floor_pae[pae_type].FindAll(obj => obj.is_same_status(status));
		var targets = this.hand_pae.filter(function(value, index, arr){ 
			return value.is_same_status(status) == true;
		});
		if(targets.length > 0)
			console.log("sdfsdf");
		return targets.length;
	}


	get_same_card_count_from_hand(number)
	{
		var same_cards = this.find_same_cards_from_hand(number);
		if (same_cards == undefined)
		{
			return 0;
		}

		return same_cards.length;
	}


	find_same_cards_from_hand(number)
	{
		//return this.hand_pae.FindAll(obj => obj.is_same_number(number));
		
		var result = this.hand_pae.filter(function(value, index, arr){ 
			return value.is_same_number(number) == true;
		});
		return result;
		
	}


	get_pee_count()
	{
		var cards = this.find_cards(PAE_TYPE.PEE);
		if (cards == undefined)
		{
			return 0;
		}

		var twopee_count = this.get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
		var total_pee_count = cards.length + twopee_count;
		return total_pee_count;
	}


	get_score_by_type(pae_type)
	{
		var pae_score = 0;

		var cards = this.find_cards(pae_type);
		if (cards == undefined)
		{
			return 0;
		}

		switch (pae_type)
		{
			case PAE_TYPE.PEE:
				{
					var twopee_count = this.get_card_count(PAE_TYPE.PEE, CARD_STATUS.TWO_PEE);
					var total_pee_count = cards.length + twopee_count;
					//UnityEngine.Debug.Log(string.Format("[SCORE] Player {0}, total pee {1}, twopee {2}",
					//	this.player_index, total_pee_count, twopee_count));
					if (total_pee_count >= 10)
					{
						pae_score = total_pee_count - 9;
					}
				}
				break;

			case PAE_TYPE.TEE:
				if (cards.length >= 5)
				{
					pae_score = cards.length - 4;
				}
				break;

			case PAE_TYPE.YEOL:
				if (cards.length >= 5)
				{
					pae_score = cards.length - 4;
				}
				break;

			case PAE_TYPE.KWANG:
				if (cards.length == 5)
				{
					pae_score = 15;
				}
				else if (cards.length == 4)
				{
					pae_score = 4;
				}
				else if (cards.length == 3)
				{
					// 비광이 포함되어 있으면 2점. 아니면 3점.
					//is_exist_beekwang = cards.Exists(obj => obj.is_same_number(CCard.BEE_KWANG));
					var is_exist_beekwang = this.hand_pae.filter(function(value, index, arr){ 
						return value.is_same_status() == true;
					});
					if (is_exist_beekwang.length > 0)
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
		this.score += this.get_score_by_type(PAE_TYPE.PEE);
		this.score += this.get_score_by_type(PAE_TYPE.TEE);
		this.score += this.get_score_by_type(PAE_TYPE.YEOL);
		this.score += this.get_score_by_type(PAE_TYPE.KWANG);

		this.LightCardsScore += this.get_score_by_type(PAE_TYPE.KWANG);  

		// 고도리
		var godori_count = this.get_card_count(PAE_TYPE.YEOL, CARD_STATUS.GODORI);
		if (godori_count == 3)
		{
			this.score += 5;
			//UnityEngine.Debug.Log("Godori 5 score");
		}

		// 홍단, 초단, 청단
		var cheongdan_count = this.get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHEONG_DAN);
		var hongdan_count = this.get_card_count(PAE_TYPE.TEE, CARD_STATUS.HONG_DAN);
		var chodan_count = this.get_card_count(PAE_TYPE.TEE, CARD_STATUS.CHO_DAN);
		if (cheongdan_count == 3)
		{
			this.score += 3;
			this.HongDanPlayerScore = this.score;	
		}

		if (hongdan_count == 3)
		{
			this.score += 3;
			//UnityEngine.Debug.Log("Hongdan 3 score");
		}
		//this.score +=9;
		if (chodan_count == 3)
		{
			this.score += 3;
			//UnityEngine.Debug.Log("Chodan 3 score");
		}

		this.PlayerScore = this.score;
		this.GwangPlayerScore = this.LightCardsScore;		//light cards score for player 

		console.log("================ Player Score :  " + this.PlayerScore + "================");
		console.log("================ Player Gwang Score :  " + this.GwangPlayerScore + "================");
	}


	/// <summary>
	/// 플레이어의 패를 번호 순서에 따라 오름차순 정렬 한다.
	/// </summary>
	/// <param name="player_index"></param>
	sort_player_hand_slots()
	{
        this.hand_pae.sort((lhs, rhs) => {
			if (lhs.number < rhs.number)
			{
				return -1;
			}
			else if (lhs.number > rhs.number)
			{
				return 1;
			}

			return 0;
          });
		
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
		//rhi
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
		if (card == undefined)
		{
			// 국진이 없다!??
			//UnityEngine.Debug.LogError("Cannot find kookjin!!  player : " + this.player_index);
			return;
		}

		card.change_pae_type(PAE_TYPE.PEE);
		card.set_card_status(CARD_STATUS.TWO_PEE);

		this.add_card_to_floor(card);
		this.calculate_score();
console.log("------------------- move_kookjin_to_pee");
	}


	is_empty_on_hand()
	{
		return this.hand_pae.length <= 0;
	}
}
module.exports = CPlayerAgent;
