using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float gameTime = 240f;
    public float restartTime = 5f;
    public bool stop = true;
    public Text[] text;
    public Animator isGameOver;

    private float gameMinutes;
    private float restartMinutes;
    private float gameSeconds;
    private float restartSeconds;
    private int index = 0;    

    private void Start()
    {
          
    }

    void Update()
    {        
        if (stop)
            return;

        gameTime -= Time.deltaTime;

        if (isGameOver.GetCurrentAnimatorStateInfo(0).IsName("GameOver"))
            restartTime -= Time.deltaTime;

        gameMinutes = Mathf.Floor(gameTime / 60);
        restartMinutes = Mathf.Floor(restartTime / 60);

        gameSeconds = gameTime % 60;
        restartSeconds = restartTime % 60;

        if (gameSeconds > 59)
            gameSeconds = 59;

        if (gameMinutes < 0)
        {
            stop = true;
            gameMinutes = 0;
            gameSeconds = 0;           
        }

        if(restartMinutes < 0)
        {
            stop = true;
            restartMinutes = 0;
            restartSeconds = 0;
            SceneManager.LoadScene(1);
        }

        text[0].text = string.Format("{0:0}:{1:00}", gameMinutes, gameSeconds);
        text[1].text = string.Format("Restarting In : {1:00}", restartMinutes, restartSeconds);        
    }
}
