using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Debug = UnityEngine.Debug;

public class GrowTree : MonoBehaviour
{

	// Update is called once per frame
	//maximum tree growth value
	public float maxGrowth = 1f;

	public Vector3 maxScale;

	//tree growth speed
	public float growthSpeed = 0.1f;

	public float currentGrowth;

	//temp scale variable
	public Vector3 tempScale;

	public bool isPlanted;

	

	void Start()
	{
		maxScale = new Vector3(1, 1, 1);

	}

	void Update()
	{

		if (isPlanted)
		{
			GrowThePlant();

		}

		if (transform.localScale.x >= maxGrowth)
		{
			isPlanted = false;
			StopGrowingTree();
			HUD theHud = FindObjectOfType<HUD>().GetComponent<HUD>();
                
			theHud.OpenMessagePanel("Right Click to Harvest");
		}
	}








	public void GrowThePlant()
	{
		currentGrowth++;
		Debug.Log("Called on grow");

		//if the local scale of the the tree is less than the maxGrowth value
		tempScale = transform.localScale;
		tempScale.x += Time.deltaTime * growthSpeed;
		tempScale.y += Time.deltaTime * growthSpeed;
		tempScale.z += Time.deltaTime * growthSpeed;
		//increase the local scale by the tempScale variable.
		transform.localScale = tempScale;
		
	}

	public void StopGrowingTree()
	{
		currentGrowth = 0;
		isPlanted = false;
		tempScale = transform.localScale;
	}
}


