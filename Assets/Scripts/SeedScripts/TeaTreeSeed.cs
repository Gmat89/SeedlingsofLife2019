using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTreeSeed : InventoryItemBase
{
	

	//bool for checking if the item is addable to the list
	public bool isAddable;

	public override void OnUse()
	{
		//base.OnUse();
		GrowTree gt = GetComponent<GrowTree>();

		if (transform.localScale.x >= gt.maxScale.x)
		{
			isAddable = true;

			if (isAddable)
			{
				GameObject.Find("GameManager").GetComponent<PartyManager>().AddTeaTree();

				isAddable = false;
				Destroy(this.gameObject);
			}

		}
	}
}

	

