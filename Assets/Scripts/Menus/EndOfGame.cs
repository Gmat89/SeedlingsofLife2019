using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{
    // end game menu holder
    public GameObject endMenu;

    //triggers the end game menu
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            endMenu.SetActive(true);
        }
    }
}
