using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}