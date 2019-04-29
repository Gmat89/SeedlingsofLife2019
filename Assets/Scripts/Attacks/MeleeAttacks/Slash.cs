using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack
{
	public Slash()
	{
		//name of the attack
		attackName = "Slash";
		//description
		attackDescription = "A basic slash attack, mildy damaging";
		//base damage value of slash
		attackDamage = 10f;
		//cost of the slash attack
		attackCost = 0;
	}
}
