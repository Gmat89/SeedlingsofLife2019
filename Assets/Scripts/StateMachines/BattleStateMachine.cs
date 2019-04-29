    using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{

	public enum PerformAction
	{
		//Waiting state(preparing for action)
		WAIT,

		//Make Action
		TAKEACTION,

		//Perform action
		PERFORMACTION,

        //Checking alive
        CHECKALIVE,

        //Win state
        WIN,

        //Lose state
        LOSE
	}



	public PerformAction battleStates;

	public List<HandleTurns> PerformList = new List<HandleTurns>();
	public List<GameObject> HerosInBattle = new List<GameObject>();
	public List<GameObject> EnemiesInBattle = new List<GameObject>();

	//handles all the inputs of all the states
	public enum HeroGUI
	{
		ACTIVATE,
		WAITING,

		//Input 1 = Basic attack
		INPUT1,

		//Input 2 = Selected enemy
		INPUT2,
		DONE
	}

    HeroStateMachine heroStateMachine;

	public HeroGUI HeroInput;

	public List<GameObject> HerosToManage = new List<GameObject>();

	private HandleTurns HeroChoice;

	//The reference to the enemy button
	public GameObject enemyButton;

	//Spacer object transform
	public Transform Spacer;

	//Attack panel reference
	public GameObject AttackPanel;

	//Enemy select panel reference
	public GameObject EnemySelectPanel;

	//Skill Panel reference
	public GameObject SkillPanel;

    //Win Screen
    public GameObject WinHolder;

    //Lose Screen
    public GameObject LoseHolder;

	//skills attacks
	public Transform actionSpacer;

    //attacks of heros
	public Transform skillSpacer;
	public GameObject actionButton;
	public GameObject skillButton;
    public GameObject healButton;
    public GameObject runButton;

	private List<GameObject> atkBtns = new List<GameObject>();

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();
	public List<Transform> spawnPointsPlayer = new List<Transform>();

	

	//static public BattleStateMachine BSM;

    private void Awake()
	{
		//BSM = this;
        for(int i = 0; i < GameManager.instance.enemyAmount; i++)
        {
            GameObject NewEnemy = Instantiate(GameManager.instance.enemiesToBattle[i],spawnPoints[i].position,GameManager.instance.enemiesToBattle[i].transform.rotation) as GameObject;
            NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemyStats.theName + "_" + (i+1);
            NewEnemy.GetComponent<EnemyStateMachine>().enemyStats.theName = NewEnemy.name;
            EnemiesInBattle.Add(NewEnemy);
        }

        for (int i = 0; i < GameManager.instance.playerAmount; i++)
        {
            GameObject NewPlayer = Instantiate(GameManager.instance.playersToBattle[i], spawnPointsPlayer[i].position, GameManager.instance.playersToBattle[i].transform.rotation) as GameObject;
            NewPlayer.name = NewPlayer.GetComponent<HeroStateMachine>().heroStats.theName + "_" + (i + 1);
            NewPlayer.GetComponent<BaseStats>().theName = NewPlayer.name;
            HerosInBattle.Add(NewPlayer);
        }
    }

    // Use this for initialization
    void Start()
	{
        
		battleStates = PerformAction.WAIT;
		//Find current enemies in the game with the range of a list with the tag of enemy
		//unneeded line ---> //EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		//Find current enemies in the game with the range of a list with the tag of enemy
		//HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
		//Set the heros input state to activate
		HeroInput = HeroGUI.ACTIVATE;
        //set the attack panel to false
        AttackPanel.SetActive(false);
		//set the enemy select panel to false
		EnemySelectPanel.SetActive(false);
		//set the skills panel to false
		SkillPanel.SetActive(false);
		//Call the Enemy button function
		EnemyButtons();

        heroStateMachine = FindObjectOfType<HeroStateMachine>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (battleStates)
		{
			//check if there is anyone in the list of performers
			case (PerformAction.WAIT):
				if (PerformList.Count > 0)
				{
					battleStates = PerformAction.TAKEACTION;
				}

				break;


			case (PerformAction.TAKEACTION):
				
				//Find the first game object in the list of attackers by name.
				GameObject performer = GameObject.Find(PerformList[0].Attacker);
				PerformList[0].Attacker = FindObjectOfType<BaseStats>().theName;
				//Check if the attacker is of the type Enemy
				if (PerformList[0].Type == "Enemy")
				{
					//Get the Enemy state machine component and make it a variable
					EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
					//check if the currently attacked hero is in the battle list
					for (int i = 0; i < HerosInBattle.Count; i++)
					{
						if (PerformList[0].AttackersTarget == HerosInBattle[i])
						{
							//find the first target in the list of heros and set that as the target
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							//Set the enemy state machine state to take a action
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						}
						else
						{
							//choose another hero from the target list from the heros in battle
							PerformList[0].AttackersTarget = HerosInBattle[Random.Range(0, HerosInBattle.Count)];
							//find the first target in the list of heros and set that as the target
							ESM.HeroToAttack = PerformList[0].AttackersTarget;
							//Set the enemy state machine state to take a action
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
					}
				}
				//Check if the attacker is of the type Hero
				if (PerformList[0].Type == "Hero")
				{
					//Get the hero state machine component
					HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
					//Add the enemy to attack to the list of performers based on the targeted enemy
					HSM.EnemyToAttack = PerformList[0].AttackersTarget;

					//Set the current state in the hero state machine to action
					HSM.currentState = HeroStateMachine.TurnState.ACTION;
					//Debug.Log("Hero is here to perform");

				}
				battleStates = PerformAction.PERFORMACTION;
				break;

			case (PerformAction.PERFORMACTION):
                //Idle
				break;

            case (PerformAction.CHECKALIVE):
                if(HerosInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    //Lose battle
                }
                else if(EnemiesInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    //Win battle
                }
                else
                {
                    ClearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                }
                break;

            case (PerformAction.LOSE):
                {
                    LoseHolder.SetActive(true);
                    Debug.Log("You Lost The Battle");
                }
                break;
            case (PerformAction.WIN):
                {
                    WinHolder.SetActive(true);
                    Debug.Log("You Won The Battle");
                    for(int i = 0; i< HerosInBattle.Count;i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;

                    }
                    StartCoroutine(GameManager.instance.BattleCoolDown());

                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemiesToBattle.Clear();
                    GameManager.instance.playersToBattle.Clear();
                }
                break;
        }

		switch (HeroInput)
		{
			case (HeroGUI.ACTIVATE):
                //check if the heros to manage count is greater than zero then
                if (HerosToManage.Count > 0 && heroStateMachine.currentCoolDown >= 5f)
				{
					//find the first hero in the list and ensure this is the first active hero
					HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
					HeroChoice = new HandleTurns();
					//Set the attack panel visibility to true
					AttackPanel.SetActive(true);
					//Create and populate the attack buttons
					CreateAttackButtons();
					//set the hero input to the Hero game ui state waiting
					HeroInput = HeroGUI.WAITING;
				}
				break;


			case (HeroGUI.WAITING):
				//idle
				break;

			case (HeroGUI.DONE):
				HeroInputDone();
				break;

		}
	}





	public void CollectActions(HandleTurns input)
	{
		PerformList.Add(input);
	}

	public void EnemyButtons()
	{
        //clean up 
        foreach(GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();
        
		//for each enemy in the list of enemies in battle.
		foreach (GameObject enemy in EnemiesInBattle)
		{
			//Spawn the correct corresponding button to target the 
			GameObject newButton = Instantiate(enemyButton) as GameObject;
			//Get the enemy button select script and create a new instance of this
			EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();
			//create a buttonText variable find the text component in the game object
			Text buttonText = newButton.transform.Find("E1Text").gameObject.GetComponent<Text>();
			//find the selected enemys name and store it in the buttonText variable
			buttonText.text = cur_enemy.enemyStats.theName;
			//Pass the content needed for when in battle
			button.enemyPrefab = enemy;


			//Set the button that is instantiated by grabbing the spacers transform
			newButton.transform.SetParent(Spacer, false);
            enemyBtns.Add(newButton);
		}
	}

	public void Input1() //player attack button
	{
		//get the hero attackers name /// THE HERO WHO IS ATTACKING
		HeroChoice.Attacker = HerosToManage[0].name;
		//WHICH HERO IS ATTACKING
		HeroChoice.AttackersGameObject = HerosToManage[0];
		//IS THE OBJECT DOING THE ATTACK A HERO?
		HeroChoice.Type = "Hero";
		//Register the first selected hero's chosen attack from the first attack ability in the list
		HeroChoice.chosenAttack = HerosToManage[0].GetComponent<HeroStateMachine>().heroStats.attacks[0];
		//set the attack panel visibility to false
		AttackPanel.SetActive(false);
		//set the enemy select panel to true, to choose which enemy to attack
		EnemySelectPanel.SetActive(true);
	}

	public void Input2(GameObject chosenEnemy) //enemy selection
	{
		//get the enemy that has been selected
		HeroChoice.AttackersTarget = chosenEnemy;
		//set the current hero input state to done(player has finished their turn)
		HeroInput = HeroGUI.DONE;
	}

	void HeroInputDone()
	{
		//Add the players action to the perform list
		PerformList.Add(HeroChoice);

		//set the enemy select panel visibility to false
		EnemySelectPanel.SetActive(false);



        ClearAttackPanel();
		
		HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
		//remove the first hero from the list
		HerosToManage.RemoveAt(0);
		//set the heroGUI state to  activate to check if there are any more heros in the list to take a turn
		HeroInput = HeroGUI.ACTIVATE;
	}


	//create action buttons
    void ClearAttackPanel()
    {
        //set the enemyselect panel visibility to false
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        SkillPanel.SetActive(false);

        //for each attack button(atkBTN) in the list of attack buttons (atkBtns)
        foreach (GameObject atkBTN in atkBtns)
        {
            //destroy the attack button in the command window
            Destroy(atkBTN);
        }
        atkBtns.Clear();
    }

	//create actionbuttons
	void CreateAttackButtons()
	{

		//PLAYER ATTACK BUTTON

		//Spawn the action button
		GameObject AttackButton = Instantiate(actionButton) as GameObject;
		//Find the attack button transform, find the Text attached to the button and get the Text component
		Text AttackButtonText = AttackButton.transform.Find("AttackBTNtext").gameObject.GetComponent<Text>();
		//Set the attack button text to Attack
		AttackButtonText.text = "Attack";
		//Get the button component, add a listener to the button. On click run the Input1 function from in this script
		AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
		//Set the attack buttons transform according to the parent spacer
		AttackButton.transform.SetParent(actionSpacer, false);
		//add an attack button to the list of buttons.
		atkBtns.Add(AttackButton);


		//PLAYER SKILL BUTTON

		//Spawn the action button
		GameObject SkillAttackButton = Instantiate(actionButton) as GameObject;
		//Find the attack button transform, find the Text attached to the button and get the Text component
		Text SkillAttackButtonText = SkillAttackButton.transform.Find("AttackBTNtext").gameObject.GetComponent<Text>();
		//Set the attack button text to Skill
		SkillAttackButtonText.text = "Skill";
		//Get the button component, add a listener to the button. On click run the Input1 function from in this script
		SkillAttackButton.GetComponent<Button>().onClick.AddListener(() => Input4());
		//Set the attack buttons transform according to the parent spacer
		SkillAttackButton.transform.SetParent(actionSpacer, false);
		//add an attack button to the list of buttons.
		atkBtns.Add(SkillAttackButton);

        /*
        //Spawn the action button
        GameObject HealButton = Instantiate(healButton) as GameObject;
        //Find the attack button transform, find the Text attached to the button and get the Text component
        Text HealButtonText = HealButton.transform.Find("AttackBTNtext").gameObject.GetComponent<Text>();
        //Set the attack button text to Skill
        HealButtonText.text = "Heal";
        //Get the button component, add a listener to the button. On click run the Input1 function from in this script
        HealButton.GetComponent<Button>().onClick.AddListener(() => Input5());
        //Set the attack buttons transform according to the parent spacer
        HealButton.transform.SetParent(actionSpacer, false);
        //add an attack button to the list of buttons.
        atkBtns.Add(HealButton);
        */
        //Spawn the action button
        GameObject RunButton = Instantiate(actionButton) as GameObject;
        //Find the attack button transform, find the Text attached to the button and get the Text component
        Text RunButtonText = RunButton.transform.Find("AttackBTNtext").gameObject.GetComponent<Text>();
        //Set the attack button text to Skill
        RunButtonText.text = "Run";
        //Get the button component, add a listener to the button. On click run the Input1 function from in this script
        RunButton.GetComponent<Button>().onClick.AddListener(() => RunAway());
        //Set the attack buttons transform according to the parent spacer
        RunButton.transform.SetParent(actionSpacer, false);
        //add an attack button to the list of buttons.
        atkBtns.Add(RunButton);

        //check if the player at the top of the list of heros to manage is the first in the list and the skill used is 0 then
        if (HerosToManage[0].GetComponent<HeroStateMachine>().heroStats.skillAttacks.Count > 0)
		{
			// for each skill attack in the Base Attack script that is attached to a hero
			foreach (BaseAttack skillAtk in HerosToManage[0].GetComponent<HeroStateMachine>().heroStats.skillAttacks)
			{

				//spawn a skill button in the command area
				GameObject SkillButton = Instantiate(skillButton) as GameObject;
				//find the text component of the skill attack button
				Text SkillButtonText = SkillButton.transform.Find("AttackBTNtext").gameObject.GetComponent<Text>();
				//set the text component's name to the specified skill name
				SkillButtonText.text = skillAtk.attackName;
				
				//Get the corresponding skill attack to be cast from the Skill attack button script
				SkillAttackButton SAB = SkillButton.GetComponent<SkillAttackButton>();
				//Place the skill attack into the spawned button in the GUI.
				SAB.skillAttackToPerform = skillAtk;
				//Set the current transform of the skill button to the parents transform of the skillspacer as false
				SkillButton.transform.SetParent(skillSpacer, false);
				//Add a skill attack button to the list of exisiting buttons
				atkBtns.Add(SkillButton);
			}
		}
		//if we have no skill attacks in the heros to manage
		else
		{
			//Set the skill attack buttons interactable property to false;
			SkillAttackButton.GetComponent<Button>().interactable = false;
		}
	}
	public void Input3(BaseAttack chosenSkill)//Choose a skill attack to use
	{
		//get the hero attackers name /// THE HERO WHO IS ATTACKING
		HeroChoice.Attacker = HerosToManage[0].name;
		//WHICH HERO IS ATTACKING
		HeroChoice.AttackersGameObject = HerosToManage[0];
		//IS THE OBJECT DOING THE ATTACK A HERO?
		HeroChoice.Type = "Hero";
		//set the attack panel visibility to false
		AttackPanel.SetActive(false);
		//set the enemy select panel to true, to choose which enemy to attack
		EnemySelectPanel.SetActive(true);
		//Set the current attacking hero's selected attack to the chosen skill
		HeroChoice.chosenAttack = chosenSkill;
		//set the skill panels visibility to false(hide it)
		SkillPanel.SetActive(false);
		//set the enemy select panel to true(display it)
		EnemySelectPanel.SetActive(true);
	}

	public void Input4() //Switching to the skill attacks
	{
		//Set the attack panel visibility to false
		AttackPanel.SetActive(false);
		//set the skill panel visibility to true
		SkillPanel.SetActive(true);

	}

    public void Input5(BaseAttack chosenSkill)
    {
        //get the hero attackers name /// THE HERO WHO IS ATTACKING
		HeroChoice.Attacker = HerosToManage[0].name;
		//WHICH HERO IS ATTACKING
		HeroChoice.AttackersGameObject = HerosToManage[0];
        
		//IS THE OBJECT DOING THE ATTACK A HERO?
		HeroChoice.Type = "Hero";
        //Set the current attacking hero's selected attack to the chosen skill
        HeroChoice.chosenAttack = chosenSkill;

        //set the attack panel visibility to false
        AttackPanel.SetActive(false);
		//set the skill panels visibility to false(hide it)
		SkillPanel.SetActive(false);
		
    }

    public void RunAway()
    {
        StartCoroutine(GameManager.instance.BattleCoolDown());
        GameManager.instance.LoadSceneAfterBattle();
        GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
        GameManager.instance.enemiesToBattle.Clear();
        GameManager.instance.playersToBattle.Clear();
    }
}



