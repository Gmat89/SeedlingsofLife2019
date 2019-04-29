using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLootGenerator : MonoBehaviour
{
	//use textbox for now change to array of game objects for seedling packs
	//public GameObject Textbox;

	public GameObject[] seedlingPacks;
	public int randomNumber;
	public Transform spawnPosition;

	//public int TheNumber;
    // Start is called before the first frame update
	public void RandomeGenerate()
	{
		randomNumber = Random.Range(0, seedlingPacks.Length);
		Instantiate(seedlingPacks[randomNumber], spawnPosition.position, spawnPosition.rotation);
		//TheNumber = Random.Range(1, 100000);
		//Textbox.GetComponent<Text>().text = "" + TheNumber;
	}   
}
