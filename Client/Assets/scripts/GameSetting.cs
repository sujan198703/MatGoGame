using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    static public GameSetting instance;
    public int turn_time = 5;
    void Start()
    {
        if (instance == null) instance = this;
    }
}
