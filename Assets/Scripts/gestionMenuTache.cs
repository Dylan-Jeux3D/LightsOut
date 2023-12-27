using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class gestionMenuTache : MonoBehaviour
{
    public GameObject menuTache; //Le menu des taches
    public GameObject[] lesTaches; //Les taches a faire
    public int numTacheEnCours; //le id de la tache en cours
    public bool menuTacheOuvert; //bool qui indique si le menu des taches est ouvert ou non

    [Header("Les clées")]
    public GameObject[] lesClees; //Les clées a collecté
    // Start is called before the first frame update
    void Start()
    {
        //Le menu tache est fermé et la tache en cours est la premiere
        menuTacheOuvert = false;
        numTacheEnCours = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur maintient TAB et que les cams sont fermer et que le joueur est en vie
        if (Input.GetKey(KeyCode.Tab) && !GetComponent<controleCams>().camsActives && !ClownDetectionDeJoueur.JoueurMort)
        {
            //On ouvre le menu
            menuTache.SetActive(true);
            menuTacheOuvert = true;
        }
        else
        {
            //Sinon on le ferme
            menuTache.SetActive(false);
            menuTacheOuvert = false;
        }

        //On active la taches correspondant au id de la tache en cours
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
    
    /************** Fonction qui Invoke la verification de tache (pour avoir un delai)********************/
    public void invokerVerificationTache()
    {
        Invoke("verifierSiTacheComplete", 0.5f);
    }

    /**************** Fonction qui vérifie si la tache en cours est complété *******************/
     public void verifierSiTacheComplete()
    {
        //On reset le id de la tache a 1
        numTacheEnCours = 1;

        //Et ensuite, pour chauque tache de complété, on l'augmente de 1
        foreach (GameObject clee in lesClees)
        {
            if (!clee.activeSelf && clee.name != "CleeChambrePrincipale")
            {
                numTacheEnCours++;
            }
            else if(clee.name == "CleeChambrePrincipale" && !clee.activeSelf && breaker.breakerOuvert)
            {
                numTacheEnCours++;
            }
        }
    }
}
