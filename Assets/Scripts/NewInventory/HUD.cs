using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public NewInventory theInventory;

	public GameObject MessagePanel;

	void Start()
	{
		//bind the itemAdded event to when an item is added to the inventory
		theInventory.ItemAdded += InventoryScript_ItemAdded;
		//bind the itemRemoved event to when an item is removed from the inventory
		theInventory.ItemRemoved += InventoryScript_ItemRemoved;
	}

	//call this to bind itemAdded event
	private void InventoryScript_ItemAdded(object sender, InventoryEventArgs invEventArgs)
	{
		//find the Inventory panel object's transform
		Transform inventoryPanel = transform.Find("InventoryPanel");
		int index = -1;
		foreach (Transform slot in inventoryPanel)
		{
			index++;

			//ImageBorder stuff
			Transform imageTransform = slot.GetChild(0).GetChild(0);
			Transform textTransform = slot.GetChild(0).GetChild(1);
			Image image = imageTransform.GetComponent<Image>();
			Text textCount = textTransform.GetComponent<Text>();
			ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

			if (index == invEventArgs.Item.Slot.Id)
			{
				image.enabled = true;
				image.sprite = invEventArgs.Item.Image;

				int itemCount = invEventArgs.Item.Slot.Count;
				if (itemCount > 1)
				{
					textCount.text = itemCount.ToString();
				}
				else
				{
					textCount.text = "";
				}

				//store a reference to the item
				itemDragHandler.Item = invEventArgs.Item;
				break;
			}
		}
	}



	private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs invEventArgs)
	{
		Transform inventoryPanel = transform.Find("InventoryPanel");

		int index = -1;
		foreach (Transform slot in inventoryPanel)
		{
			index++;

			Transform imageTransform = slot.GetChild(0).GetChild(0);
			Transform textTransform = slot.GetChild(0).GetChild(1);
			Image image = imageTransform.GetComponent<Image>();
			Text textCount = textTransform.GetComponent<Text>();
			ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

			//Found the item in the UI
			if (itemDragHandler.Item == null)
				continue;

			//find the slot to remove from
			if(invEventArgs.Item.Slot.Id == index)
			{
				int itemCount = invEventArgs.Item.Slot.Count;
				itemDragHandler.Item = invEventArgs.Item.Slot.FirstItem;

				if (itemCount < 2)
				{
					textCount.text = "";
				}
				else
				{
					textCount.text = itemCount.ToString();
				}

				if (itemCount == 0)
				{
					//set the image enabled to false
					image.enabled = false;
					//set the item sprite to null
					image.sprite = null;
				}
				//set the item drag handler to null
				break;
			}
		}
	}

	private bool mIsMessagePanelOpened = false;

	public bool IsMessagePanelOpened
	{
		get { return mIsMessagePanelOpened; }
	}

	public void OpenMessagePanel(InteractableItemBase item)
	{
		Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
		mpText.text = item.InteractText;


		mIsMessagePanelOpened = true;


	}

	public void OpenMessagePanel(string text)
	{
		MessagePanel.SetActive(true);
		Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
		mpText.text = text;

		mIsMessagePanelOpened = true;
	}

	public void CloseMessagePanel()
	{
		
		MessagePanel.SetActive(false);
		mIsMessagePanelOpened = false;
	}
}



	