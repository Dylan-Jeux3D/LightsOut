using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionsSpotsSonnores : MonoBehaviour
{
    public float tempsCouperSon; //le temps apr�s lequel l'object de son doit se d�sactiver

    private void OnTriggerEnter(Collider collision)
    {
        //Si le joueur entre dans la zone et que le son n'est pas deja entrain de jouer
        if (collision.gameObject.name == "Joueur" && !GetComponent<AudioSource>().isPlaying)
        {
            //On fait jouer le son
            GetComponent<AudioSource>().Play();

            //On d�sactive le gameObject apres un delai
            Invoke("enleverTrigger", tempsCouperSon);
        }
    }

    /****************** Fonction pour desactiver le gameobject qui a le script (la zone) ****************************/
    void enleverTrigger()
    {
        gameObject.SetActive(false);
    }
}
