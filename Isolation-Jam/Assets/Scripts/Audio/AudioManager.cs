using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { private set; get; }
    #endregion

    public AudioData[] backgroundMusicList;
    public AudioData[] sfxList;

    [SerializeField] private AudioData currentMusic;
    #region Settings
    [HideInInspector] public bool isMusicMuted { get { return musicVolume <= 0; } }
    [HideInInspector] public bool isSfxMuted { get { return sfxVolume <= 0; } }

    [HideInInspector] public float musicVolume = 1f;
    [HideInInspector] public float sfxVolume = 1f;
    #endregion

    void Awake()
    {
        CreateInstance();

        SetUpAudioArray(backgroundMusicList);

        SetUpAudioArray(sfxList);
    }

    void Start()
    {
        PlayMusic("Music");
    }

    private void CreateInstance()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }
    }

    private void SetUpAudioArray(AudioData[] _array)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            GameObject child = new GameObject(_array[i].Name);
            child.transform.parent = transform;

            AudioSource audioSource = child.AddComponent<AudioSource>();

            _array[i].go = child;
            _array[i].audioSource = audioSource;

            _array[i].audioSource.clip = _array[i].audioClip;
            _array[i].audioSource.volume = _array[i].volume;
            _array[i].audioSource.pitch = _array[i].pitch;
            _array[i].audioSource.loop = _array[i].isLooping;
        }
    }

    public void PlayMusic(string _name)
    {
        Debug.Log("Playing Music");
        AudioData data = Array.Find(backgroundMusicList, bgm => bgm.Name == _name);
        if (data == null)
        {
            Debug.LogError("Didnt find music");
            return;
        }
        else
        {
            currentMusic = data;
            currentMusic.audioSource.Play();
        }
    }

    public void PlaySFX(string _name)
    {
        AudioData data = Array.Find(sfxList, sfx => sfx.Name == _name);
        if (data == null)
        {
            Debug.LogError("Didn't find sound " + _name);
            return;
        }
        else
        {
            data.audioSource.Play();
        }
    }

    #region Settings
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        for (int i = 0; i < backgroundMusicList.Length; i++)
            backgroundMusicList[i].audioSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        for (int i = 0; i < sfxList.Length; i++)
            sfxList[i].audioSource.volume = volume;
    }
    #endregion
}
