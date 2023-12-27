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
    public static bool noSanity; //Determine si le joueur n'a plus de sanity (santé mental)
    public GameObject leBreaker; //Fait référence au gameobject du breaker
    public int maxSanity; //Le max de sanity que le joueur peut avoir (au début)
    public int sanity; //La sanity courrente du joueur
    int vitesseDeDescentesSanity; // a quel vitesse le santy du joueur descend
    public Slider barreSanity; //Le slider de la barre de sanity
    public TextMeshProUGUI pourcentage; //Le texte représentant le pourcentage de sanity
    public Image fill; //L'image de fond du slider de sanity (vert)
    public AudioClip musiquePanic; //Musique de panique assez intense
    public AudioClip sonPowerOut; //Son du breaker qui ferme
    public VolumeProfile volumePrincipale; //Le volume profile (Post processing en URP)
    Vignette vign; //Variable contenant la vignette rouge pour l'effet de panique
    public GameObject controleurKeyPad; //Le gameobject qui controle le keypad
    public Light lumiereDirectionnel; //La directional light de la scene

    [Header("Tous les lumieres a fermer et leurs materiaux")]
    public Light[] lesLumieres; //Tous les lumières qui vont clignoter
    public Material[] lesMatLumieres; //Avec leur materiaux
    // Start is called before the first frame update
    void Start()
    {
        //On met la sanity a son max
        sanity = maxSanity;
        InvokeRepeating("ReduireNiveauDeSanity", 21f, 1f);
        noSanity = false;

        //Au debut, si il y a une vignette, on met sa valeur a 0 pour pas la voir
        if (volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //On assigne les parametres du slider a ces variables
        barreSanity.maxValue = maxSanity;
        barreSanity.value = sanity;

        //Si le breaker est ouvert la sanity descend moin vite que si il est fermé
        if (breaker.breakerOuvert)
        {
            vitesseDeDescentesSanity = 2;
        }
        else
        {
            vitesseDeDescentesSanity = 4;
        }

        //Si le pourcentage de sanity est plus bas que 30%, on change la couleur de fond a jaune
        if(Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 30 && Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) >= 10)
        {
            fill.color = Color.yellow;
        }
        //Sinon si il est plus bas que 10%, on le met a rouge
        else if (Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 10)
        {
            fill.color = Color.red;
        }

        //Si la sanity du joueur est plus bas que 1%, on declenche le mode panique
        if (Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) <= 1 && !noSanity)
        {
            //Le joueur n'a plus de sanity
            noSanity = true;
            leBreaker.GetComponent<breaker>().FermerBreaker();
            Invoke("AllumerLumieres", 5f);
            Invoke("AllerVersMenuMort", 48f);
            GetComponent<AudioSource>().PlayOneShot(sonPowerOut);
            GetComponent<AudioSource>().PlayOneShot(musiquePanic);
            GetComponent<controleCams>().camsActives = false;
            GetComponent<controleCams>().telephone.GetComponent<Animator>().SetBool("CamsOuvertes", GetComponent<controleCams>().camsActives);
            lumiereDirectionnel.intensity = 0f;

            //Si le keypad est ouvert
            if (controleurKeyPad.activeSelf)
            {
                //On le ferme
                controleurKeyPad.GetComponent<gesrionKeypad>().fermerKeypad();
            }

            //On change la couleur des lumieres pour un rouge
            foreach (Light lum in lesLumieres)
            {
                lum.color = Color.red;
            }
            //Et on désactive l'émission de leurs matériaux
            foreach(Material mat in lesMatLumieres)
            {
                mat.DisableKeyword("_EMISSION");
            }
        }

        //On affecte le texte pourcentage avec un calcule mathématique
        pourcentage.text = Mathf.Ceil(barreSanity.value / barreSanity.maxValue * 100) + "%";
    }

    /*********** Fonction qui fait descendre le niveau de sanity ********************/
    void ReduireNiveauDeSanity()
    {
        sanity -= vitesseDeDescentesSanity; 
    }

    /************** Fonction qui allume les lumieres ********************/
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

        //Et on active aussi la vignette
        if(volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0.4f;
        }

        //On appelle la fonction pour fermer les lumieres
        Invoke("FermerLumieres", 0.7f);
    }

    /***************** Fonction qui ferme les lumières ********************/
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

        //Et on desactive la vignette
        if (volumePrincipale.TryGet<Vignette>(out vign))
        {
            vign.intensity.value = 0f;
        }

        //On appelle la fonction allumerlumieres pour faire un effet de clignotement en boucle
        Invoke("AllumerLumieres", 0.7f);
    }

    /*************** Fonction qui change la scene vers le menu de mort ******************/
    void AllerVersMenuMort()
    {
        SceneManager.LoadScene("LightsOutMort");
    }
}
