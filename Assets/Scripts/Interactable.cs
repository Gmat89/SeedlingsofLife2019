using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
/// <summary>
/// This script acts as a Base class for all interactions however each object such as a treasure box or an item or environmental object
/// will have it's own conditions/functionality
/// </summary>

public class Interactable : MonoBehaviour
{
	//how far the player needs to get to interact with an object
	public float radius = 3f;

	//check if the player is focusing on an object
	public bool isFocusing = false;
	//the Players transform in the scene
	private Transform thePlayerObject;

	//interaction transform reference
	public Transform interactionTransform;
	//has interacted bool
	private bool hasInteracted = false;

	//function for actually interacting with the object
	/// <summary>
	/// This function allows us to call the Interact function in other scripts such as items or enemies
	/// and override the the conditions so that each object can have it's own set of conditions/functionality. Pretty Sweet!
	/// </summary>
	public virtual void Interact()
	{
		//This method is meant to be overridden
		Debug.Log("Interacting with " + transform.name);
	}

	void Update()
	{
		//if IsFocusing = true && we havent interacted with an object
		if(isFocusing && !hasInteracted)
		{
			//check the distance between the objects position and the players position
			float distance = Vector3.Distance(thePlayerObject.position, interactionTransform.position);
			//check if the player is within the radius
			if (distance <= radius)
			{
				//print that we interacted with an object in the console
				Debug.Log ("Interact");

				//call the interact function
				Interact();
				//set the has interacted bool to true
				hasInteracted = true;
			}
		}

	}
	//function to check if the player can interact with the object
	public void OnFocused(Transform thePlayerTransform)
	{
		//set isFocusing bool to true
		isFocusing = true;
		//set the player object transform to the value 
		thePlayerObject = thePlayerTransform;
		//set the hasInteracted bool to false (interact once)
		hasInteracted = false;
	}

	public void OnDeFocused()
	{
		//set isFocusing to false
		isFocusing = false;
		//set the player object to false
		thePlayerObject = null;
		//set the hasInteracted bool to false(interact once)
		hasInteracted = false;
	}


	//Show colours on the object that the player is looking at
	void OnDrawGizmosSelected()
	{
		//check if the assigned interaction transform is null 
		if (interactionTransform == null)
		{
			//set the interaction transform to the objects own transform
			interactionTransform = transform;
		}
		//colour the interaction radius lines yellow
		Gizmos.color = Color.yellow;
		//draw the interaction radius around the object
		Gizmos.DrawWireSphere(interactionTransform.position,radius);
	}
}
