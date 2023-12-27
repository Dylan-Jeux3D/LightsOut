using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breaker : MonoBehaviour
{
    public static bool breakerOuvert;//Un bool public qui dit a d'autre script si le breaker est ouvert ou non
    public Material matLumiereBreaker; //Le materiel de la lumiere du breaker
    public Light lumiereBreaker; //La lumiere attachee au breaker

    [Header("TOUS les lumieres du jeu")]
    public GameObject[] tousLesLumieres; //Tous les lumieres du jeu (Array)

    [Header("Les sons")]
    public AudioClip sonPowerDown; //Son du debut lorsque les lumieres se ferme tout seul
    public GameObject sonAmbiance; //Le son d'ambiance creepy une fois que les lumieres sont fermer

    // Start is called before the first frame update
    void Start()
    {
        //On met la couleur de l'emission du material de la lumiere a vert
        matLumiereBreaker.SetColor("_EmissionColor", Color.green);

        //On met la lumiere a vert
        lumiereBreaker.color = Color.green;

        //On enleve tous le tag de chaque lumiere
        foreach (GameObject lum in tousLesLumieres)
        {
            lum.gameObject.tag = "Untagged";
        }

        //Le breaker est ouvert au debut
        breakerOuvert = true;

        //Apres 20 secondes, on ferme tous les lumiers et on joue le son intense
        Invoke("FermerBreaker", 20f);
        Invoke("JouerSonPowerOut", 20f);
    }

    /***************** Fonction qui allume le breaker ***************************/
    public void AllumerBreaker()
    {
        //On change l'etat du breaker
        breakerOuvert = !breakerOuvert;

        //On active l'animation
        GetComponent<Animator>().SetBool("breakerOuvert", breakerOuvert);

        //On met la couleur de l'emission du material de la lumiere a vert
        matLumiereBreaker.SetColor("_EmissionColor", Color.green);

        //On change la couleur de la lumiere a vert
        lumiereBreaker.color = Color.green;

        //On active tous les lumieres qui ont le tag "LumAcive" (grace au tag, on peut garder en memoire
        //les lumieres qui etaient activent lorsque le breaker a ete desactiver)
        foreach (GameObject lum in tousLesLumieres)
        {
            if (lum.gameObject.tag == "LumActive")
            {
                lum.gameObject.SetActive(true);
            }
        }
    }

    /*********************** Fonction qui ferme le breaker ******************************/
    public void FermerBreaker()
    {
        //On change l'etat du breaker
        breakerOuvert = !breakerOuvert;

        //On met la couleur de l'emission du material de la lumiere a rouge
        matLumiereBreaker.SetColor("_EmissionColor", Color.red);

        //On active l'animation
        GetComponent<Animator>().SetBool("breakerOuvert", breakerOuvert);

        //On change la couleur de la lumiere a rouge
        lumiereBreaker.color = Color.red;

        //On desactive tous les lumieres
        foreach (GameObject lum in  tousLesLumieres)
        {
            if (lum.gameObject.tag == "LumActive")
            {
                lum.gameObject.SetActive(false);
            }
        }
    }

    /******************** Fonction qui fait jouer les sons quand le breaker ferme (AU DEBUT SEULEMENT) ****************************/
    void JouerSonPowerOut()
    {
        GetComponent<AudioSource>().PlayOneShot(sonPowerDown, 2f);
        sonAmbiance.GetComponent<AudioSource>().Play();
    }
}
