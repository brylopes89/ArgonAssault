using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static List<TrackClass> trackList = new List<TrackClass>();

    private static float clipLength;
    private static bool keepFadingIn;
    private static bool keepFadingOut;

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
                //Loop
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
                //Loop
            }
        }
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
}
