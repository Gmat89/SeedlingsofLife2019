using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestingPlant : MonoBehaviour
{
	//harvest time variable
	public float harvestTime = 3.0f;

	//harvest text
	public Text harvestText;

	//boolean canHarvest the plant
	public bool canHarvest;





	// Start is called before the first frame update
	void Start()
	{
		//set canHarvest to false
		canHarvest = false;
	}

	// Update is called once per frame
	void Update()
	{
		//if canHarvest is true
		if (canHarvest)
		{
			//if the player presses the left mouse button
			if (Input.GetMouseButtonDown(0))
			{
				//set the harvest text to blank
				harvestText.text = "";
				//run the harvesting Routine function
				StartCoroutine(harvestingRoutine());
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		//check if the collided objects tag is the player
		if (other.gameObject.tag == "Player")
		{
			//set canHarvest to true
			canHarvest = true;
			//set the harvest text to Ready to harvest
			harvestText.text = "ReadyToHarvest";
		}

	}

	void OnTriggerExit(Collider other)
	{
		//check if the player is exiting the collider
		if (other.gameObject.tag == "player")
		{
			//set can harvest to false
			canHarvest = false;
			//set the harvest text blank
			harvestText.text = "";
		}
	}

	public IEnumerator harvestingRoutine()
	{
		//wait for the set amount of harvest time
		yield return new WaitForSeconds(harvestTime);
		//destroy the tree
		Destroy(this.gameObject);
	}
}

