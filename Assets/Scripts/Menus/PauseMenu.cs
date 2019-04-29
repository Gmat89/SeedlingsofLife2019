using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //fmod stuff
    [FMODUnity.EventRef]
    public string selectsound;
    FMOD.Studio.EventInstance soundevent;

    //pause menu holder
    public GameObject PauseMenuHolder;
    public static bool GameIsPaused = false;

    private void Update()
    {
        //if escape is pressed display pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, GetComponent<Transform>(), GetComponent<Rigidbody>());
            soundevent.start();

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Start()
    {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(selectsound);
    }

    //pause game
    public void PauseGame()
    {
        Time.timeScale = 0f;
        PauseMenuHolder.SetActive(true);
        GameIsPaused = true;
        
    }

    //resume game
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenuHolder.SetActive(false);
        GameIsPaused = false;
    }

    //load main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        
    }
}
