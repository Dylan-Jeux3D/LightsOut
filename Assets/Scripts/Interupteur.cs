using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interupteur : MonoBehaviour
{

    public GameObject[] lesLumieres;
    public Material[] materielLumiere;
    public bool lumOuverte;
    public GameObject breaker;

    public void Start()
    {
        if (tag == "InterupteurDebut")
        {
            AllumerLuieres();
            lumOuverte = false;
            Invoke("FermerLumieres", 20f);
        }

    }

    private void Update()
    {
        if (!breaker.GetComponent<breaker>().breakerOuvert && !GererNiveauSanity.noSanity)
        {
            foreach (Material unMaterial in materielLumiere)
            {
                unMaterial.DisableKeyword("_EMISSION");
            }
        }
        else if (lumOuverte && breaker.GetComponent<breaker>().breakerOuvert && !GererNiveauSanity.noSanity)
        {
            foreach (Material unMaterial in materielLumiere)
            {
                unMaterial.EnableKeyword("_EMISSION");
            }
        }
    }

    public void AllumerLuieres()
    {
        lumOuverte = true;
        foreach (GameObject uneLumieres in lesLumieres)
        {
            uneLumieres.SetActive(true);
            uneLumieres.gameObject.tag = "LumActive";
        }

        foreach(Material unMaterial in materielLumiere)
        {
            unMaterial.EnableKeyword("_EMISSION");
        }
    }

    public void FermerLumieres()
    {
        lumOuverte = false;
        foreach (GameObject uneLumieres in lesLumieres)
        {
            uneLumieres.SetActive(false);
            uneLumieres.gameObject.tag = "Untagged";
        }

        foreach (Material unMaterial in materielLumiere)
        {
            unMaterial.DisableKeyword("_EMISSION");
        }
    }
}
