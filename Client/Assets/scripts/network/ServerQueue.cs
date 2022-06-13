using System.Collections.Generic;
using UnityEngine;

public class ServerQueue : MonoBehaviour
{
    public static ServerQueue instance;
    public Queue<string> ServerDataQueue = new Queue<string>();

    private void Awake()
    {
        instance = this;
    }

    public void AddToQueue(string str)
    {
        ServerDataQueue.Enqueue(str);
        foreach (var item in ServerDataQueue)
        {
            // print(item);
        }
    }

    public void Pop()
    {
        if (ServerDataQueue.Count > 0)
        {
            ServerDataQueue.Dequeue();
        }
    }
}
