using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundScript : MonoBehaviour
{
    public GameObject player;
    public AudioMixer shipMix;
    public AudioMixer enemyMix;
    public AudioMixer worldMix;

    public List<GameObject> Enemies;
    public List<AudioClip> Engine;
    public List<AudioClip> projectiles;
    public List<AudioClip> Thrusters;
    public List<AudioClip> Score;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.AddTracks(2, gameObject);

        SoundManager.TrackSettings(0, worldMix, "Score1", 0.5f, true);
        SoundManager.TrackSettings(1, worldMix, "Score2", 0.5f, true);
        //SoundManager.TrackSettings(0, shipMix, "Score", 0.5f, true);

        SoundManager.PlayMusic(0, Score[0]);
        SoundManager.PlayMusic(1, Score[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
