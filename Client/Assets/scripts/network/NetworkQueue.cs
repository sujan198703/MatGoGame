using System.Collections.Generic;
using UnityEngine;

public class NetworkQueue : MonoBehaviour
{
    public static NetworkQueue instance;
    public  Queue<string> NetworkDataQueue = new Queue<string>();

    private void Awake()
    {
        instance = this;
    }

   public void AddToQueue(string str)
    {
        NetworkDataQueue.Enqueue(str);
        foreach(var item in NetworkDataQueue)
        {
           // print(item);
        }
    }
}
