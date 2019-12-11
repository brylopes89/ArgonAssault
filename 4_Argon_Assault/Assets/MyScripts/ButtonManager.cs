using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    private CustomPointer pointer;

    private void Start()
    {
       
       
    }
    public void NewGameButton(int sceneIndex)
    {        
        StartCoroutine(LoadAsynchronously(sceneIndex));                       
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        print(sceneIndex);
        loadingScreen.SetActive(true);        

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
    public void ExitGameButton(string newGameLevel)
    {
        Application.Quit();
    }
}
