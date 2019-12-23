using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MenuClick : MonoBehaviour
{ 
    enum Screen { PauseMenu, OptionsMenu, MainMenu, Game};
    Screen currentScreen = Screen.MainMenu;

    public Text[] pauseMenu;
    public Toggle[] toggle;    
    public Text settingsMenu;    

    public bool IsUp, IsDown;

    private bool isPauseMenu = false;
    private bool isOptionsMenu = false;
    private bool isMainMenu = false;
    private bool isGame = false;

    private bool switchCase = false;
    private bool istoggle = false;
    private bool changeFromMain = false;
    private bool changeFromPause = false;

    private float _LastY;
    private float y;

    private int numberOfOptions = 3;
    private int selectedOption;

    private PauseOptions pauseOptions;
    private SceneOptions displayOptions;
    private SoundSettings togglePress;

    // Use this for initialization
    void Start()
    {
        pauseOptions = FindObjectOfType<PauseOptions>();
        displayOptions = FindObjectOfType<SceneOptions>();
        togglePress = FindObjectOfType<SoundSettings>();        

        selectedOption = 1;

        CheckScreen();

    }
    // Update is called once per frame
    void Update()
    {
        y = CrossPlatformInputManager.GetAxis("UpDown");
        IsUp = false;
        IsDown = false;

        if (_LastY != y)
        {
            if (y == -1)
                IsDown = true;
            else if (y == 1)
                IsUp = true;
        }       
        _LastY = y;        

        if (IsDown)
        { //Input telling it to go up or down.
            selectedOption += 1;            

            if (selectedOption > numberOfOptions) //If at end of list go back to top
            {
                selectedOption = 1;
            }         

            SelectColorChange();           
        }

        if (IsUp)
        { //Input telling it to go up or down.
            selectedOption -= 1;
            
            if (selectedOption < 1) //If at end of list go back to top
            {
                selectedOption = numberOfOptions;
            }

            SelectColorChange();           
        }

        if (CrossPlatformInputManager.GetButtonDown("Submit"))
        {            
            if (currentScreen == Screen.PauseMenu)
            {
                
                switch (selectedOption) //Set the visual indicator for which option you are on.
                {
                    case 1:
                        pauseOptions.UnpauseGame();
                        currentScreen = Screen.Game;                        
                        Debug.Log("Resume Pressed");
                        break;
                    case 2:
                        displayOptions.DisplayOptionsMenu(true);
                        changeFromPause = true;                        
                        currentScreen = Screen.OptionsMenu;
                        Debug.Log("Options Pressed");
                        break;
                    case 3:
                       
                        displayOptions.StartGame(false);                        
                                                                   
                        Debug.Log("Main Menu Pressed");
                        break;
                }
            }

            else if (currentScreen == Screen.MainMenu)
            {
                switch (selectedOption) //Set the visual indicator for which option you are on.
                {
                    case 1:
                        displayOptions.StartGame(true);
                        currentScreen = Screen.Game;
                        changeFromMain = true;                       
                        Debug.Log("New Game Pressed");
                        break;
                    case 2:
                        displayOptions.DisplayOptionsMenu(true);
                        changeFromMain = true;
                        currentScreen = Screen.OptionsMenu;
                        Debug.Log("Options Pressed");
                        break;
                    case 3:
                        displayOptions.QuitGame();
                        Debug.Log("Quit Pressed");
                        break;
                }                
            }           

            else if (currentScreen == Screen.OptionsMenu)
            {
                switchCase = true;
                istoggle = !istoggle;

                switch (selectedOption)
                {
                    case 1:
                        if (toggle[0].isOn)
                            toggle[0].isOn = false;
                        else
                            toggle[0].isOn = true;
                        Debug.Log("Toggle Music");
                        break;
                    case 2:
                        if (toggle[1].isOn)
                            toggle[1].isOn = false;
                        else
                            toggle[1].isOn = true;
                        break;

                    case 3:
                        displayOptions.DisplayOptionsMenu(false);
                        if (changeFromMain)
                        {                            
                            currentScreen = Screen.MainMenu;                            
                        }
                        else if (changeFromPause)
                        {                            
                            currentScreen = Screen.PauseMenu;
                            
                        }
                        switchCase = false;
                        changeFromPause = false;
                        changeFromMain = false;
                        break;
                }
            }            
        }

    }
    void CheckScreen()
    {
        if (displayOptions.IsInMainMenu)
        {
            currentScreen = Screen.MainMenu;
        }

        else if (displayOptions.IsInGame)
        {
            currentScreen = Screen.Game;
        }

        else if (pauseOptions.isPaused)
        {
            currentScreen = Screen.PauseMenu;
        }

        else if (displayOptions.isInOptions)
        {
            currentScreen = Screen.OptionsMenu;
        }

        ScreenValues();
    }

    void ScreenValues()
    {
        if (currentScreen == Screen.PauseMenu)
        {
            isPauseMenu = true;
            isOptionsMenu = false;
            isGame = false;
            isMainMenu = false;

            pauseMenu[0].color = new Color32(177, 245, 245, 255);
            pauseMenu[1].color = new Color32(0, 0, 0, 255);
            pauseMenu[2].color = new Color32(0, 0, 0, 255);
        }

        else if (currentScreen == Screen.OptionsMenu)
        {            
            isOptionsMenu = true;
            isPauseMenu = false;
            isGame = false;
            isMainMenu = false;

            toggle[0].GetComponentInChildren<Text>().color = new Color32(177, 245, 245, 255);
            toggle[1].GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
            settingsMenu.color = new Color32(0, 0, 0, 255);
        }

        else if (currentScreen == Screen.MainMenu)
        {
            isMainMenu = true;

            isOptionsMenu = false;
            isGame = false;
            isPauseMenu = false;
        }

        else if (currentScreen == Screen.Game)
        {
            isPauseMenu = false;
            isOptionsMenu = false;
            isMainMenu = false;            
        }
    }

    private void SelectColorChange()
    {
        if (currentScreen == Screen.PauseMenu)
        {
            pauseMenu[0].color = new Color32(0, 0, 0, 255);
            pauseMenu[1].color = new Color32(0, 0, 0, 255);
            pauseMenu[2].color = new Color32(0, 0, 0, 255);

            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 1:
                    pauseMenu[0].color = new Color32(177, 245, 245, 255);
                    
                    break;
                case 2:

                    pauseMenu[1].color = new Color32(177, 245, 245, 255);
                    
                    break;
                case 3:
                    pauseMenu[2].color = new Color32(177, 245, 245, 255);
                   
                    break;
            }
        }

        if (currentScreen == Screen.MainMenu)
        {
            pauseMenu[0].color = new Color32(0, 0, 0, 255);
            pauseMenu[1].color = new Color32(0, 0, 0, 255);
            pauseMenu[2].color = new Color32(0, 0, 0, 255);

            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 1:
                    pauseMenu[0].color = new Color32(177, 245, 245, 255);

                    break;
                case 2:

                    pauseMenu[1].color = new Color32(177, 245, 245, 255);

                    break;
                case 3:
                    pauseMenu[2].color = new Color32(177, 245, 245, 255);

                    break;
            }
        }

        if(currentScreen == Screen.Game)
        {
            if (pauseOptions.isPaused)
            {
                currentScreen = Screen.PauseMenu;
            }
        }

        else if (currentScreen == Screen.OptionsMenu)
        {
            toggle[0].GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
            toggle[1].GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255);
            settingsMenu.color = new Color32(0, 0, 0, 255);

            switch (selectedOption) //Set the visual indicator for which option you are on.
            {
                case 1:
                    
                    toggle[0].GetComponentInChildren<Text>().color = new Color32(177, 245, 245, 255);
                    break;
                case 2:

                    
                    toggle[1].GetComponentInChildren<Text>().color = new Color32(177, 245, 245, 255);
                    break;
                case 3:
                    
                    settingsMenu.color = new Color32(177, 245, 245, 255);
                    break;
            }
        }
        
    }
}
