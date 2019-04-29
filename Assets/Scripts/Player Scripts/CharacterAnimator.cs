using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
	//a const float to smooth out the animation
	private const float locomotionAnimationSmoothTime = 0.1f;
	//animator reference
	private Animator animator;
	//nav mesh agent reference
	private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
	{
		//get the nav mesh agent component attached to the player
		agent = GetComponent<NavMeshAgent>();
		//get the animator attached to the player
		animator = GetComponentInChildren<Animator>();
	}

    // Update is called once per frame
    void Update()
	{
		//local variable speedPercent, set it to the agents velocity divided by the agents speed
		float speedPercent = agent.velocity.magnitude / agent.speed;
		//clamp the speedPercent calculation between what the value is and 0 to 1
		speedPercent = Mathf.Clamp(speedPercent, 0f, 1f);
		//set the animator float value to the speedPercent value with the smooth time and by time(each frame)
		animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
	}
}
