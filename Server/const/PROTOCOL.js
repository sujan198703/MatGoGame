
/// <summary>
/// 프로토콜 정의.
/// 서버에서 클라이언트로 가는 패킷 : S -> C
/// 클라이언트에서 서버로 가는 패킷 : C -> S
/// </summary>

exports.BEGIN = 0

// 시스템 프로토콜.
exports.GAME_SERVER_STARTED = 2	// S -> C

// 게임 프로토콜.
exports.READY_TO_START = 10		// C -> S
exports.BEGIN_CARD_INFO = 11		// S -> C
exports.DISTRIBUTED_ALL_CARDS = 12	// C -> S

exports.SELECT_CARD_REQ = 13		// C -> S
exports.SELECT_CARD_ACK = 14		// S -> C

// 플레이어가 두개의 카드 중 하나를 선택해야 하는 경우.
//CHOICE_ONE_CARD = 15,		// S -> C   // 사용하지 않음.
exports.CHOOSE_CARD = 16			// C -> S

exports.FLIP_BOMB_CARD_REQ = 17	// C -> S

exports.FLIP_DECK_CARD_REQ = 18	// C -> S
exports.FLIP_DECK_CARD_ACK = 19	// S -> C

exports.TURN_RESULT = 20			// S -> C

exports.ASK_GO_OR_STOP = 21		// S -> C
exports.ANSWER_GO_OR_STOP = 22		// C -> S
exports.NOTIFY_GO_COUNT = 23		// S -> C

exports.UPDATE_PLAYER_STATISTICS = 24	// S -> C

exports.ASK_KOOKJIN_TO_PEE = 25		// S -> C
exports.ANSWER_KOOKJIN_TO_PEE = 26		// C -> S

exports.MOVE_KOOKJIN_TO_PEE = 27		// S -> C

exports.GAME_RESULT = 28				// S -> C

exports.PLAYER_ORDER_RESULT = 29	// S -> C

exports.GAME_SCORE = 30 // S-> C

exports.TIME_OUT = 97;
exports.START_TURN = 98
exports.START_TURN_OTHER = 99
exports.TURN_END = 100

exports.END = 101;









