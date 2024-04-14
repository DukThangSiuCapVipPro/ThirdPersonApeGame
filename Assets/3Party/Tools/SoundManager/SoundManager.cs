using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TruongNT;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    #region Inspector
    public AudioSource music;
    public AudioMixer mixer;
    public AudioMixerGroup musicGroup, sfxGroup, voiceGroup;
    public SortedList<int, AudioSource> playingSound;
    public SortedList<int, LoopSoundRequest> loopSound;
    public SortedList<int, AudioSource> voiceSound;
    public AudioClip[] battleMusics;
    public AudioClip[] homeMusics;
    public AudioClip clickSound;
    public AudioClip claimReward;
    public AudioClip purchaseSound;
    public SoundObject soSample;
    #endregion;

    #region Variables
    private Dictionary<int, SoundObject> soDict = new Dictionary<int, SoundObject>();
    private Queue<SoundObject> soQueue = new Queue<SoundObject>();

    public bool Music_Setting
    {
        get
        {
            return musicSetting;
        }
        set
        {
            musicSetting = value;
            PlayerPrefs.SetInt("MusicSetting", musicSetting ? 1 : 0);
        }
    }
    public bool musicSetting;
    public bool SFX_Setting
    {
        get
        {
            return sFXSetting;
        }
        set
        {
            sFXSetting = value;
            PlayerPrefs.SetInt("SFXSetting", musicSetting ? 1 : 0);
        }
    }
    public bool sFXSetting;
    public float Music_Volume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }
    }
    public float musicVolume;
    public float SFX_Volume
    {
        get
        {
            return sFXVolume;
        }
        set
        {
            sFXVolume = value;
            PlayerPrefs.SetFloat("SFXVolume", sFXVolume);
        }
    }
    public float sFXVolume;
    #endregion

    #region Unity Methods
    private void Start()
    {
        musicSetting = PlayerPrefs.GetInt("MusicSetting", 1) == 1;
        sFXSetting = PlayerPrefs.GetInt("SFXSetting", 1) == 1;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 80);
        sFXVolume = PlayerPrefs.GetFloat("SFXVolume", 80);

        playingSound = new SortedList<int, AudioSource>();
        loopSound = new SortedList<int, LoopSoundRequest>();
        voiceSound = new SortedList<int, AudioSource>();

        PrepareQueue();
        LoadSettings();
    }

    void HandlePressButton(object ob1, object ob2)
    {
        PlayUIButtonClick();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    #endregion;

    #region Public Methods
    public void LoadSettings()
    {
        MusicVolume(Music_Volume);
        SoundVolume(SFX_Volume);
        if (Music_Setting)
            MusicOn();
        else
            MusicOff();
        if (SFX_Setting)
            SoundOn();
        else
            SoundOff();
    }
    public AudioSource AddAudioSource(AudioClip sfx, bool isSFX = true)
    {
        AudioSource ac = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        ac.outputAudioMixerGroup = isSFX ? sfxGroup : musicGroup;
        ac.loop = false;
        ac.playOnAwake = false;
        ac.clip = sfx;
        playingSound.Add(sfx.GetInstanceID(), ac);
        return ac;
    }

    public LoopSoundRequest AddLoopSoundRequest(AudioClip sfx, int requesterId, bool isSFX = true)
    {
        AudioSource ac = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        ac.outputAudioMixerGroup = isSFX ? sfxGroup : musicGroup;
        ac.loop = true;
        ac.playOnAwake = true;
        ac.clip = sfx;
        LoopSoundRequest r = new LoopSoundRequest();
        r.source = ac;
        r.requester = requesterId;
        loopSound.Add(sfx.GetInstanceID(), r);
        return r;
    }

    public void PlayVoiceRewind(AudioClip voice)
    {
        if (voice == null)
            return;
        int id = voice.GetInstanceID();
        if (voiceSound.ContainsKey(id))
        {
            voiceSound[id].Stop();
            voiceSound[id].Play();
        }
        else
        {
            AudioSource ac = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            ac.outputAudioMixerGroup = voiceGroup;
            ac.loop = false;
            ac.playOnAwake = false;
            ac.clip = voice;
            voiceSound.Add(voice.GetInstanceID(), ac);
            ac.Play();
        }
    }

    /// <summary>
    /// stop the sfx and replay it
    /// </summary>
    public void PlaySfxRewind(AudioClip sfx, float volume = 1)
    {
        if (sfx == null)
            return;
        int id = sfx.GetInstanceID();
        if (playingSound.ContainsKey(id))
        {
            playingSound[id].Stop();
            playingSound[id].volume = volume;
            playingSound[id].Play();
        }
        else
        {
            AudioSource ac = AddAudioSource(sfx);
            ac.volume = volume;
            ac.Play();
        }
    }

    /// <summary>
    /// Play the sfx if it is not being played at the moment 
    /// </summary>
    public void PlaySfxNoRewind(AudioClip sfx, float volume = 1)
    {
        if (sfx == null)
            return;
        int id = sfx.GetInstanceID();
        IList<AudioSource> lstSFX = playingSound.Values;
        int numberPlaying = 0;
        foreach (var sf in lstSFX)
        {
            numberPlaying += sf.isPlaying ? 1 : 0;
        }
        if (numberPlaying > 5)
            return;
        if (playingSound.ContainsKey(id))
        {
            if (!playingSound[id].isPlaying)
            {
                playingSound[id].Play();
                playingSound[id].volume = volume;
            }
        }
        else
        {
            AudioSource ac = AddAudioSource(sfx);
            ac.volume = volume;
            ac.Play();
        }
    }

    /// <summary>
    /// play the sfx one more no matter it is playing or not
    /// </summary>
    public void PlaySfxOverride(AudioClip sfx, float volume = 1)
    {
        if (sfx == null)
            return;
        int id = sfx.GetInstanceID();
        if (playingSound.ContainsKey(id))
        {
            playingSound[id].volume = volume;
            playingSound[id].PlayOneShot(sfx);
        }
        else
        {
            AudioSource ac = AddAudioSource(sfx);
            ac.volume = volume;
            ac.Play();
        }
    }

    public void PlaySfxLoop(AudioClip sfx, int requesterId)
    {
        if (sfx == null)
            return;
        int id = sfx.GetInstanceID();
        if (loopSound.ContainsKey(id))
        {
            loopSound[id].requester = requesterId;
            if (!loopSound[id].source.isPlaying)
                loopSound[id].source.Play();
        }
        else
        {
            AddLoopSoundRequest(sfx, requesterId).source.Play();
        }
    }

    public bool StopLoopSound(AudioClip sfx, int requesterId)
    {
        if (sfx == null)
            return false;
        int id = sfx.GetInstanceID();
        if (loopSound.ContainsKey(id))
        {
            LoopSoundRequest lsr = loopSound[id];
            if (lsr.requester == requesterId)
            {
                lsr.source.Stop();
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public void PauseMusic()
    {
        music.Pause();
    }

    public void MusicVolume(float value)
    {
        mixer.SetFloat("music", -80 + value);
        Music_Volume = value;
    }
    public void MusicOn()
    {
        mixer.SetFloat("music", -80 + Music_Volume);
        Music_Setting = true;
    }

    public void MusicOff()
    {
        mixer.SetFloat("music", -80);
        Music_Setting = false;
    }

    public void SoundVolume(float value)
    {
        mixer.SetFloat("sfx", -85 + value);
        mixer.SetFloat("voice", -80 + value);
        SFX_Volume = value;
    }
    public void SoundOn()
    {
        mixer.SetFloat("sfx", -85 + SFX_Volume);
        mixer.SetFloat("voice", -80 + SFX_Volume);
        SFX_Setting = true;
    }

    public void SoundOff()
    {
        mixer.SetFloat("sfx", -85);
        mixer.SetFloat("voice", -80);
        SFX_Setting = false;
    }
    public void TurnOff(string soundType)
    {
        mixer.SetFloat(soundType, -80);
    }

    private Coroutine nextAudio;
    public void PlayMusic(AudioClip clip, bool isLoop = true, List<AudioClip> list = null)
    {
        if (autoTask != null)
            StopCoroutine(autoTask);
        if (nextAudio != null)
            StopCoroutine(nextAudio);
        if (music.isPlaying)
            music.Stop();
        music.clip = clip;
        music.Play();
        music.loop = isLoop;
        if (!isLoop)
        {
            nextAudio = StartCoroutine(Tools.Delay(clip.length, delegate
            {
                clip = list[Random.Range(0, list.Count)];
                PlayMusic(clip, isLoop, list);
            }));
        }
    }

    public void PlayHomeMusic()
    {

        if (autoTask != null)
            StopCoroutine(autoTask);
        if (music.isPlaying)
            music.Stop();
        music.clip = homeMusics[0];
        music.Play();
        music.loop = true;
    }

    public void PlayBattleMusic()
    {
        if (autoTask != null)
            StopCoroutine(autoTask);
        if (music.isPlaying)
            music.Stop();
        music.clip = battleMusics[0];
        music.Play();
        music.loop = true;
    }

    Coroutine autoTask;
    public void PlayGameMusicInList(int _id = -1)
    {
        if (autoTask != null)
            StopCoroutine(autoTask);
        if (music.isPlaying)
            music.Stop();
        if (_id == -1)
            _id = Random.Range(0, homeMusics.Length);
        else
        {
            if (_id >= homeMusics.Length)
                _id = 0;
        }
        music.clip = homeMusics[_id];
        music.Play();
        autoTask = StartCoroutine(AutoNextSong(_id + 1, music.clip.length + 0.3f));
    }

    private IEnumerator AutoNextSong(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayGameMusicInList(id);
    }

    public void PlayUIButtonClick()
    {
        PlaySfxOverride(clickSound);
    }

    public void PlayClaimReward()
    {
        PlaySfxRewind(claimReward);
    }
    public void PlayPurchaseSound()
    {
        PlaySfxRewind(purchaseSound);
    }
    #endregion;

    #region SO Methods
    public void AssignSO(GameObject go, AudioClip clip, bool loop = false)
    {
        SoundObject so = null;
        if (soDict.ContainsKey(go.GetInstanceID()))
        {
            so = soDict[go.GetInstanceID()];
            so.PlaySound(go, clip, loop);
        }
        else
        {
            so = PopSO();
            soDict.Add(go.GetInstanceID(), so);
            so.PlaySound(go, clip, loop);
        }
    }
    public void RecallSO(SoundObject so)
    {
        if (soDict.ContainsKey(so.id))
        {
            soDict.Remove(so.id);
        }
        PushSO(so);
    }

    private void PrepareQueue()
    {
        for (int i = 0; i < 10; i++)
        {
            SoundObject so = Instantiate(soSample);
            so.mTrans.parent = gameObject.transform;
            so.gameObject.SetActive(false);
            PushSO(so);
        }
    }
    private SoundObject PopSO()
    {
        if (soQueue.Count > 0)
        {
            return soQueue.Dequeue();
        }
        else
        {
            SoundObject so = Instantiate(soSample);
            return so;
        }
    }
    private void PushSO(SoundObject so)
    {
        so.mTrans.parent = gameObject.transform;
        so.mTrans.localPosition = Vector3.zero;
        so.gameObject.SetActive(false);
        soQueue.Enqueue(so);
    }
    #endregion

}

public class LoopSoundRequest
{
    public AudioSource source;
    public int requester;
}

/// <summary>
/// the way the sound fx is played
/// </summary>
public enum SFX_PLAY_STYLE
{
    /// <summary>
    /// restart the sound
    /// </summary>
    REWIND,
    /// <summary>
    /// if the same sound is playing, dont play this one
    /// </summary>
    DONT_REWIND,
    /// <summary>
    /// always play the sound
    /// </summary>
    OVERRIDE,
    NONE
}
