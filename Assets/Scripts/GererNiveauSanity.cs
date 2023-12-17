using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GererNiveauSanity : MonoBehaviour
{

    public GameObject leBreaker;
    public int maxSanity;
    public int sanity;
    int vitesseDeDescentesSanity;
    public Slider barreSanity;
    public TextMeshProUGUI pourcentage;
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

        pourcentage.text = Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) + "%";
    }

    void ReduireNiveauDeSanity()
    {
        sanity -= vitesseDeDescentesSanity; 
    }
}
