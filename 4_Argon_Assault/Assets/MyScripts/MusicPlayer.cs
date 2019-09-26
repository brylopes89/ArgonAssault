using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Awake()
    {
        // if more than one music player in scene, destroy ourselves
        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;

        if (numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }        
    }

}