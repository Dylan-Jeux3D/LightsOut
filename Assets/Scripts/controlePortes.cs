using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlePortes : MonoBehaviour
{
    public bool porteOuverte; //bool qui dit si la porte est ouverte ou non

    // Update is called once per frame
    void Update()
    {
        //On affecte cette valeur a l'animator pour joueur l'animation correspondante
        GetComponent<Animator>().SetBool("PorteOuverte", porteOuverte);
    }
}
