using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemHandler : MonoBehaviour, IDropHandler
{
	public NewInventory theInventory;

	public void OnDrop(PointerEventData eventData)
	{
		RectTransform invPanel = transform as RectTransform;

		if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
		{
			InventoryItemBase item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
			if (item != null)
			{
				theInventory.RemoveItem(item);
				item.OnDrop(item);
				item.GetComponent<GrowTree>().isPlanted = true;
				item.GetComponent<GrowTree>().GrowThePlant();
				Debug.Log("we dropped" + item.Name);

			}
		}
	}
}

		
	
   