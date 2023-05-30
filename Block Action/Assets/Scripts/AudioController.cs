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

    public string currentBGM;
    public AudioClip bgm;
    public AudioClip bgmRepeat;
    public AudioSource audioSource
    {
        get
        {
            return audioController.GetComponent<AudioSource>();
        }
    }
    private int notPlaying;
    private void Awake()
    {
        if (audioController != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        notPlaying = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgm != null)
        {
            if (!audioSource.isPlaying)
            {
                notPlaying++;
                if (notPlaying > 1)
                {
                    audioSource.clip = bgmRepeat;
                    audioSource.Play();
                    notPlaying = 0;
                }
            } else
            {
                notPlaying = 0;
            }
        }
    }

    public void PlayBGM(string bgmName)
    {
        if (bgmName == null)
        {
            bgmRepeat = null;
            bgm = null;
        }
        if (currentBGM == bgmName)
        {
            return;
        }
        AudioClip repeatVer = Resources.Load<AudioClip>($"Audio/{bgmName}R");
        AudioClip newBgm = Resources.Load<AudioClip>($"Audio/{bgmName}");
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
        currentBGM = bgmName;
        audioSource.Play();
    }
}
