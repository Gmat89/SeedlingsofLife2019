using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
	public GameObject enemyPrefab;

	public void SelectEnemy()
	{
		GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(enemyPrefab); //save enemy input prefab
	}

	public void HideSelector()
	{
		//Find the selector attached to the enemy prefab set its visibility to true
		enemyPrefab.transform.Find("Selector").gameObject.SetActive(false);

	}

	public void ShowSelector()
	{
		//Find the selector attached to the enemy prefab set its visibility to true
		enemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
	}
}
