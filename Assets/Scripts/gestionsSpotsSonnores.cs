using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionsSpotsSonnores : MonoBehaviour
{
    public float tempsCouperSon;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Joueur" && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
            Invoke("enleverTrigger", tempsCouperSon);
        }
    }

    void enleverTrigger()
    {
        gameObject.SetActive(false);
    }
}
