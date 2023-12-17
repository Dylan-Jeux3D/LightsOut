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
    public AudioClip sonBoutton;
    public AudioClip sonMauvaisMDP;
    public AudioClip sonBonMDP;

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
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);
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
    }

    public void verifierMDP()
    {
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);
        foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
        {
            mdp = mdp + unNum.text;
        }
        print(mdp);
        if (mdp == joueur.GetComponent<ControleJoueur>().mdp)
        {
            GetComponent<Animator>().SetTrigger("bon");
            GetComponent<AudioSource>().PlayOneShot(sonBonMDP);
            Invoke("allezVersMenuVictoire", 5f);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("mauvais");
            GetComponent<AudioSource>().PlayOneShot(sonMauvaisMDP);
            Invoke("resetMDP", 2f);
        }

    }

    public void resetMDP()
    {
        GetComponent<AudioSource>().PlayOneShot(sonBoutton);
        foreach (TextMeshProUGUI unNum in lesNbPourCombinaison)
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
