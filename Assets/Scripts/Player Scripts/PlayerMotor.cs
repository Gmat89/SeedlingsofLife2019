using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//ensure there is always a navmesh to get
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
	//reference to the NavMeshAgent
	private NavMeshAgent agent;

	//Target Reference
	public Transform target;

	// Start is called before the first frame update
	void Start()
	{
		//Get the NavMeshAgent attached to the player
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		// if there is a target
		if (target != null)
		{
			//set the players destination to the targeted objects position
			agent.SetDestination(target.position);
			FaceTarget();

		}
	}

	public void MoveToPoint(Vector3 point)
	{
		//Move the player to the specified point
		agent.SetDestination(point);
	}

	public void FollowTarget(Interactable newTarget)
	{
		//stop the player moving when it gets to the object (NOT INSIDE IT)
		agent.stoppingDistance = newTarget.radius * .8f;

		//lock onto the targeted object
		agent.updateRotation = false;
		//set the target to the newly selected objects transform
		target = newTarget.interactionTransform;
		
	}
	//function to stop the player from turning and facing the target
	public void StopFollowingTarget()
	{
		//set the agent stopping distance to zero
		agent.stoppingDistance = 0f;
		//stop locking onto the object
		agent.updateRotation = true;
		//set the target object to null
		target = null;
	}

	//function to face the targeted object
	void FaceTarget()
	{
		//from our position to the targets position
		Vector3 direction = (target.position - transform.position).normalized;
		//Calculation to determine where to turn to look
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime * 5f);
	}
}
