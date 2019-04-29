using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilySeed : InventoryItemBase
{
    //bool to check if the item is addable to the party manager
    public bool isAddable;

    public override void OnUse()
    {

        //TODO: Do something with the object
        //base.OnUse();
        GrowTree gt = GetComponent<GrowTree>();

        if (transform.localScale.x >= gt.maxScale.x)
        {
            isAddable = true;

            if (isAddable)
            {
                GameObject.Find("GameManager").GetComponent<PartyManager>().AddLily();

                isAddable = false;
                Destroy(this.gameObject);
            }
        }
    }
}
