using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemClicker : MonoBehaviour
{
	//reference to the inventory
	public NewInventory myInventory;
	//reference to the key pressed
	public KeyCode _Key;
	//reference to the button
	private Button _Button;

	void Awake()
	{
		//Get the button component
		_Button = GetComponent<Button>();
	}

	void Update()
	{
		//Check if the key is pressed down
		if (Input.GetKeyDown(_Key))
		{
			//call the fade to colour method
			FadeToColour(_Button.colors.pressedColor);
			//Click the button
			_Button.onClick.Invoke();
		}
		//check if the key is let go of
		else if (Input.GetKeyUp(_Key))
		{
			//call the fade to color method but assign the normal color instead
			FadeToColour(_Button.colors.normalColor);
		}
	}

	//Fade to the pressed colour of the button
	void FadeToColour(Color btnColor)
	{
		Graphic theGraphic = GetComponent<Graphic>();
		theGraphic.CrossFadeColor(btnColor, _Button.colors.fadeDuration, true, true);
	}

	private InventoryItemBase AttachedItem
	{
		get
		{
			ItemDragHandler dragHandler = gameObject.transform.Find("ItemImage").GetComponent<ItemDragHandler>();

			return dragHandler.Item;
		}
	}


	public void OnItemClicked()
	{
		InventoryItemBase item = AttachedItem;
	
		myInventory.UseItem(item);
	}

}