using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interupteur : MonoBehaviour
{

    public GameObject[] lesLumieres; //les lumieres qui sont allumer par l'interrupteur
    public Material[] materielLumiere; //Les materiaux associé aux lumières
    public bool lumOuverte; //bool qui indique si les lumieres sont active ou non

    public void Start()
    {
        if (tag == "InterupteurDebut")
        {
            //On ouvre les lumieres de tous les interupteurs qui ont le tag InterupteurDebut
            AllumerLuieres();
            lumOuverte = false;
            Invoke("FermerLumieres", 20f);
        }

    }

    private void Update()
    {
        //Si le breaker est fermer et que le joueur a encore de la sanity
        if (!breaker.breakerOuvert && !GererNiveauSanity.noSanity)
        {
            foreach (Material unMaterial in materielLumiere)
            {
                //On desactive les l'émission des materiaux
                unMaterial.DisableKeyword("_EMISSION");
            }
        }
        //Sinon si les lumieres sont allumer 
        else if (lumOuverte && breaker.breakerOuvert && !GererNiveauSanity.noSanity)
        {
            foreach (Material unMaterial in materielLumiere)
            {
                //On active l'émission des materiaux
                unMaterial.EnableKeyword("_EMISSION");
            }
        }
    }

    /****************** Fonction pour allumer les lumieres **********************/
    public void AllumerLuieres()
    {
        //On dit que les lumieres sont ouverte
        lumOuverte = true;
        foreach (GameObject uneLumieres in lesLumieres)
        {
            //On active les lumieres et on leurs donne le tag de lumActive
            uneLumieres.SetActive(true);
            uneLumieres.gameObject.tag = "LumActive";
        }

        //ET on active l'émission des materiaux associé
        foreach(Material unMaterial in materielLumiere)
        {
            unMaterial.EnableKeyword("_EMISSION");
        }
    }

    /***************** Fonction pour fermer les lumieres ********************/
    public void FermerLumieres()
    {
        //On dit que les lumieres sont fermer
        lumOuverte = false;

        //On desactive les lumieres et on leur retire le tag de lumActive
        foreach (GameObject uneLumieres in lesLumieres)
        {
            uneLumieres.SetActive(false);
            uneLumieres.gameObject.tag = "Untagged";
        }

        //ET on desactive l'émission des mats associés
        foreach (Material unMaterial in materielLumiere)
        {
            unMaterial.DisableKeyword("_EMISSION");
        }
    }
}
