using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// this is used to wrap around the events in the inventory class that will be called when the player interacts/collides with an item
///  </summary>
public class InventoryEventArgs : EventArgs
{
	public InventoryEventArgs(InventoryItemBase item)
	{
		//parameter that is used to call this event
		Item = item;
	}

	public InventoryItemBase Item;
}
