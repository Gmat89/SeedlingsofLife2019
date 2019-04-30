using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
//ensure that the playermotor script is found so this script can be used
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    //Movement Sounds
    //  [FMODUnity.EventRef]
    // public string PickedUp;
    // FMOD.Studio.EventInstance pickup;

    //Reference to the camera
    public Camera cam;

    //Player move speed value
    public float moveSpeed;

    //Input value
    private Vector3 input;

    //Player velocity
    public Vector3 playerVelocity;

    public Vector3 currentPosition;
    public Vector3 lastPosition;

    //Reference to the PlayerMotor Script
    public PlayerMotor thePlayerMotor;

    //Player LayerMask reference
    public LayerMask movementMask;

    //Players rigidbody
    private Rigidbody rb;

    //Keep track of what is being focused on
    //public Interactable focus;

    public NewInventory theInventory;

    public GameObject playersHand;

    //private InventoryItemBase myCurrentItem = null;

    public HUD theHud;

    //public bool mLockPickUp;

    private InventoryItemBase myCurrentItem = null;



    void Start()
    {
        //  pickup = FMODUnity.RuntimeManager.CreateInstance(PickedUp);
        //get the attached rigidbody component
        rb = GetComponent<Rigidbody>();
        //Find the camera in the scene & attach it to the player
        cam = FindObjectOfType<Camera>();
        //Get the playermotor script attached to the player
        thePlayerMotor = GetComponent<PlayerMotor>();
        //sets the position for gamemanger
        transform.position = GameManager.instance.nextPlayerPosition;

        theInventory.ItemUsed += Inventory_ItemUsed;
        theInventory.ItemRemoved += Inventory_ItemRemoved;
    }

    void Update()
    {
        //Check if the mouse if hovering over a UI element and if it is stop the player from moving
        if (EventSystem.current.IsPointerOverGameObject())

        {
            return;
        }

        //input variable  = movement made on the horizontal or vertical axis
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        //set the players velocity to the detected input multiplied by the move speed
        //playerVelocity = input * moveSpeed;

        //LEFT MOUSE BUTTON USED FOR MOVEMENT

        //check if the left mouse button has been pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Cast a ray from the camera towards whatever we have clicked on
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //store the info detected in a raycast variable
            RaycastHit hit;
            //if the ray hits something then run the next lines of code
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //move the player to a point
                thePlayerMotor.MoveToPoint(hit.point);
                Debug.Log(("We hit" + hit.collider.name + " " + hit.point));
                //StopFocusing();
                //stop focusing on any objects
            }
        }

        //RIGHT MOUSE BUTTON USED FOR INTERACTING
        //if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            //Cast a ray from the camera towards whatever we have clicked on
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //store the info detected in a raycast variable
            RaycastHit hit;
            //if the ray hits something then run the next lines of code
            if (Physics.Raycast(ray, out hit, 100))
            {
                // Debug.Log("Code Got Here");
                //theInventory.AddItem(myCurrentItem);
                //myCurrentItem.OnPickUp();
                //theHud.CloseMessagePanel();
                InventoryItemBase inventoryItem = myInteractableItem as InventoryItemBase;
                theInventory.AddItem(inventoryItem);
                if (inventoryItem != null)
                {
                    inventoryItem.OnPickUp();
                }
                theHud.CloseMessagePanel();
                Debug.Log(("We hit" + hit.collider.name + " " + hit.point));
                if (myInteractableItem != null)
                {
                    myInteractableItem.OnInteract();
                    //  FMODUnity.RuntimeManager.AttachInstanceToGameObject(pickup, GetComponent<Transform>(), GetComponent<Rigidbody>());
                    //   pickup.start();
                }

                //focus on an object
                //Collect an item
                //stop focusing on any objects
            }
        }
    }

    //early prototype of player needing tools to plant and harvest
    public bool HasTool
    {
        get
        {
            if (myCurrentItem == null)
            {
                return false;
            }

            return myCurrentItem.ItemType == EItemType.Tool;
        }
    }

    //allows player to equip tools
    //early prototype
    private void SetItemActive(InventoryItemBase item, bool active)
    {
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? playersHand.transform : null;
    }


    //function to handle when an Item is used
    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.Item.ItemType != EItemType.Consumable)
        {
            if (myCurrentItem != null)
            {
                SetItemActive(myCurrentItem, false);
            }
        }
        InventoryItemBase item = e.Item;

        //put the current item in the players hand
        SetItemActive(item, true);

        myCurrentItem = e.Item;
    }


    public void ThrowCurrentItem()
    {
        GameObject goItem = (myCurrentItem as MonoBehaviour).gameObject;

        theInventory.RemoveItem(myCurrentItem);

        Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
        if (rbItem != null)
        {
            rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

            Invoke("DoDropItem", 0.25f);

        }
    }

    public void DoThrowItem()
    {
        Destroy((myCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

        myCurrentItem = null;
    }



    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InteractableItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);

        goItem.transform.parent = null;
    }





    void FixedUpdate()
    {
        //set the rigidbodys velocity to the player velocity
        //rb.velocity = playerVelocity;

        //controls encounter zone stuff
        currentPosition = transform.position;
        if (currentPosition == lastPosition)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }

        StartCoroutine(UpdatePos());

    }

    IEnumerator UpdatePos()
    {
        yield return new WaitForSeconds(0.5f);
        lastPosition = currentPosition;
    }


    public void InteractWithItem()
    {
        if (myInteractableItem != null)
        {
            myInteractableItem.OnInteract();

            if (myInteractableItem is InventoryItemBase)
            {
                InventoryItemBase inventoryItem = myInteractableItem as InventoryItemBase;
                theInventory.AddItem(inventoryItem);
                inventoryItem.OnPickUp();



                if (inventoryItem.UseItemAfterPickup)
                {
                    theInventory.UseItem(inventoryItem);
                }
            }
            theHud.CloseMessagePanel();

            myInteractableItem = null;
        }
    }

    private InteractableItemBase myInteractableItem = null;
    private void OnTriggerEnter(Collider other)
    {
        /* //also make tag for scene name
		if(other.tag == "SceneNameHere")
		{
			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
			GameManager.instance.nextPlayerPosition = col.spawnPoint.transform.position;
			GameManager.instance.sceneToLoad = col.sceneToLoad;
			GameManager.instance.LoadNextScene();
		}
		*/

        InteractableItemBase item = other.GetComponent<InteractableItemBase>();

        if (item != null)
        {
            if (item.CanInteract(other))
            {
                myInteractableItem = item;
                theHud.OpenMessagePanel(myInteractableItem);
                //SetFocus(item);
            }

        }

        if (other.tag == "EncounterZone")
        {
            RegionData region = other.gameObject.GetComponent<RegionData>();

            GameManager.instance.currentRegion = region;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EncounterZone")
        {
            GameManager.instance.canEncounter = true;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            theHud.CloseMessagePanel();
            myInteractableItem = null;
        }
        if (other.tag == "EncounterZone")
        {
            GameManager.instance.canEncounter = false;
        }

    }
}



