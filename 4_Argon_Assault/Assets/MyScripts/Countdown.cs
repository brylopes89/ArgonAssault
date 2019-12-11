using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float timeLeft = 5.0f;
    public bool stop = true;

    private float minutes;
    private float seconds;

    public Text text;

    public void startTimer(float from)
    {
        stop = false;
        timeLeft = from;
        Update();
        StartCoroutine(updateCoroutine());
    }

    void Update()
    {        

        if (stop) return;
        timeLeft -= Time.deltaTime;

        minutes = Mathf.Floor(timeLeft / 60);
        seconds = timeLeft % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            stop = true;
            minutes = 0;
            seconds = 0;
        }

        text.text = string.Format("Time Left : {0:0}:{1:00}", minutes, seconds);
        //        fraction = (timeLeft * 100) % 100;
    }

    IEnumerator updateCoroutine()
    {
        while (!stop)
        {
            text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            
            yield return new WaitForSeconds(0.2f);
        }
    }

}
