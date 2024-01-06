using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    
    [SerializeField]
    private AudioClip[] _BGMs;

    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _sfxSource;

    [SerializeField]
    Slider _musicVolSlider;

    [SerializeField]
    Slider _sfxVolSlider;

    private const string MUSIC_VOLUME_PLAYER_PREFS_KEY = "musicVolume";
    private const string SFX_VOLUME_PLAYER_PREFS_KEY = "sfxVolume";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_PLAYER_PREFS_KEY))
        {
            _musicVolSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_PLAYER_PREFS_KEY);
        }

        if (PlayerPrefs.HasKey(SFX_VOLUME_PLAYER_PREFS_KEY))
        {
            _sfxVolSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_PLAYER_PREFS_KEY);
        }

        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolSlider.value) * 20);
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolSlider.value) * 20);

        _musicSource.loop = true;
        _musicSource.clip = _BGMs[SceneManager.GetActiveScene().buildIndex%_BGMs.Length];
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        _sfxSource.PlayOneShot(sfxClip);
    }

    public void SetMusicVolume()
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolSlider.value)*20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PLAYER_PREFS_KEY, _musicVolSlider.value);
    }

    public void SetSFXVolume()
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolSlider.value) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME_PLAYER_PREFS_KEY , _sfxVolSlider.value);
    }
}
