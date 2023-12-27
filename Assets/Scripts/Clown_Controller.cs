using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown_Controller : MonoBehaviour
{
    public GameObject[] lesClownDebut; //Les clowns a activer/desactiver
    public int idClownActif; //le id du clown actif
    public AudioClip sonMonstreLoin; //Son d'un ricanement qui indique que le id du monstre actif a changé
    // Start is called before the first frame update
    void Start()
    {
        //aucun clown n'est activé au debut du jeu
        idClownActif = 0;
        foreach (GameObject unClown in lesClownDebut)
        {
            unClown.SetActive(false);
        }

        //On active le premier clown apres 120s et on le change a chaque 60s
        InvokeRepeating("ChangerLeClownActif", 120f, 60f);
    }

    // Update is called once per frame
    void Update()
    {
        //Si le clown finale est activé
        if(idClownActif == lesClownDebut.Length)
        {
            //On arrete d'incrémenter le id du clown actif
            CancelInvoke("ChangerLeClownActif");
        }
    }

    /****************** Fonction qui change le clown actif **************************/
    void ChangerLeClownActif()
    {
        //On incrémente le id du clown actif
        idClownActif++;

        //On active le clown avec le bon id
        foreach(GameObject unClown in lesClownDebut)
        {
            if(unClown.name == "Clown(Debut)" + idClownActif)
            {
                unClown.SetActive(true);
            }
            else
            {
                unClown.SetActive(false);
            }
        }
        //Et on fait jouer le son distant
        GetComponent<AudioSource>().PlayOneShot(sonMonstreLoin, 3f);
    }
}
