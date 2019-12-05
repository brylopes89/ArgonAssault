using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip exploSFX;

    // Start is called before the first frame update
    void Start()
    {
        SetupSound();
    }

    private void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.2f;
        _audioSource.PlayOneShot(exploSFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
