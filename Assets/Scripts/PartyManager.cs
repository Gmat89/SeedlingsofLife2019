using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<GameObject> seedlingsInParty = new List<GameObject>();

    public int maxPartyMembers;

    

    private void Awake()
    {
        //thePartyUI = GetComponent<PartyManagerUI>();
        if (seedlingsInParty.Count < maxPartyMembers)
        {
            //AddSeedling(gameObject);
        }
        else
        {
            return;
        }

        


    }

//Function to add a seedling to the party manager list
    public void AddGumnut()
    {
        GumnutSeed GNS = FindObjectOfType<GumnutSeed>().GetComponent<GumnutSeed>();
        //TeaTreeSeed TTS = FindObjectOfType<TeaTreeSeed>().GetComponent<TeaTreeSeed>();
        //Find the seedling in the Assets/Resources/Prefabs directory
        var GumNut = Resources.Load<GameObject>("Prefabs/GumNut");

        
        if (GNS.CompareTag("Gumnut"))
        {
            seedlingsInParty.Add(GumNut);
            maxPartyMembers = seedlingsInParty.Count;
            
            
        }



        //  maxPartyMembers = seedlingsInParty.Count;
    }

    public void AddTeaTree()
    {
        TeaTreeSeed TTS = FindObjectOfType<TeaTreeSeed>().GetComponent<TeaTreeSeed>();
        var TeaTree = Resources.Load<GameObject>("Prefabs/TeaTree");

        if (TTS.CompareTag("Teatree"))
        {
            seedlingsInParty.Add(TeaTree);
            maxPartyMembers = seedlingsInParty.Count;
        }

    }

    public void AddFungi()
    {
        FungiSeed FS = FindObjectOfType<FungiSeed>().GetComponent<FungiSeed>();
        //Find the seedling in the Assets/Resources/Prefabs directory
        var Fungi = Resources.Load<GameObject>("Prefabs/Fungi");


        if (FS.CompareTag("Fungi"))
        {
            seedlingsInParty.Add(Fungi);
            maxPartyMembers = seedlingsInParty.Count;
        }

    }

    public void AddVine()
    {
        VineSeed VS = FindObjectOfType<VineSeed>().GetComponent<VineSeed>();
        //Find the seedling in the Assets/Resources/Prefabs directory
        var Vine = Resources.Load<GameObject>("Prefabs/Ivy");


        if (VS.CompareTag("Vine"))
        {
            seedlingsInParty.Add(Vine);
            maxPartyMembers = seedlingsInParty.Count;
        }
    }

    public void AddLily()
    {
        LilySeed LS = FindObjectOfType<LilySeed>().GetComponent<LilySeed>();
        //Find the seedling in the Assets/Resources/Prefabs directory
        var Lily = Resources.Load<GameObject>("Prefabs/Lily");


        if (LS.CompareTag("Lily"))
        {
            seedlingsInParty.Add(Lily);
            maxPartyMembers = seedlingsInParty.Count;
        }
    }


    public void AddOrchid()
    {
        OrchidSeed OS = FindObjectOfType<OrchidSeed>().GetComponent<OrchidSeed>();
        //Find the seedling in the Assets/Resources/Prefabs directory
        var Orchid = Resources.Load<GameObject>("Prefabs/Orchid");


        if (OS.CompareTag("Orchid"))
        {
            seedlingsInParty.Add(Orchid);
            maxPartyMembers = seedlingsInParty.Count;
        }
    }
}
