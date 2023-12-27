using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gesrionKeypad : MonoBehaviour
{
    public GameObject imageKeypad; //L'image du keypad
    public GameObject joueur; //Le joueur

    /* Section pour les sons*/
    public AudioClip sonBoutton; 
    public AudioClip sonMauvaisMDP;
    public AudioClip sonBonMDP;

    public TextMeshProUGUI[] lesNbPourCombinaison; //Les nombres a entrer dans le UI
    public int positionCombinaison; //Le position du chiffre que le joueur doit entrer
    string mdp; //Le mot de passe insérer dans le champ de texte par le joueur (les 4 num que le joueur a entrer)

    void Start()
    {
        //Au debut on doit entrer le prmeier chiffre
        positionCombinaison = 1;
    }

    /**************************** Fonction pour fermer le keypad (appelé par un bouton) ***********************************/
    public void fermerKeypad()
    {
        imageKeypad.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        joueur.GetComponent<ControleJoueur>().keypadOuvert = false;
    }

    /************************* Fonction pour ajouter un numero dans le champ pour entrer le mdp (appelé par des boutons)*********************************/
    //Parametre GameObject boutonClique: le bouton qui a été cliqué 
    public void ajouterNumALaCombinaison(GameObject boutonClique)
    {
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);
        if (positionCombinaison < lesNbPourCombinaison.Length + 1)
        {
            foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
            {
                if (unNum.gameObject.name == positionCombinaison.ToString())
                {
                    //On affecte le nom du bouton cliqué au text de la position actuel de la combinaison
                    unNum.text = boutonClique.gameObject.name.ToString();
                }
            }
            //Et on augmente la position de la combinaison 
            positionCombinaison++;
        }
    }

    /************* Fonction pour vérifier si le champs correspond au mdp **********************/
    public void verifierMDP()
    {
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);

        //On insert chaque num de la combinaison du champ dans le mdp en un seul string
        foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
        {
            mdp = mdp + unNum.text;
        }

        //Si le mot de passe est bon
        if (mdp == joueur.GetComponent<ControleJoueur>().mdp)
        {
            //On fait joueur le son, l'animation et on envoie le joueur au menu de victoire
            GetComponent<Animator>().SetTrigger("bon");
            GetComponent<AudioSource>().PlayOneShot(sonBonMDP);
            Invoke("allezVersMenuVictoire", 3f);
        }
        else
        {
            //On fait joueur le son, l'animation et on reset le combinaison du champ
            GetComponent<Animator>().SetTrigger("mauvais");
            GetComponent<AudioSource>().PlayOneShot(sonMauvaisMDP);
            Invoke("resetMDP", 2f);
        }

    }

    /********************** Fonction de remise a 0 du champs pour inserer la combinaison *******************************/
    public void resetMDP()
    {
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);
        foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
        {
            //On remet les num a leur valeur de base
            unNum.text = "-";

            //On remet la position a 1
            positionCombinaison = 1;

            //On vide le champ
            mdp = string.Empty;
        }
    }

    /*************************** Fonction pour allez vers le menu de victoire ***************************************/
    void allezVersMenuVictoire()
    {
        SceneManager.LoadScene("LightsOutVictoire");
    }
}
