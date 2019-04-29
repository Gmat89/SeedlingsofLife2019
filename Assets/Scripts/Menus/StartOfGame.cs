using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOfGame : MonoBehaviour
{
    //menu holders
    public GameObject startMenu;
    
    private void Start()
    {
        //just ensures that the start menu is on at start by default
        startMenu.SetActive(true);
    }

    //removes the start menu to allow for gameplay
    public void RemoveStartMenu()
    {
        startMenu.SetActive(false);
    }

    //load main menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
