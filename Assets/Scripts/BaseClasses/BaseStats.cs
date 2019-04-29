using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStats : MonoBehaviour //BaseClass
{
	public string theName;
	public Sprite image;
	public float Health;
	public int Speed;
    public int Strength;
    public int Skill;
	public int Defense;
    public int Spirit;
	private int level;
	public SeedlingType seedType;

	


	public enum SeedlingType
	{
		Party,
		Enemy
	}

	public enum Rarity
	{
		COMMON,
		UNCOMMON,
		RARE,
		SUPERRARE
	}

	public Type EnemyType;
	public Rarity rarity;


	//anything inheriting from this class can be added to the list
	public List<BaseAttack> attacks = new List<BaseAttack>();
	public List<BaseAttack> skillAttacks = new List<BaseAttack>();
}
