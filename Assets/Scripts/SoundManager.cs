using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    const float FADE_TIME_SECONDS = 10;

    public AudioMixer masterMixer;
    public AudioSource musicSource;
    public AudioSource[] sfxSources;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip footstepSound;
    public AudioClip fireBallSound;

    public string MasterVolumeKey = "MasterVolume";

    private void Awake()
    {
        if(Instance !=null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitializeAudioMixer());
        }
        //Debug.Log("Before Load");
        LoadVolume();
        //StartCoroutine(FadeIn());
    }


    private IEnumerator InitializeAudioMixer()
    {
        yield return null;

        float volume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        SetMasterVolume(volume);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Menu":
                PlayMenuMusic();
                break;
            case "NormalMap":
                PlayGameMusic();
                break;
        }
    }


    private void OnEnable()
    {
        //Debug.Log("OnLoad");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SetMasterVolume(float volume)
    {
        //Debug.Log("Set Volume");
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        Debug.Log(savedVolume);
        SetMasterVolume(savedVolume);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        AudioSource availableSource = GetAvailableSFXSource();
        availableSource.PlayOneShot(sfxClip);
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return sfxSources[0];
    }

    public void PlayMenuMusic()
    {
        StartCoroutine(FadeIn());
        SoundManager.Instance.PlayMusic(menuMusic);
    }

    public void PlayGameMusic()
    {
        StartCoroutine(FadeIn());
        SoundManager.Instance.PlayMusic(gameMusic);
    }

    public void PlayFootstepSound()
    {
        SoundManager.Instance.PlaySFX(footstepSound);
    }

    IEnumerator FadeIn()
    {
        float timeElapsed = 0f;

        while (timeElapsed < FADE_TIME_SECONDS)
        {
            musicSource.volume = Mathf.Lerp(0, 1, timeElapsed / FADE_TIME_SECONDS);
            //masterMixer.SetFloat(MasterVolumeKey, Mathf.Log10(Mathf.Lerp(0, 1, timeElapsed / FADE_TIME_SECONDS) * 20));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
