using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataManager
{
    public int nyangs;
    public int rubies;
    public int yellowChips;
    public int greenChips;

    public PlayerDataManager()
    {
        this.nyangs = 0;
        this.rubies = 0;
        this.yellowChips = 0;
        this.greenChips = 0;
    }
}
