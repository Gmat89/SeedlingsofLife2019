using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventory : MonoBehaviour
{
	//fixed number of inventory slots
	private const int SLOTS = 9;

	//list reference  of base inventory items
	//private List<IInventoryItem> myItems = new List<IInventoryItem>();

	//reference to a list of the inventory slots(changed from items)^^^
	private IList<InventorySlots> mySlots = new List<InventorySlots>();

	//method to add an item to the myItems list
	public event EventHandler<InventoryEventArgs> ItemAdded;

	//method to remove an item from the myItems list
	public event EventHandler<InventoryEventArgs> ItemRemoved;
	
	//method for when an item is used from the myItems list
	public event EventHandler<InventoryEventArgs> ItemUsed;

	public NewInventory()
	{
		for (int i = 0; i < SLOTS; i++)
		{
			mySlots.Add(new InventorySlots(i));
		}
	}

	private InventorySlots FindStackableSlot(InventoryItemBase item)
	{
		foreach (InventorySlots slot in mySlots)
		{
			if (slot.IsStackable(item))
			{
				return slot;
			}

		}

		return null;
	}

	private InventorySlots FindNextEmptySlot()
	{
		foreach (InventorySlots slot in mySlots)
		{
			if (slot.IsEmpty)
				return slot;
		}

		return null;
	}


	public void AddItem(InventoryItemBase item)

	{
		InventorySlots freeSlot = FindStackableSlot(item);
		if (freeSlot == null)
		{
			freeSlot = FindNextEmptySlot();
		}

		if (freeSlot != null)
		{
			freeSlot.AddItem(item);
			{
				if (ItemAdded != null)
				{
					ItemAdded(this, new InventoryEventArgs(item));
				}
			}
		}
	}


	//function for removing an item from the inventory, reference to IInventoryItem as item
	public void RemoveItem(InventoryItemBase item)
	{

		foreach (InventorySlots slot in mySlots)
		{
			if (slot.Remove(item))
			{
				if (ItemRemoved != null)
				{
					ItemRemoved(this, new InventoryEventArgs(item));
				}

				break;
			}

		}

		
	}

	internal void UseItem(InventoryItemBase item)
	{
		if (ItemUsed != null)
		{
			ItemUsed(this, new InventoryEventArgs(item));
		}
		//call the onuse function the selected item
		item.OnUse();
		//check if the items tag == Seed
		if (item.CompareTag("Gumnut") || item.CompareTag("Teatree") || item.CompareTag("Vine") || item.CompareTag("Fungi") || item.CompareTag("Lily") || item.CompareTag("Orchid"))
		{
			//call remove item once used
			RemoveItem(item);
		}
		
		
	}
}


	
