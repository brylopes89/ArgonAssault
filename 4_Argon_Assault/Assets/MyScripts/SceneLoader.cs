using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float ChangeTime = 6f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadFirstScene", ChangeTime);
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
