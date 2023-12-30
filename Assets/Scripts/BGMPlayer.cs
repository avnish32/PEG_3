using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _BGMs;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource.loop = true;

        _audioSource.clip = _BGMs[SceneManager.GetActiveScene().buildIndex%_BGMs.Length];
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
