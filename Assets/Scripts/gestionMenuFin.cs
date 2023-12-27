using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionMenuFin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //On affiche le curseur de windows
        Cursor.lockState = CursorLockMode.None;

        //et on fait joueur le son de fin
        GetComponent<AudioSource>().Play();
    }

    /*************** Fonction pour recommencer la partie *******************/
    public void recommencerPartie()
    {
        SceneManager.LoadScene(1);
    }

    /*************** Fonction pour quitter le jeu (*seulement sur une version exécutable en .exe*) *******************/
    public void quitterJeu()
    {
        Application.Quit();
    }

    /*************** Fonction pour commencer la partie *******************/
    public void commencerPartie()
    {
        SceneManager.LoadScene(2);
    }
}
