using System.Collections;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    public GameObject logo, contentRating;

    void Start() => StartCoroutine(NextScreen());

    IEnumerator NextScreen()
    {
        yield return new WaitForSeconds(2.0f);
        contentRating.SetActive(true);
        StartCoroutine(NextScene());
    }

    // Go to Login Scene
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
