using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //menu holders
    public GameObject playMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject quitHolder;
    
    //options stuff
    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullScreenToggle;
    public int[] screenWidths;

    //scene management
    int activeScreenResIndex;

    

    private void Start()
    {
        playMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        quitHolder.SetActive(false);

        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1 ? true : false);

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = 1 == activeScreenResIndex;

        }

        fullScreenToggle.isOn = isFullscreen;
    }
    
    //displays play menu
    public void DisplayPlayMenu()
    {
        playMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        quitHolder.SetActive(false);
       
    }

    //display quit menu
    public void DisplayQuitMenu()
    {
        playMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        quitHolder.SetActive(true);
       
    }

    //display options menu
    public void DisPlayOptionsMenu()
    {
        playMenuHolder.SetActive(false);
        quitHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
       
    }

    //quit game
    public void Quit()
    {
        Application.Quit();
    }
    
    //set resoultions in options menu
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    //toggle fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }

        PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    //load game
    public void LoadOverWorld()
    {
        SceneManager.LoadScene("OverWorld");
    }

    
}

