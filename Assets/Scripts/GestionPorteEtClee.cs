using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionPorteEtClee : MonoBehaviour
{
    public GameObject PorteQueLaCleeOuvre; //Reference a la clee que le joueur doit prendre pour ouvrir cette porte
    public GameObject LaClee;
    public bool PeutOuvrirLaPorte; //Est ce que il peut ouvrir la porte
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PeutOuvrirLaPorte)
        {
            PorteQueLaCleeOuvre.tag = "PorteDebarre";
            LaClee.SetActive(false);
        }
    }
}
