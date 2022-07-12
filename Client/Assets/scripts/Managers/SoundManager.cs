using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource bgmSource;
    public AudioSource voiceSource;
    public AudioSource ambientSource;

    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PlaySound(string audioClipName)
    {

    }
}
