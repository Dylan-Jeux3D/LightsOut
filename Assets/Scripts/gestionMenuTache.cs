using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class gestionMenuTache : MonoBehaviour
{
    public GameObject menuTache;
    public GameObject[] lesTaches;
    public int numTacheEnCours;
    public bool menuTacheOuvert;
    public GameObject breaker;

    [Header("Les clées")]
    public GameObject[] lesClees;
    // Start is called before the first frame update
    void Start()
    {
        menuTacheOuvert = false;
        numTacheEnCours = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            menuTache.SetActive(true);
            menuTacheOuvert = true;
        }
        else
        {
            menuTache.SetActive(false);
            menuTacheOuvert = false;
        }

        foreach (GameObject uneTache in lesTaches)
        {
            if(uneTache.name == "tache" + numTacheEnCours)
            {
                uneTache.SetActive(true);
            }
            else
            {
                uneTache.SetActive(false);
            }
        }

    }
    
    public void invokerVerificationTache()
    {
        Invoke("verifierSiTacheComplete", 0.5f);
    }
     public void verifierSiTacheComplete()
    {
        numTacheEnCours = 1;
        foreach (GameObject clee in lesClees)
        {
            if (!clee.activeSelf && clee.name != "CleeChambrePrincipale")
            {
                numTacheEnCours++;
            }
            else if(clee.name == "CleeChambrePrincipale" && !clee.activeSelf && breaker.GetComponent<breaker>().breakerOuvert)
            {
                numTacheEnCours++;
            }
        }
    }
}
