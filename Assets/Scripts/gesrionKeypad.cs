using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gesrionKeypad : MonoBehaviour
{
    public GameObject imageKeypad;
    public GameObject joueur;

    public TextMeshProUGUI[] lesNbPourCombinaison;
    public int positionCombinaison;
    string mdp;

    void Start()
    {
        positionCombinaison = 1;
    }

    public void fermerKeypad()
    {
        imageKeypad.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        joueur.GetComponent<ControleJoueur>().keypadOuvert = false;
    }

    public void ajouterNumALaCombinaison(GameObject boutonClique)
    {
        if (positionCombinaison < lesNbPourCombinaison.Length + 1)
        {
            foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
            {
                if (unNum.gameObject.name == positionCombinaison.ToString())
                {
                    unNum.text = boutonClique.gameObject.name.ToString();
                }
            }
            positionCombinaison++;
        }

        if(positionCombinaison == lesNbPourCombinaison.Length + 1)
        {
            Invoke("verifierMDP", 0);
        }
    }

    public void verifierMDP()
    {
        foreach(TextMeshProUGUI unNum in lesNbPourCombinaison)
        {
            mdp = mdp + unNum.text;
        }
        print(mdp);
        if (mdp == joueur.GetComponent<ControleJoueur>().mdp)
        {
            Invoke("allezVersMenuVictoire", 2f);
        }
        else
        {
            Invoke("resetMDP", 2f);
        }
    }

    void resetMDP()
    {
        foreach(TextMeshProUGUI unNum in lesNbPourCombinaison)
        {
            unNum.text = "-";
            positionCombinaison = 1;
            mdp = string.Empty;
        }
    }
    void allezVersMenuVictoire()
    {
        SceneManager.LoadScene("LightsOutVictoire");
    }
}
