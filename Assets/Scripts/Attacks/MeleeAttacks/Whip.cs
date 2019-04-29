using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : BaseAttack
{
	public Whip()
	{
		//name
		attackName = "Whip";
		//description
		attackDescription = "Whip an enemy with a vine, mildly damaging";
		//base damage
		attackDamage = 15f;
		//cost
		attackCost = 0;
	}
	
}
