using System.Collections.Generic;
using UnityEngine;

public enum Sfx
{
    UIOpen,
    UIClose,
    ChangeToggle,
    UIUpgrade,
    Magic_Fire,
    Magic_Ice,
    Magic_Light,
    Heal,
    Attack,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("=== BGM ===")] 
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource bgmPlayer;
    private AudioHighPassFilter bgmEffect;

    [Header("=== SFX ===")] 
    public List<AudioClip> sfxClips;
    public float sfxVolume;
    public int channels;
    private List<AudioSource> sfxPlayers;
    private int channelIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        //BGM
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        
        //SFX
        sfxPlayers = new();
        GameObject sfxObj = new GameObject("SfxPlayer");
        
        for (int i = 0; i < channels; i++)
        {
            AudioSource sfxPlayer = sfxObj.AddComponent<AudioSource>();
            sfxPlayer.playOnAwake = false;
            sfxPlayer.bypassListenerEffects = true;
            sfxPlayer.volume = sfxVolume;
            sfxPlayers.Add(sfxPlayer);
        }

        PlayBgm(true);
    }
    
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Count; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Count;
            
            if (sfxPlayers[loopIndex].isPlaying) continue;
            
            int ranIndex = 0;
            if (sfx == Sfx.Attack)
            {
                ranIndex = Random.Range(0, 2);
            }
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
