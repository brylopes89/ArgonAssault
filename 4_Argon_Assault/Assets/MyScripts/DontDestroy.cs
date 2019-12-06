using UnityEngine;


public class DontDestroy : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Awake()
    {
        // if more than one music player in scene, destroy ourselves
        int numMusicPlayers = FindObjectsOfType<DontDestroy>().Length;

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