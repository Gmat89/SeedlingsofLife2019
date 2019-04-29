using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class InventorySlots
{ 

	private Stack<InventoryItemBase> myItemStack = new Stack<InventoryItemBase>();

	private int myInventoryID = 0;


	public InventorySlots(int ID)
	{
		myInventoryID = ID;
	}

	public int Id
	{
		get { return myInventoryID; }
	}

	public void AddItem(InventoryItemBase item)
	{
		
		item.Slot = this;
		myItemStack.Push(item);
	}

	public InventoryItemBase FirstItem
	{
		get
		{
			if (IsEmpty)
			{
				return null;
			}

			return myItemStack.Peek();

		}
	}

	public bool IsStackable(InventoryItemBase item)
	{
		if (IsEmpty)
		{
			return false;
		}

		InventoryItemBase first = myItemStack.Peek();

		if (first.Name == item.Name)
		{
			return true;
		}

		return false;
	}

	public bool IsEmpty
	{
		get { return Count == 0; }
	}

	public int Count
	{
		get { return myItemStack.Count; }
	}

	public bool Remove(InventoryItemBase item)
	{
		if (IsEmpty)
		{
			return false;
		}

		InventoryItemBase first = myItemStack.Peek();
		if (first.Name == item.Name)
		{
			myItemStack.Pop();
			return true;
		}

		return false;
	}





}
