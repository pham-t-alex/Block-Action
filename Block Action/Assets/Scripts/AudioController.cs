using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioClip _blockClick;
    public static AudioClip blockClick
    {
        get
        {
            if (_blockClick == null)
            {
                _blockClick = Resources.Load<AudioClip>("Audio/BlockClick");
            }
            return _blockClick;
        }
    }

    private static AudioClip _blockPlace;
    public static AudioClip blockPlace
    {
        get
        {
            if (_blockPlace == null)
            {
                _blockPlace = Resources.Load<AudioClip>("Audio/BlockPlace");
            }
            return _blockPlace;
        }
    }

    private static AudioClip _blockPlaceFail;
    public static AudioClip blockPlaceFail
    {
        get
        {
            if (_blockPlaceFail == null)
            {
                _blockPlaceFail = Resources.Load<AudioClip>("Audio/BlockPlaceFail");
            }
            return _blockPlaceFail;
        }
    }

    private static AudioController _audioController;
    public static AudioController audioController
    {
        get
        {
            if (_audioController == null)
            {
                _audioController = FindObjectOfType<AudioController>();
            }
            return _audioController;
        }
    }

    public AudioClip bgm;
    public AudioClip bgmRepeat;
    public AudioSource audioSource
    {
        get
        {
            return audioController.GetComponent<AudioSource>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bgm != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmRepeat;
                audioSource.Play();
            }
        }
    }

    public void PlayBGM(AudioClip newBgm, AudioClip repeatVer)
    {
        bgm = newBgm;
        if (repeatVer == null)
        {
            bgmRepeat = newBgm;
        }
        else
        {
            bgmRepeat = repeatVer;
        }
        audioSource.clip = bgm;
        audioSource.Play();
    }
}
