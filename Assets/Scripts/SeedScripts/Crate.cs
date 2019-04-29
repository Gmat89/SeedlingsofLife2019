using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : InteractableItemBase

{
	private bool crateOpened = false;
	public override void OnInteract()
	{
		InteractText = "Press F to ";

		crateOpened = ! crateOpened;

		InteractText += crateOpened ? "to close" : "to open";
		
		GetComponent<Animator>().SetBool("Open", crateOpened);
	}
}
