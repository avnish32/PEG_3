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

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void SetSFXVolume()
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolSlider.value) * 20);
    }
}
