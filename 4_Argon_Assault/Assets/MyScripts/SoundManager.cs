using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static List<TrackClass> trackList = new List<TrackClass>();
    public static SoundManager instance;

    private static float clipLength;
    private static bool keepFadingIn;
    private static bool keepFadingOut;

    private void Awake()
    {
        instance = this;
    }
    
    static public void AddTracks(int numberOfTracks, GameObject gameObj)
    {
        if(numberOfTracks != 0)
        {
            for(int i = 0; i< numberOfTracks; i++)
            {
                TrackClass track = new TrackClass { id = i, audioSource = gameObj.AddComponent<AudioSource>()};
                trackList.Add(track);                
            }
        }
        
    }
    static public void TrackSettings(int track, AudioMixer mainMix, string audioGroup, float trackVolume, bool loop = false)
    {
        trackList[track].audioSource.outputAudioMixerGroup = mainMix.FindMatchingGroups(audioGroup)[0];
        trackList[track].trackVolume = trackVolume;
        trackList[track].loop = loop;
    }


    static public void PlayMusic(int track, AudioClip audioClip = null, List<AudioClip> listAudioclip = null, int min = -2, int max = -2)
    {
        //Play a clip one time or looped
        if (audioClip != null && listAudioclip == null && trackList[track].audioSource.isPlaying == false)
        {
            trackList[track].audioSource.PlayOneShot(audioClip, trackList[track].trackVolume);
            Debug.Log("Now Playing: " + audioClip.name);

            if (trackList[track].loop)
            {
                clipLength = audioClip.length;
                LoopCaller(track, clipLength, audioClip, null, min, max);
            }
        }

        //Play from a List randomly, looped or not
        if (audioClip == null && listAudioclip != null && trackList[track].audioSource.isPlaying == false)
        {
            int index = Random.Range(min, max);

            if (index == -1)
            {
                Debug.Log("No sound because -1");
            }
            else
            {
                Debug.Log("Playing: " + listAudioclip[index].name);
                trackList[track].audioSource.PlayOneShot(listAudioclip[index], trackList[track].trackVolume);
                clipLength = listAudioclip[index].length;
            }

            if (trackList[track].loop)
            {
                LoopCaller(track, clipLength, audioClip, listAudioclip, min, max);
            }
        }
    }

    public static void FadeInCaller(int track, float speed, float maxVolume)
    {
        instance.StartCoroutine(FadeIn(track, speed, maxVolume));
    }

    public static void FadeOutCaller(int track, float speed)
    {
        instance.StartCoroutine(FadeOut(track, speed));
    }

    public static void LoopCaller(int track, float clipLength, AudioClip audioClip, List<AudioClip> listAudioclip, int min, int max)
    {
        instance.StartCoroutine(Loop(track, clipLength, audioClip, listAudioclip, min, max));
    }

    public static void ChangeMusicCaller(int track, float speed, AudioClip audioClip, List<AudioClip> listAudioclip = null, int min = -2, int max = -2)
    {
        instance.StartCoroutine(ChangeMusic(track, speed, audioClip, listAudioclip, min, max));
    }

    static IEnumerator FadeIn (int track, float speed, float maxVolume)
    {
        keepFadingIn = true;
        keepFadingOut = false;

        trackList[track].audioSource.volume = 0;
        float audioVolume = trackList[track].audioSource.volume;

        while (trackList[track].audioSource.volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed;
            trackList[track].audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    static IEnumerator FadeOut(int track, float speed)
    {
        keepFadingIn = false;
        keepFadingOut = true;
        
        float audioVolume = trackList[track].audioSource.volume;
        
        while (trackList[track].audioSource.volume >= speed && keepFadingOut)
        {
            audioVolume -= speed;
            trackList[track].audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    static IEnumerator Loop (int track, float clipLength, AudioClip audioClip = null, List<AudioClip> listAudioclip = null, int min = -2, int max = -2)
    {
        yield return new WaitForSeconds(clipLength);

        PlayMusic(track, audioClip, listAudioclip, min, max);
    }

    static IEnumerator ChangeMusic(int track, float speed, AudioClip audioClip = null, List<AudioClip> listAudioclip = null, int min = -2, int max = -2)
    {
        FadeOutCaller(track, speed);

        while(trackList[track].audioSource.volume >= speed)
        {
            yield return new WaitForSeconds(0.01f);
        }

        trackList[track].audioSource.Stop();

        //Play a clip one time or looped
        if (audioClip != null)
        {
            Debug.Log("Now Playing: " + audioClip.name);
            trackList[track].audioSource.PlayOneShot(audioClip, trackList[track].trackVolume);                       
            clipLength = audioClip.length;
            FadeInCaller(track, speed, trackList[track].trackVolume);
            LoopCaller(track, clipLength, audioClip, null, min, max);            
        }

        //Play from a List randomly, looped or not
        if (listAudioclip != null)
        {
            int index = Random.Range(min, max);

            if (index == -1)
            {
                Debug.Log("No sound because -1");
            }
            else
            {
                Debug.Log("Playing: " + listAudioclip[index].name);
                trackList[track].audioSource.PlayOneShot(listAudioclip[index], trackList[track].trackVolume);
                clipLength = listAudioclip[index].length;
                FadeInCaller(track, speed, trackList[track].trackVolume);
                LoopCaller(track, clipLength, audioClip, listAudioclip, min, max);
            }
        }
    }
}
