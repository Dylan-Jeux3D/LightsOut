using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionPorteEtClee : MonoBehaviour
{
    public GameObject PorteQueLaCleeOuvre; //Reference a la porte que le clee ouvre
    public GameObject LaClee; //Reference a la clee que le joueur doit prendre pour ouvrir cette porte
    public bool PeutOuvrirLaPorte; //Est ce que il peut ouvrir la porte

    // Update is called once per frame
    void Update()
    {
        //Si on la clee a été ramassé
        if (PeutOuvrirLaPorte)
        {
            //On debarre la porte
            PorteQueLaCleeOuvre.tag = "PorteDebarre";

            //Et on désactive la clee
            LaClee.SetActive(false);
        }
    }
}
