using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
	public string attackName;//name of the attack
	public string attackDescription; //Description  of the attack/ability/item etc
	public float attackDamage;//Base Damage eg: 15 damage, if player level is say 10 and their stamina is 35 = base dmg + stamina + lvl = 60
	public float attackCost; //this is for when the player/enemy uses an ability/spell etc
}
