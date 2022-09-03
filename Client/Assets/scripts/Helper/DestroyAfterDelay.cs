using System.Collections;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay;

    void Start() => StartCoroutine(WaitAndDestroy());
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(gameObject);
    }
}
