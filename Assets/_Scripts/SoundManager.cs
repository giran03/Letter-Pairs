using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] Audio[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSource, sfxSource;

    public static SoundManager Instance;
    AudioSource currentSfxLooping;
    AudioSource currentMusic;
    bool playedMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    private void Start() => PlayMusic("MenuBGM");
    private void Update()
    {
        if (SceneHandler.Instance.CurrentScene() == "_All-Cards")
        {
            Debug.Log($"All cards screen");
            currentMusic.volume = .2f;
        }
        else
            currentMusic.volume = .5f;
    }

    public void Button_CreditsBGM()
    {
        Debug.Log($"Credits Screen");
        PlayMusic("Credits BGM");
    }

    public void Button_MenuBGM()
    {
        Debug.Log($"Menu Screen");
        PlayMusic("MenuBGM");
    }

    public void PlayMusic(string clipName)
    {
        Debug.Log($"Called play music");
        Audio sound = Array.Find(musicSounds, x => x.clipName == clipName);
        if (sound != null)
        {
            musicSource.clip = sound.audioClip;
            musicSource.loop = true;
            musicSource.Play();
            currentMusic = musicSource;
        }
    }

    public void PlaySFX(string clipName, bool loop = false)
    {
        Audio audio = Array.Find(sfxSounds, x => x.clipName == clipName);
        if (audio != null)
        {
            sfxSource.loop = loop;

            if (loop)
            {
                if (currentSfxLooping != null)
                    currentSfxLooping.Stop();

                sfxSource.clip = audio.audioClip;
                sfxSource.Play();
                currentSfxLooping = sfxSource;
            }
            else
            {
                sfxSource.clip = audio.audioClip;
                // sfxSource.volume = 1f;
                AudioSource.PlayClipAtPoint(sfxSource.clip, Camera.main.transform.position);
                StartCoroutine(Cooldown(audio.audioClip));
            }
        }
    }

    public void StopMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
            currentMusic = null;
        }
    }

    public void StopSFX()
    {
        if (currentSfxLooping != null)
        {
            currentSfxLooping.Stop();
            currentSfxLooping = null;
        }
    }

    IEnumerator Cooldown(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
    }
}
