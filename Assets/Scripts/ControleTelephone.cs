using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControleTelephone : MonoBehaviour
{
    public GameObject[] Lumiere; //Les lumi�res du t�l�phone
    bool LumiereAllumer; //bool qui indique si les lum du t�l�phone son allumer
    public bool peutOuvrirCams; //bool qui indique si le joueur peut ouvrir les cams de s�curit�
    bool peutOuvrirLum; // bool qui indique si le joueur peut ouvrir les lum du t�l�phone

    /* Les images sur le t�l�phone indiquant les touches */
    public GameObject cameraLogo;
    public GameObject toucheCamera;
    public TextMeshPro textTouche;

    public Color couleurTransparente; //Couleur personnalis� dans l'inspecteur pour un effet de transparence
    // Start is called before the first frame update
    void Start()
    {
        //Au debut on ferme les lums du t�l�phone
        foreach (GameObject lum in Lumiere)
        {
            lum.SetActive(false);
        }

        //On fait joueur l'animation du t�l�phone apres 25s
        Invoke("JouerAnimationTelephone", 25f);

        //et il ne peut pas ouvrir la lum du t�l�phone
        peutOuvrirLum = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur peut ouvrir la lum du t�l�phone et appuie sur F
        if (Input.GetKeyDown(KeyCode.F) && peutOuvrirLum)
        {
            //On change l'etat des lumieres
            LumiereAllumer = !LumiereAllumer;

            //Et on l'affecte aux lumieres
            foreach (GameObject lum in Lumiere)
            {
                lum.SetActive(LumiereAllumer);
            }
        }

        //Si le breaker est ouvert et que le joueur e encore de la sanity 
        if (breaker.breakerOuvert && !GererNiveauSanity.noSanity)
        {
            //On enleve la transparence du logo de la camera
            cameraLogo.GetComponent<SpriteRenderer>().color = Color.white;
            toucheCamera.GetComponent<SpriteRenderer>().color = Color.white;
            textTouche.color = Color.black;
        }
        else
        {
            //Sinon le logo est transparent
            cameraLogo.GetComponent<SpriteRenderer>().color = couleurTransparente;
            toucheCamera.GetComponent<SpriteRenderer>().color = couleurTransparente;
            textTouche.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }

    /******************** Fonction qui fait jouer l'animation du t�l�phone au d�but quand les lumieres se ferment ***********************/
    void JouerAnimationTelephone()
    {
        GetComponent<Animator>().SetTrigger("DebutJeux");

        //Il peut ouvrir les cams (si le breaker est ouvert) et il peut ouvrir la lum du t�l�phone
        peutOuvrirCams = true;
        peutOuvrirLum = true;
    }
}
