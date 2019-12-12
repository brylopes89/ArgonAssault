using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    bool inFirstScene = true;
    bool inSecondScene = false;

    public bool IsInMainMenu
    {
        get { return SceneManager.GetActiveScene().buildIndex == 1; }
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.AddTracks(2, gameObject);
       
        SoundManager.TrackSettings(0, worldMix, "Score1", 0.5f, true);
        SoundManager.TrackSettings(1, worldMix, "Score2", 0.5f, true);

        if (IsInMainMenu)
        {
            SoundManager.PlayMusic(0, Score[0]);
        }
       
        SoundManager.FadeInCaller(0, 0.01f, SoundManager.trackList[0].trackVolume);      

    }

    // Update is called once per frame
    void Update()
    {    

        if (SceneManager.GetActiveScene().buildIndex == 1 && !inSecondScene)
        {
            inFirstScene = false;
            inSecondScene = true;

            //SoundManager.FadeOutCaller(0, 0.05f);
           // SoundManager.ChangeMusicCaller(1, 0.05f, Score[1]);
        }

        
    }
}
