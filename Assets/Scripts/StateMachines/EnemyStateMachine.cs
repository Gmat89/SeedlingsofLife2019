using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{
	private BattleStateMachine BSM;
	public BaseStats enemyStats;

	public Text enemyHealthText;

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
		//Choose an action to perform
		CHOOSEACTION,
		//State for adding hero to a list
		ADDTOLIST,
		//State for waiting/Idle
		WAITING,
		//State for when the enemy performs an action
		ACTION,
		//State for death
		DEATH,
	}

	//Enum reference
	public TurnState currentState;
	//current cooldown for the Progressbar
	private float currentCoolDown = 0;
	//maximum cooldown for the ProgressBar
	private float maximumCoolDown = 5f;
	//This gameObject references
	private Vector3 startPosition;
	//Selector Reference
	public GameObject Selector;
	//tiemforactionstuff
	private bool actionStarted = false;
	//Reference to the targeted hero (enemy target -----> hero)
	public GameObject HeroToAttack;
	//Animation speed
	private float animSpeed = 10f;
    //Enemy is alive
    private bool enemyIsAlive = true;




	// Use this for initialization
	void Start()
	{
		enemyHealthText.text = enemyStats.Health.ToString("F0");
		//Set the current state to PROCESSING. 
		currentState = TurnState.PROCESSING;
		//set the current selector to false
		Selector.SetActive(false);
		//Find the battle manager then get the battle state machine component
		BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
		startPosition = transform.position;
		enemyStats = FindObjectOfType<BaseStats>();
		myAnim = GetComponent<Animator>();
		changeState(STATE_IDLE);
	}

	// Update is called once per frame
	void Update()
	{
		//Display current state in the console
		//Debug.Log(currentState);

		switch (currentState)
		{
			case (TurnState.PROCESSING):
				UpdateProgressBar();
				break;

			case (TurnState.CHOOSEACTION):
				ChooseAction();
				currentState = TurnState.WAITING;
				break;

			case (TurnState.ACTION):
				StartCoroutine(TimeforAction());
				break;

			case (TurnState.WAITING):

				break;

			case (TurnState.DEATH):
                if(!enemyIsAlive)
                {
                    return;
                }
                else
                {
                    //change tag of enemy so that u know they can't be attacked anymore
                    this.gameObject.tag = "DeadEnemy";
                    //not attackable by heros
                    BSM.EnemiesInBattle.Remove(this.gameObject);
                    //Disable the selector
                    Selector.SetActive(false);
                    //remove all inputs by enemy
                    if(BSM.EnemiesInBattle.Count > 0)
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
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                                }
                            }
                            
                        }
                    }
                    //change the color of the enemy/ play death animation
                   // this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    //set alive to false
                    enemyIsAlive = false;
                    //resets enemy Buttons
                    BSM.EnemyButtons();
                    //check alive
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                }
				break;
		}
	}

	void UpdateProgressBar()
	{
		//Add to the current cooldown based on the time that has past until it reaches the maximum cooldown time
		currentCoolDown = currentCoolDown + Time.deltaTime ;
		//check if the current cooldown is greater than or equal to the max cooldown then...change the current state to ADDTOLIST
		if (currentCoolDown >= maximumCoolDown)
		{
			
			currentState = TurnState.CHOOSEACTION;
		}
	}

	void ChooseAction()
	{
		HandleTurns myAttack = new HandleTurns();
		//find the attacker and get their name
		//Set the attacker string value in the handle turn  to get the component of base stats.theName
		myAttack.Attacker = GetComponent<BaseStats>().theName;
		//myAttack.Type;
		myAttack.Type = "Enemy";
		//Set the attacker gameobject to this gameObject
		myAttack.AttackersGameObject = this.gameObject;
		//Get a random enemy target in a range from the list and choose a random action
		myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
		//perform a random attack assigned to the enemy attacks list
		int num = Random.Range(0, enemyStats.attacks.Count);
		//pass the attack into baseAttack class with the chosen number
		myAttack.chosenAttack = enemyStats.attacks[num];
		//print the attack being done with the name of the attacker, the ability name, the damage to console.
		Debug.Log(this.gameObject.name + "has chosen to use" + myAttack.chosenAttack.attackName + "and did" + myAttack.chosenAttack.attackDamage + "damage!");

		//Set the Battle state machine to use myAttack
		BSM.CollectActions(myAttack);
	}

	private IEnumerator TimeforAction()
	{
		//Check if an action has already started
		if (actionStarted)
		{
			//Stop the coroutine here
			yield break;
		}
		//Otherwise set actionStarted bool to true
		actionStarted = true;
		changeState(STATE_WALK);
		//animate the enemy near the hero to attack the targetted hero, increase the targets.X position
		Vector3 targetHeroPosition = new Vector3 (HeroToAttack.transform.position.x-1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
		while (MoveTowardsEnemy(targetHeroPosition))
		{
			yield return null;
		}
		//wait for an amount of time
		yield return new WaitForSeconds(0.5f);
		changeState(STATE_ATTACK);
		//Do damage
		DoDamage();
		//animate back to the start position
		Vector3 initialPosition = startPosition;
		while (MoveTowardsInitialPosition(initialPosition))
		{
			yield return null;
		}
		//remove the performer from the list in the Battle State Machine so the next enemy can attack
		BSM.PerformList.RemoveAt(0);
		//Reset the battle state machine -> Wait
		BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

		actionStarted = false;
		//reset the enemy state
		currentCoolDown = 0;
		changeState(STATE_IDLE);
		//set the current state to turn state.PROCESSING
		currentState = TurnState.PROCESSING;
	}
    //moves enemy to player when attacking
	private bool MoveTowardsEnemy(Vector3 target)
	{
		changeState(STATE_WALK);
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

    //moves enemy back after they have attacked
	private bool MoveTowardsInitialPosition(Vector3 target)
	{
		changeState(STATE_WALK);
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}

    //do damage to player
	void DoDamage()
	{
		//add the enemies current attack to the chosen attack, when its added to the perform action list.
		float calc_damage = enemyStats.Strength + BSM.PerformList[0].chosenAttack.attackDamage;
		//Apply the damage to the hero that has been attacked based on the calc damage
		HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
		//
		enemyHealthText.text = enemyStats.Health.ToString("F0");
	}

    //take damage from player
	public void TakeDamage(float getDamageAmount) // Enemy takes damage from the hero
	{
		//Take the inputted damage from the enemies hp
		enemyStats.Health -= getDamageAmount / enemyStats.Defense;
		//Check if it the enemies current HP is less than equal to 0
		if (enemyStats.Health <= 0)
		{
			//Set the enemys hp to 0
			enemyStats.Health = 0;
			//Set the enemies turn state to DEATH.
			currentState = TurnState.DEATH;
		}
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

