using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyManagerUI : MonoBehaviour
{

    public Text memberName;

    public Image memberImage;

    public Text memberHealth;

    public Text memberSP;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MemberInfo(string name, int hp, int mp)
    {
        memberName.text = name;
        memberHealth.text = hp.ToString();
        memberSP.text = mp.ToString();
    }
    
}
