using Papae.UnitySDK.Managers;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PauseOptions : MonoBehaviour
{
    // reference to the SceneOptions script
    private SceneOptions sceneOptions;
    // reference to the PanelOptions script
    private PanelOptions panelOptions;

    private FlightStates flightStates;
    private PlayerShootControl shoot;
    private PlayerFlightControl player;

    [HideInInspector] public bool isPaused = false;

    void Awake()
	{
        // retrieve the attached PanelOptions script
        panelOptions = GetComponent<PanelOptions>();
        // retrieve the attached SceneOptions script
        sceneOptions = GetComponent<SceneOptions>();

        flightStates = FindObjectOfType<FlightStates>();
        shoot = FindObjectOfType<PlayerShootControl>();
        player = FindObjectOfType<PlayerFlightControl>();
	}

	// Update is called once per frame
	void Update ()
    {
		// is Escape key pressed while the game is not paused and that we're not in main menu
		if (CrossPlatformInputManager.GetButtonDown ("Cancel") && !isPaused && !sceneOptions.IsInMainMenu) // 
        {
            // pause the game
            Pause();
		} 
		// if game is paused and not in main menu
		else if (CrossPlatformInputManager.GetButtonDown ("Cancel") && isPaused && !sceneOptions.IsInMainMenu) // 
		{
            // unpause the game
            Resume();
            StartCoroutine(ResumePlay());
		}
	}


	public void PauseGame()
	{
		AudioManager.Instance.PlayOneShot(AudioManager.Instance.LoadClip("button"), Pause);
    }

    void Pause()
    {
        isPaused = true;
        // display the pause menu
        panelOptions.ShowPauseMenu();
        // this will cause animations and physics to stop updating
        Time.timeScale = 0;
        //stopping shooting functionality
        shoot.enabled = false;
        //stop liftoff trigger from activating when pressing submit button
        flightStates.enabled = false;

        //player.GetComponentInParent<AudioSource>().enabled = false;
    }

    public void UnpauseGame()
	{
		AudioManager.Instance.PlayOneShot(AudioManager.Instance.LoadClip("button"));
        Resume();

        StartCoroutine(ResumePlay());
	}

    IEnumerator ResumePlay() //to do: move to flight control script
    {     
        panelOptions.HidePauseMenu();
        Time.timeScale = 1;
        isPaused = false;        

        yield return new WaitForSeconds(.5f);
        shoot.enabled = true;
        flightStates.enabled = true;
        //player.GetComponentInParent<AudioSource>().enabled = true;
    }

    void Resume()
    {
        // this will cause animations and physics to continue updating at regular speed
        

        
        // hide the pause menu
        
    }

}
