using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gestionAvertissement : MonoBehaviour
{
    public static string nomJoueur; //champ de text static qui garde en mémoire le nom du joueur sur toutes les scenes
    public GameObject textExperience; //Le texte pour une meilleur experience
    public GameObject textEntrerNomJoueur; //Le menu pour entrer le nom du joueur
    public TMP_InputField champTextJoueur; //Le input field ou le joueur entre son nom
    // Start is called before the first frame update
    void Start()
    {
        //On fait jouer les animation une a la suite de l'autre
        Invoke("activerLeTextExperience", 10f);
        Invoke("activerInputField", 20f);

        //On reset le nom du joueur a 0
        nomJoueur = "";
    }

    /****************** Fonction qui active le text de meilleur experience et son animation (PlayOnAwake) ********************/
    void activerLeTextExperience()
    {
        textExperience.SetActive(true);
    }

    /****************** Fonction qui active le text du input field et son animation (PlayOnAwake) ********************/
    void activerInputField()
    {
        textEntrerNomJoueur.gameObject.SetActive(true);
    }

    /****************** Fonction qui cahnge la scene pour allers vers le menu principale ********************/
    public void allerVersMenuPrincipale()
    {
        //On affecte le nom du joueur avec ce qui a été écrit dans le input field
        nomJoueur = champTextJoueur.text;

        SceneManager.LoadScene(1);
    }
}
