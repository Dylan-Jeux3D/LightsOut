using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GererNiveauSanity : MonoBehaviour
{
    public static bool noSanity;
    public GameObject leBreaker;
    public int maxSanity;
    public int sanity;
    int vitesseDeDescentesSanity;
    public Slider barreSanity;
    public TextMeshProUGUI pourcentage;
    public Image fill;
    public AudioClip musiquePanic;
    public AudioClip sonPowerOut;
    public VolumeProfile volumePrincipale;
    Vignette vign;
    public GameObject controleurKeyPad;
    public Light lumiereDirectionnel;

    [Header("Tous les lumieres a fermer et leurs materiaux")]
    public Light[] lesLumieres;
    public Material[] lesMatLumieres;
    // Start is called before the first frame update
    void Start()
    {
        sanity = maxSanity;
        InvokeRepeating("ReduireNiveauDeSanity", 21f, 1f);
        noSanity = false;
        if (volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        barreSanity.maxValue = maxSanity;
        barreSanity.value = sanity;

        if (leBreaker.GetComponent<breaker>().breakerOuvert)
        {
            vitesseDeDescentesSanity = 2;
        }
        else
        {
            vitesseDeDescentesSanity = 4;
        }

        if(Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 30 && Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) >= 10)
        {
            fill.color = Color.yellow;
        }
        else if (Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 10)
        {
            fill.color = Color.red;
        }

        if (Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 10 && !noSanity)
        {
            noSanity = true;
            leBreaker.GetComponent<breaker>().FermerBreaker();
            Invoke("AllumerLumieres", 5f);
            Invoke("AllerVersMenuMort", 40f);
            GetComponent<AudioSource>().PlayOneShot(sonPowerOut);
            GetComponent<AudioSource>().PlayOneShot(musiquePanic);
            GetComponent<controleCams>().camsActives = false;
            GetComponent<controleCams>().telephone.GetComponent<Animator>().SetBool("CamsOuvertes", GetComponent<controleCams>().camsActives);
            lumiereDirectionnel.intensity = 0f;

            if (controleurKeyPad.activeSelf)
            {
                controleurKeyPad.GetComponent<gesrionKeypad>().fermerKeypad();
            }

            foreach (Light lum in lesLumieres)
            {
                lum.color = Color.red;
            }
            foreach(Material mat in lesMatLumieres)
            {
                mat.DisableKeyword("_EMISSION");
            }
        }

        pourcentage.text = Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) + "%";
    }

    void ReduireNiveauDeSanity()
    {
        sanity -= vitesseDeDescentesSanity; 
    }

    void AllumerLumieres()
    {
        foreach(Light lum in lesLumieres)
        {
            lum.gameObject.SetActive(true);
        }
        foreach (Material mat in lesMatLumieres)
        {
            mat.EnableKeyword("_EMISSION");
        }

        if(volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0.4f;
        }
        Invoke("FermerLumieres", 0.7f);
    }

    void FermerLumieres()
    {
        foreach(Light lum in lesLumieres)
        {
            lum.gameObject.SetActive(false);
        }
        foreach (Material mat in lesMatLumieres)
        {
            mat.DisableKeyword("_EMISSION");
        }

        if (volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0f;
        }
        Invoke("AllumerLumieres", 0.7f);
    }

    void AllerVersMenuMort()
    {
        SceneManager.LoadScene("LightsOutMort");
    }
}
