using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string selectsound;
    FMOD.Studio.EventInstance soundevent;

    //makes gamemanager a singleton because it is carried over every scene
    public static GameManager instance;

    //reference to party manager
    private PartyManager partyManager;


    public RegionData currentRegion;
    public GameObject playerCharacter;

    //determines the location in which the player should be spawned when they exit battle
    public Vector3 nextPlayerPosition;
    public Vector3 lastPlayerPosition; //before battle

    //scenes to load
    public string sceneToLoad;
    public string lastScene; //before battle

    //bools that affecct gameplay
    public bool isWalking = false;
    public bool canEncounter = false;
    public bool gotAttacked = false;
    public bool canBattle = true;

    //used to count
    public int counter;

    //the states for the world
    public enum GameStates
    {
        WORLD_STATE,
        BATTLE_STATE,
        IDLE
    }

    //used for the battle state machine
    public int enemyAmount;
    public List<GameObject> enemiesToBattle = new List<GameObject>();

    //used in party manager
    public int playerAmount;
    public List<GameObject> playersToBattle = new List<GameObject>();

	public GameStates gameState;

    private void Start()
    {
        soundevent = FMODUnity.RuntimeManager.CreateInstance(selectsound);
        
    }

    void Awake()
    {
        //singleton stuff
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        //spawns player in the last location they exited the level
        if (!GameObject.Find("Player"))
        {
            
            GameObject player = Instantiate(playerCharacter, nextPlayerPosition, Quaternion.identity) as GameObject;
            player.name = "Player";
        }
        //makes sure the player can battle things if they need to
        canBattle = true;
        partyManager = GetComponent<PartyManager>();
        
    }

    private void Update()
    {
        //the states of the overworld
        switch(gameState)
        {
            //used when walking around in the overworld
            case (GameStates.WORLD_STATE):
                {
                    
                    if (isWalking && canBattle)
                    {
                        RandomEncounter();
                    }
                    if (gotAttacked)
                    {
                        gameState = GameStates.BATTLE_STATE;
                    }
                    break;
                    
                }
                //used to switch over to the battle scene
            case (GameStates.BATTLE_STATE):
                {
                    //Load Battle Scene
                    StartBattle();
                    gameState = GameStates.IDLE;
                    break;
                }
                //idle
            case (GameStates.IDLE):
                {
                    break;
                }
            
        }
        if (GameManager.instance.isWalking == false)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevent, GetComponent<Transform>(), GetComponent<Rigidbody>());
            soundevent.start();
        }

    }

    //load next scene
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    //just loads overworld after battle
    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    //just a dice throwing function that determines when u can battle things
    void RandomEncounter()
    {
        if(isWalking && canEncounter)
        {
            //the 10 refers to the likely hood of an encounter
            if(Random.Range(0,1000) < 50)
            {
                Debug.Log("U GOT ATTACKED");
                gotAttacked = true;
            }
        }
    }

    void StartBattle()
	{
		
		//enemy reference here
		//amount of enemies
		enemyAmount = Random.Range(1, currentRegion.maxAmountEnemies + 1);

        //which enemies
        //determines how many enemies need to going to the battle scene
        for (int i = 0; i < enemyAmount; i++)
        {
            if (counter <= partyManager.maxPartyMembers)
            {
                enemiesToBattle.Add(currentRegion.possibleEnemies[Random.Range(0, currentRegion.possibleEnemies.Count)]);
                counter++;
            }
            else if (counter > partyManager.maxPartyMembers)
            {
                break;
            }
            counter = 0;
        }

        //amount of players
        playerAmount = partyManager.maxPartyMembers;
        //which players
        //determines how many players goes into the battle scene
        for (int i = 0; i < playerAmount; i++)
        {
            if(counter <= partyManager.maxPartyMembers)
            {
                playersToBattle.Add(partyManager.seedlingsInParty[GameManager.instance.playersToBattle.Count]);
                counter++;
            }
            else if(counter > partyManager.maxPartyMembers)
            {
                break;
            }
            counter = 0;
        }

        //saves player location for them to be spawned in at later
        lastPlayerPosition = GameObject.Find("Player").gameObject.transform.position;
        nextPlayerPosition = lastPlayerPosition;

        //saves scene so the game knows what scene to load back to after a battle
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentRegion.BattleScene);

        //just ensures that the encounters are reset and the player can go into battle again
        isWalking = false;
        gotAttacked = false;
        canEncounter = false;
    }

	[System.Serializable]
	public class SeedlingMoves
	{
		private string name;
		public MoveType category;
		public Stat moveStat;
		public BaseStats.SeedlingType moveType;
	}

	[System.Serializable]
	public class Stat
	{
		public float minimum;
		public float maximum;
	}

	public enum MoveType
	{
		Physical,
		Skill,
	}

    public IEnumerator BattleCoolDown()
    {
        canBattle = false;
        yield return new WaitForSeconds(3f);
        canBattle = true;
    }
}
