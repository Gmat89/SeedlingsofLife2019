using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpinnerScript : MonoBehaviour
{
	private int randomValue;

	private float timeInterval;

	private bool coroutineAllowed;

	private int finalAngle;

	[SerializeField]
	private Text winText;

    // Start is called before the first frame update
    private void Start()
    {
	    coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
		    StartCoroutine(Spin());
	    }
    }

	private IEnumerator Spin()
	{
		coroutineAllowed = false;
		randomValue = Random.Range(20, 30);
		timeInterval = 0.1f;

		for (int i = 0; i < randomValue; i++)
		{
			transform.Rotate(0,0,22.5f);
			if (i > Mathf.RoundToInt(randomValue * 0.5f))
			{
				timeInterval = 0.2f;
			}
			if (i > Mathf.RoundToInt(randomValue * 0.85f))
			{
				timeInterval = 0.4f;
			}
			yield return new WaitForSeconds(timeInterval);
		}
		 
		if (Mathf.RoundToInt((transform.eulerAngles.z)) % 45 != 0)
		{
			transform.Rotate(0, 0, 22.5f);
		}

		finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);

		switch (finalAngle)
		{
			case 0:
				winText.text = "You Win 1(Chocolate Lilly Seed Pack)";
				//spawn choclilly pack
				//add to inventory
				//carry to next level
				break;
			case 45:
				winText.text = "You Win 2(TeaTree Seed Pack)";
				break;
			case 90:
				winText.text = "You Win 3 (Gumnut SeedPack)";
				break;
			case 135:
				winText.text = "You Win 4 (Mushroom Seed Pack)";
				break;
			case 180:
				winText.text = "You Win 5(Orchid Seed Pack)";
				break;
			case 225:
				winText.text = "You Win 6(TeaTree SeedPack)";
				break;
			case 270:
				winText.text = "You Win 7(Gumnut Seed Pack)";
				break;
			case 315:
				winText.text = "You Win 8";
				break;
		}
		coroutineAllowed = true;


	}
}
