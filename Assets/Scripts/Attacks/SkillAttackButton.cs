using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackButton : MonoBehaviour
{
	public BaseAttack skillAttackToPerform;

	public void CastSkill()
	{
		//Find the Battle manager, Get the BattleStateMachine, and pass through the skill casting information from input 3 function.
		GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input3(skillAttackToPerform);
	}
    public void CastSkillHeal()
    {
        //Find the Battle manager, Get the BattleStateMachine, and pass through the skill casting information from input 3 function.
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input5(skillAttackToPerform);
    }

    public void RunAway()
    {
        //Find the Battle manager, Get the BattleStateMachine, and pass through the skill casting information from input 3 function.
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().RunAway();
    }
}
