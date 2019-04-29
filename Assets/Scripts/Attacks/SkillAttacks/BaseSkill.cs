using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : BaseAttack
{
    public BaseSkill()
    {
        attackName = "";
        attackDescription = "";
        attackDamage = 10f;
        attackCost = 5f;
    }
}
