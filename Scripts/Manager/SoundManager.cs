using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : Singleton<SoundManager>
{
    // Resources/Sounds 폴더 안의 사운드 소스들을 저장하는 딕셔너리
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private AudioMixer _audioMixer;

    private AudioSource _bgmAudioSource;
    private List<AudioSource> _effectAudioSources;

    private DataManager _dataManager;

    private void Awake()
    {
        if (_dataManager == null) _dataManager = DataManager.Instance;
        Debug.Assert(_dataManager != null, "Null Exception : DataManager");

        _audioMixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _effectAudioSources = new List<AudioSource>();
        _bgmAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];

        SetVolume(_dataManager.LoadGameSettings().BgmVolume, Enums.AudioType.Bgm);
        SetVolume(_dataManager.LoadGameSettings().EffectVolume, Enums.AudioType.Effect);

        _bgmAudioSource.dopplerLevel = 0;
    }

    private AudioClip LoadAudioClip(string name)
    {
        if (!audioClips.ContainsKey(name))
        {
            // 요청 시점에 사운드 클립이 없다면 Load
            AudioClip audioClip = Resources.Load<AudioClip>($"Sounds/{name}");
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip 로드 실패 : {name}");
                return null;
            }
            audioClips.Add(name, audioClip);
        }
        return audioClips[name];
    }

    // 배경 음악 또는 효과음 재생
    // 배경 음악 재생 : SoundManager.Instance.Play("오디오 클립 이름", AudioType.BGM);
    // 효과음 재생 : SoundManager.Instance.Play("오디오 클립 이름");
    public void Play(string audioClipName, Enums.AudioType audioType = Enums.AudioType.Effect)
    {
        AudioClip audioClip = LoadAudioClip(audioClipName);

        switch (audioType)
        {
            case Enums.AudioType.Bgm:
                if (_bgmAudioSource.isPlaying) _bgmAudioSource.Stop();
                _bgmAudioSource.clip = audioClip;
                SetVolume(_dataManager.LoadGameSettings().BgmVolume, Enums.AudioType.Bgm);
                _bgmAudioSource.loop = true;
                _bgmAudioSource.Play();
                break;
            case Enums.AudioType.Effect:
                SetVolume(_dataManager.LoadGameSettings().EffectVolume, Enums.AudioType.Effect);
                AudioSource effectAudioSource = GetEffectAudioSource();
                effectAudioSource.clip = audioClip;
                effectAudioSource.Play();
                break;
        }
    }

    private AudioSource GetEffectAudioSource()
    {
        foreach (var audioSource in _effectAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
                audioSource.dopplerLevel = 0;
                return audioSource;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        _effectAudioSources.Add(newAudioSource);

        newAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];

        return newAudioSource;
    }

    // 재생 중인 배경 음악 멈추기
    public void Stop()
    {
        if (_bgmAudioSource.isPlaying) _bgmAudioSource.Stop();
    }

    public void Stop(string audioClipName)
    {
        foreach (var audioSource in _effectAudioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClips[audioClipName])
            {
                audioSource.Stop(); 
            }
        }
    }

    public void SetVolume(float volume, Enums.AudioType audioType)
    {
        switch (audioType)
        {
            case Enums.AudioType.Bgm:
                _audioMixer.SetFloat("BGM", Mathf.Clamp(Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20, -80, 0));
                break;

            case Enums.AudioType.Effect:
                _audioMixer.SetFloat("SFX", Mathf.Clamp(Mathf.Log10(Mathf.Max(volume, 0.0001f)) *20, -80, 20));
                break;
        }
    }

    public void FadeBGM(bool isFadeIn)  // 페이드 아웃 적용 시 BGM이 멈추기 때문에 필요한 시점에서 Play를 호출해야 함
    {
        if (_bgmAudioSource == null || !_bgmAudioSource.isPlaying) return;
        float originalVolume = _bgmAudioSource.volume;

        if (isFadeIn)
        {
            _bgmAudioSource.volume = 0;
            _bgmAudioSource.DOFade(originalVolume, 1f).SetEase(Ease.OutCirc).SetUpdate(true).OnComplete(() =>
            {
                _bgmAudioSource.volume = originalVolume;
            });
        }
        else
        {
            _bgmAudioSource.DOFade(0, 1f).SetEase(Ease.OutCirc).SetUpdate(true).OnComplete(() =>
            {
                _bgmAudioSource.volume = originalVolume;
                Stop();
            });
        }
    }

    public bool IsPlaying(string audioClipName, Enums.AudioType audioType = Enums.AudioType.Effect)
    {
        switch (audioType)
        {
            case Enums.AudioType.Bgm:
                return _bgmAudioSource.isPlaying && _bgmAudioSource.clip == audioClips[audioClipName];
            case Enums.AudioType.Effect:
                foreach (var audioSource in _effectAudioSources)
                {
                    if (audioSource.isPlaying && audioSource.clip == audioClips[audioClipName])
                    {
                        return true;
                    }
                }
                return false;
            default:
                return false;
        }
    }
}