using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown_Controller : MonoBehaviour
{
    public GameObject[] lesClownDebut;
    public int idClownActif;
    public AudioClip sonMonstreLoin;
    // Start is called before the first frame update
    void Start()
    {
        idClownActif = 0;
        foreach (GameObject unClown in lesClownDebut)
        {
            unClown.SetActive(false);
        }
        InvokeRepeating("ChangerLeClownActif", 60f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if(idClownActif == lesClownDebut.Length)
        {
            CancelInvoke("ChangerLeClownActif");
        }
    }

    void ChangerLeClownActif()
    {
        idClownActif++;

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
        GetComponent<AudioSource>().PlayOneShot(sonMonstreLoin, 3f);
    }
}
