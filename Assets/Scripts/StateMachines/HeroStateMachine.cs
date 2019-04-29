using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
	private BattleStateMachine BSM;

	//Reference to the BaseStats Class
	public BaseStats heroStats;

	private Animator myAnim;
	
	
	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_ATTACK = 2;
	const int STATE_DEATH = 3;
	
	
	
	int _currentAnimationState = STATE_IDLE;
	//Enum Declaration
	public enum TurnState
	{
		//State for when the bar is filling
		PROCESSING,

		//State for adding hero to a list
		ADDTOLIST,

		//State for waiting/Idle
		WAITING,

		//State for when the player can performs an action
		ACTION,

		//State for death
		DEATH,
	}

	//Enum reference
	public TurnState currentState;

	//For the Progressbar
	public float currentCoolDown = 0;

	public float maximumCoolDown = 5f;

	//reference to the ProgressBar
	private Image theProgressBar;

	//Selector Reference
	public GameObject Selector;

	//IEnumerator
	public GameObject EnemyToAttack;

	//bool for action started
	private bool actionStarted = false;

	//player start position
	private Vector3 startPosition;

	//animation speed
	private float animSpeed = 10f;

	//bool for player alive
	private bool playerIsAlive = true;
	//hero panel references
	private HeroPanelStats stats;
	public GameObject heroPanel;
	private Transform heroPanelSpacer;

	// Use this for initialization
	// Use this for initialization
	void Start()
	{
		//find the spacer object in the scene
		heroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
		//create the panel and fill it with info with the corresponding hero stats
		CreateHeroPanel();

		//set the players transform to the startPositon
		startPosition = transform.position;
		//set the current cooldown to a random range to add randomness to the progress bars (can use luck stat to alter/speed stat
		currentCoolDown = 0f;//Random.Range(0, 2.5f);
		//set the current selector to false
		Selector.SetActive(false);
		//Find the battle manager
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
		//Set the current state to PROCESSING. 
		currentState = TurnState.PROCESSING;
		myAnim = GetComponent<Animator>();
		changeState(STATE_IDLE);


	}

	// Update is called once per frame
	void Update()
	{
		//Display current state in the console
		Debug.Log(currentState);

		switch (currentState)
		{
			case (TurnState.PROCESSING):
				UpdateProgressBar();

				break;

			case (TurnState.ADDTOLIST):
				//Add whichever hero is currently selected to the HerosToManage List.
				BSM.HerosToManage.Add(this.gameObject);
				//set the current state to turnstate.WAITING
				currentState = TurnState.WAITING;
				break;

			case (TurnState.WAITING):
				//idle state
				break;

			case (TurnState.ACTION):
				//start the coroutine for performing an action
				StartCoroutine(TimeforAction());
				break;

			case (TurnState.DEATH):
				if (!playerIsAlive)
				{
					return;
				}
				else
				{
					//change the tag of the hero
					this.gameObject.tag = "DeadHero";
					//the player is not attackable by the enemy
					BSM.HerosInBattle.Remove(this.gameObject);
					//the player is no longer manageable
					BSM.HerosToManage.Remove(this.gameObject);
					//deactivate the selector
					Selector.SetActive(false);
					//reset the GUI
					BSM.AttackPanel.SetActive(false);
					BSM.EnemySelectPanel.SetActive(false);
                    //remove the object from the perform list
                    if(BSM.HerosInBattle.Count > 0)
                    { 
                        for (int i = 0; i < BSM.PerformList.Count; i++)
					    {
                            if(i != 0)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                                }
                            }
                        }
                    }
                    //change the colour of the player / play the death animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105,105,105,255);
                    //reset the players input
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
					playerIsAlive = false;

					

				}
				break;
		}
	}

	void UpdateProgressBar()
	{
		//Add to the current cooldown based on the time that has past until it reaches the maximum cooldown time
		currentCoolDown = currentCoolDown + Time.deltaTime * heroStats.Speed;
		//value for calculating the cooldown
		float calc_cooldown = currentCoolDown / maximumCoolDown;
		//clamp the x value between 0 and 1 of the cooldown value to represent the current progressbar
		theProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1),
			theProgressBar.transform.localScale.y, theProgressBar.transform.localScale.z);
		//check if the current cooldown is greater than or equal to the max cooldown then...change the current state to ADDTOLIST
		if (currentCoolDown >= maximumCoolDown)
		{
			currentState = TurnState.ADDTOLIST;
		}
	}

	private IEnumerator TimeforAction()
	{
		//Check if an action has already started
		if (actionStarted)
		{
			//Stop the coroutine here
			yield break;
		}

		currentCoolDown = 0f;
		//Otherwise set actionStarted bool to true
		actionStarted = true;
		changeState(STATE_WALK);
		//animate the player near the enemy to attack the targeted enemy, increase the targets.X position
		Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y,
			EnemyToAttack.transform.position.z);
		while (MoveTowardsEnemy(enemyPosition))
		{
			yield return null;
		}
		//wait for an amount of time
		yield return new WaitForSeconds(0.5f);
		changeState(STATE_ATTACK);
		//Do damage
		DoDamage();
		changeState(STATE_WALK);
		//animate back to the start position
		Vector3 initialPosition = startPosition;
		changeState(STATE_IDLE);
		while (MoveTowardsInitialPosition(initialPosition))
		{
			yield return null;
		}
		//remove the performer from the list in the Battle State Machine so the next enemy can attack
		BSM.PerformList.RemoveAt(0);
		
        if(BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            //Reset the battle state machine -> Wait
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //reset the enemy state
            currentCoolDown = 0;
            //set the current state to turn state.PROCESSING
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        actionStarted = false;
		
	}

    //moves player to enemy, affects visuals not gameplay
	private bool MoveTowardsEnemy(Vector3 target)
	{
		changeState(STATE_WALK);
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

    //moves back to original position before moving towards enemy
	private bool MoveTowardsInitialPosition(Vector3 target)
	{
		changeState(STATE_WALK);
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

    //does damage to enemy when they move next to them
	public void TakeDamage(float getDamageAmount)
	{
		//apply the damage to the hero based on the getDamageAmount
		heroStats.Health -= getDamageAmount / heroStats.Defense;
		//check if the current hero is dead
		if (heroStats.Health <= 0)
		{
			heroStats.Health = 0;
			currentState = TurnState.DEATH;
		}
		UpdateHeroPanel();
	}

	//Do Damage function
	void DoDamage()
	{
		//Calculate the damage that the hero will do from the battle state machine/ chosen attack command + the attack damage
		float calc_Damage = heroStats.Strength + BSM.PerformList[0].chosenAttack.attackDamage;
		//get the enemy that has been attacked, call the take damage function from the Enemy State Machine and input the damage based on the calc_damage from the Hero
		EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_Damage);
	}
	//create a hero panel
	void CreateHeroPanel()
	{
		//spawn the hero panel as a game object
		heroPanel = Instantiate(heroPanel) as GameObject;
		//get the hero panel stats script attached to the stats object
		stats = heroPanel.GetComponent<HeroPanelStats>();
		//display the current hero name
		stats.heroName.text = heroStats.theName;
		//display the current hero hp
		stats.heroHP.text = "HP: " + heroStats.Health;
		//display the current hero mp
		stats.heroSP.text = "SP: " + heroStats.Skill;
		//display the current progress bar
		theProgressBar = stats.progressBar;
		//set the hero panel to the parent spacer panel game object without changing the local scale.
		heroPanel.transform.SetParent(heroPanelSpacer, false);
	}
	//Update the stats/values in the hero panel when the player takes damage, uses abilities etc
	void UpdateHeroPanel()
	{
		//display the current hero hp
		stats.heroHP.text = "HP: " + heroStats.Health;
		//display the current hero mp
		stats.heroSP.text = "SP: " + heroStats.Health;
	}
	
	void changeState(int state){
 
		if (_currentAnimationState == state)
			return;
 
		switch (state) {
 
			case STATE_WALK:
				myAnim.SetTrigger("Walk");
				break;
 
			case STATE_ATTACK:
				myAnim.SetTrigger ("Attack");
				break;
 
			case STATE_DEATH:
				myAnim.SetTrigger("Death");
				break;
 
			case STATE_IDLE:
				myAnim.SetTrigger("Idle");
				break;
		}
		_currentAnimationState = state;
	}
}

