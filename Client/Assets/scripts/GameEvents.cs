using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private List<CCardPicture> tableCards = new List<CCardPicture>();
    private List<CCardPicture> handCards = new List<CCardPicture>();
    private List<CCardPicture> floorCards = new List<CCardPicture>();

    CPlayerHandCardManager CPlayerHandCardManager;

    public delegate void BlueArrow();
    public static event BlueArrow BlueArrowEvent;

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

    public void SetHandCards(List<CCardPicture> HandCards)
    {
        //handCards = CPlayerHandCardManager.GetHandCards();
    }

    public void SetFloorCards(List<CCardPicture> FloorCards)
    {
        floorCards = FloorCards;
    }
}
