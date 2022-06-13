class CCardManager
{
    cards = [];

	constructor()
	{
	}

    make_all_cards()
    {
        // Generate cards.
        const total_pae_type = [];

        // 1
        total_pae_type.push(PAE_TYPE.KWANG);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 2
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 3
        total_pae_type.push(PAE_TYPE.KWANG);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 4
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 5
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 6
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 7
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 8
        total_pae_type.push(PAE_TYPE.KWANG);
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 9
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 10
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 11
        total_pae_type.push(PAE_TYPE.KWANG);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);
        total_pae_type.push(PAE_TYPE.PEE);

        // 12
        total_pae_type.push(PAE_TYPE.KWANG);
        total_pae_type.push(PAE_TYPE.YEOL);
        total_pae_type.push(PAE_TYPE.TEE);
        total_pae_type.push(PAE_TYPE.PEE);

		this.cards.clear();
        for (let number = 0; number < 12; ++number)
        {
            for (let pos = 0; pos < 4; ++pos)
            {
                this.cards.push(new CCard(number, total_pae_type.shift(), pos));
            }
        }
    }


    set_allcard_status()
    {
        // 카드 속성 설정.
        // 고도리.
        apply_card_status(1, PAE_TYPE.YEOL, 0, CARD_STATUS.GODORI);
        apply_card_status(3, PAE_TYPE.YEOL, 0, CARD_STATUS.GODORI);
        apply_card_status(7, PAE_TYPE.YEOL, 1, CARD_STATUS.GODORI);

        // 청단, 홍단, 초단
        apply_card_status(5, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);
        apply_card_status(8, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);
        apply_card_status(9, PAE_TYPE.TEE, 1, CARD_STATUS.CHEONG_DAN);

        apply_card_status(0, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);
        apply_card_status(1, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);
        apply_card_status(2, PAE_TYPE.TEE, 1, CARD_STATUS.HONG_DAN);

        apply_card_status(3, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);
        apply_card_status(4, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);
        apply_card_status(6, PAE_TYPE.TEE, 1, CARD_STATUS.CHO_DAN);

        // 쌍피.
        apply_card_status(10, PAE_TYPE.PEE, 1, CARD_STATUS.TWO_PEE);
        apply_card_status(11, PAE_TYPE.PEE, 3, CARD_STATUS.TWO_PEE);

        // 국진.
        apply_card_status(8, PAE_TYPE.YEOL, 0, CARD_STATUS.KOOKJIN);
    }

    apply_card_status( number, pae_type, position, status)
    {
        let card = find_card(number, pae_type, position);
        card.set_card_status(status);
    }

    shuffle()
    {
        CHelper.Shuffle<CCard>(this.cards);
    }

	find_card(number, pae_type, position)
	{
		return this.cards.Find(obj => obj.is_same(number, pae_type, position));
	}


	TEST_MAKE_SHAKING_CARDS()
	{
		this.cards.clear();

		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));

		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));

		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));

		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));

		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));

		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));

		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
	}

	TEST_MAKE_KOOKJIN_CARDS()
	{
		this.cards.clear();
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
	}

	TEST_MAKE_BOMB_CARDS()
	{
		// 1P 폭탄 패.
		this.cards.clear();
		// 바닥 4장.
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));

		// 1P 5장.
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));

		// 2P 5장.
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));

		// 바닥 4장.
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));

		// 1P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));

		// 2P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));

		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
	}

	TEST_MAKE_KISS_CARDS()
	{
		this.cards.Clear();
		// 바닥 4장.
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));

		// 1P 5장.
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));

		// 2P 5장.
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));

		// 바닥 4장.
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));

		// 1P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));

		// 2P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));

		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
	}

	TEST_MAKE_PPUK_CARDS()
	{
		this.cards.Clear();
		// 바닥 4장.
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));

		// 1P 5장.
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));

		// 2P 5장.
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));

		// 바닥 4장.
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));

		// 1P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));

		// 2P 5장.
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));

		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
	}

	TEST_MAKE_CLEAN_CARDS()
	{
		this.cards.Clear();
		// 바닥 4장.
		this.cards.push(new CCard(1, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(1, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(2, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 2));

		// 1P 5장.
		this.cards.push(new CCard(1, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(5, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.YEOL, 0));

		// 2P 5장.
		this.cards.push(new CCard(2, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(4, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(7, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 2));

		// 바닥 4장.
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(4, PAE_TYPE.YEOL, 0));

		// 1P 5장.
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(0, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(11, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 3));

		// 2P 5장.
		this.cards.push(new CCard(11, PAE_TYPE.TEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(0, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(7, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.PEE, 3));

		this.cards.push(new CCard(1, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(2, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(3, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(4, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(8, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(8, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(0, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 2));
		this.cards.push(new CCard(5, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(7, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(10, PAE_TYPE.KWANG, 0));
		this.cards.push(new CCard(11, PAE_TYPE.YEOL, 1));
		this.cards.push(new CCard(10, PAE_TYPE.PEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.TEE, 1));
		this.cards.push(new CCard(9, PAE_TYPE.YEOL, 0));
		this.cards.push(new CCard(9, PAE_TYPE.PEE, 3));
		this.cards.push(new CCard(6, PAE_TYPE.PEE, 3));
	}

    fill_to(target)
    {
        this.cards.ForEach(obj => target.Enqueue(obj));
    }
}
