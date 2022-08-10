using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenPanel : MonoBehaviour
{
    [SerializeField] Slider loadingProgressBar;

    void Start() => LoadScene();

    public void LoadScene()
    {
        StartCoroutine(AsynchronousLoad("Lobby"));
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // Update Loading Slider with Progress
            loadingProgressBar.value = ao.progress;

            // Loading completed
            if (ao.progress == 0.9f)
            {
                loadingProgressBar.value = 1.0f;

                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
