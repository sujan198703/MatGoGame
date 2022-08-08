using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private List<CCardPicture> tableCards = new List<CCardPicture>();
    private List<CCard> handCards = new List<CCard>();
    private List<CCard> floorCards = new List<CCard>();

    CPlayerHandCardManager CPlayerHandCardManager;
    CPlayRoomUI CPlayRoomUI;

    public delegate void BlueArrow();
    public static event BlueArrow BlueArrowEvent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DisplayCards();
    }

    public void DisplayCards()
    {
        print(handCards.Count);
    }

    private List<CCardPicture> IntersectCards(List<CCardPicture> ListA, List<CCardPicture> ListB)
    {
        var result = ListA.Intersect(ListA, (IEqualityComparer<CCardPicture>)ListB);
        return (List<CCardPicture>)result;
    }

    public void SetHandCards(CCard HandCards)
    {
        handCards.Add(HandCards);
    }

    public void SetFloorCards(CCard FloorCards)
    {
        floorCards.Add(FloorCards);
    }
}
