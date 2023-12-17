using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gestionAvertissement : MonoBehaviour
{
    public static string nomJoueur;
    public GameObject textExperience;
    public GameObject textEntrerNomJoueur;
    public TMP_InputField champTextJoueur;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("activerLeTextExperience", 10f);
        Invoke("activerInputField", 20f);
        nomJoueur = "";
    }


    void activerLeTextExperience()
    {
        textExperience.SetActive(true);
    }

    void activerInputField()
    {
        textEntrerNomJoueur.gameObject.SetActive(true);
    }
    public void allerVersMenuPrincipale()
    {
        SceneManager.LoadScene(1);
        nomJoueur = champTextJoueur.text;
    }
}
