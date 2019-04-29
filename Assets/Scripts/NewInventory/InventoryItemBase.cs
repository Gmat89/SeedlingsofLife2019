using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using JetBrains.Annotations;
using UnityEngine;
using Debug = FMOD.Debug;


public enum EItemType
{
    Default,
    Consumable,
    Tool
}


//InteractableItemBase Class, serves as the basis for each items information
public class InteractableItemBase : MonoBehaviour
{
    //reference to the name of the item
    public string Name;

    //reference to the sprite of the item for the inventory slot
    public Sprite Image;

    //Reference to objects that can be interacted with( text displays in the hud)
    public string InteractText = "Press Right Mouse Button to pick up the item";

    //reference to the Enum item type
    public EItemType ItemType;



//function for when the player interacts with an object can be overridden
    public virtual void OnInteract()
    {
    }

//bool that checks if the object collided with can be interacted with and displays a message panel can be overridden
    public virtual bool CanInteract(Collider other)
    {
        //Local reference to the HUD
        HUD theHud = FindObjectOfType<HUD>();
        //Open the HUD's message panel via name
        theHud.OpenMessagePanel(Name);
        //set the bool to true
        return true;
    }
}

//Class InventoryItemBase that inherits from the InteractableItemBase class
public class InventoryItemBase : InteractableItemBase
{
    //reference to the inventory Slot
    public InventorySlots Slot { get; set; }


    //Called when the player picks up an item can be overridden
    public virtual void OnPickUp()
    {
        //Destroy the game objects rigidbody when picked up
        Destroy(gameObject.GetComponent<Rigidbody>());
        //set the items visibility to false
        gameObject.SetActive(false);
    }

    //function for when the player drops an item can be overridden
    public virtual void OnDrop(InventoryItemBase item)
    {
        //declare a hit point variable
        RaycastHit hit = new RaycastHit();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //check if the output ray hit in a distance of 1000
        if (Physics.Raycast(ray, out hit, 1000))
        {
            //set the gameobject visibility to true
            gameObject.SetActive(true);
            //set the gameobjects position to the hit point of the mouse pointer
            gameObject.transform.position = hit.point;
            
            //see the gameobjects rotation to that of the defined rotation
            gameObject.transform.eulerAngles = DropRotation;
            //OnGrow(item);
        }
    }


    //function for when the player uses and item can be overridden
    public virtual void OnUse()
    {
        //set the selected items local transform to the pick position variable (players hand position)
        transform.localPosition = PickPosition;
        //set the selected items local rotation to the pick rotation variable (players hand rotation)
        transform.localEulerAngles = PickRotation;
    }


    // function for when the dropped item is growing can be overridden
    public virtual void OnGrow(InventoryItemBase item)
    {
        //check if the selected item has the grow tree script attached
        if (item.GetComponent<GrowTree>())
        {
            //Find the grow tree script attached and turn it into a variable
            GrowTree gt = FindObjectOfType<GrowTree>();
            //set the isplanted bool to true
            gt.isPlanted = true;
            //if the item is planted


            //if it is not planted
            if (!gt.isPlanted)
            {
                //stop growing the plant
                gt.StopGrowingTree();
             
            }
        }
    }


    //vector 3 variable for the  items picked up transform
    public Vector3 PickPosition;

    //vector 3 variable for the items picked up rotation
    public Vector3 PickRotation;

    //vector 3 variable for the items dropped rotation
    public Vector3 DropRotation;

    //bool for if the item is used after it is picked up
    public bool UseItemAfterPickup = false;
}