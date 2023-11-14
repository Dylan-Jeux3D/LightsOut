using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GererNiveauSanity : MonoBehaviour
{

    public GameObject leBreaker;
    public int maxSanity;
    public int sanity;
    int vitesseDeDescentesSanity;
    public Slider barreSanity;
    // Start is called before the first frame update
    void Start()
    {
        sanity = maxSanity;
        InvokeRepeating("ReduireNiveauDeSanity", 21f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        barreSanity.maxValue = maxSanity;
        barreSanity.value = sanity;

        if (leBreaker.GetComponent<breaker>().breakerOuvert)
        {
            vitesseDeDescentesSanity = 1;
        }
        else
        {
            vitesseDeDescentesSanity = 5;
        }
    }

    void ReduireNiveauDeSanity()
    {
        sanity -= vitesseDeDescentesSanity; 
    }
}
