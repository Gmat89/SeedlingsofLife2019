using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionData : MonoBehaviour
{
    //how max amount of enemies that can spawn in battle
    public int maxAmountEnemies = 4;
    //what level u want to load
    public string BattleScene;
    //a list of all possible enemy prefabs
    public List<GameObject> possibleEnemies = new List<GameObject>();  
}
