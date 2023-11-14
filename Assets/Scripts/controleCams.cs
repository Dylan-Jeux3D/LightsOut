using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controleCams : MonoBehaviour
{
    public GameObject telephone; //Le telephone du joueur
    public bool camsActives; //Est ce que les cams de securite

    [Header("Les Cams")]
    public Camera mainCam; // La camera du joueur
    public Camera[] lesCamsDeSurveillances; // Les cameras de surveillance

    int numeroCamActif; //Int qui represente le ID de la cam de securite qui est active
    public Image curseur; //Le curseur pour pouvoir le desactiver


    // Start is called before the first frame update
    void Start()
    {
        //Au debut les cams sont fermer et le ID de la cam par default est 1;
        camsActives = false;
        numeroCamActif = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur peut ouvrir les cams et il appuie sur la touche espace
        if (telephone.GetComponent<ControleTelephone>().peutOuvrirCams && Input.GetKeyDown(KeyCode.Space))
        {
            //On active ou desactive les cams
            camsActives = !camsActives;

            //On joue l'animation du telephone
            telephone.GetComponent<Animator>().SetBool("CamsOuvertes", camsActives);

            //Si les cams sont actives
            if (camsActives)
            {
                //On les active
                Invoke("OuvrirFermerLesCams", 0.6f);

                //Et on desactive le curseur
                Invoke("activerDesactiverCurseur", 0.6f);
            }
        }

        //Si les cams sont active
        if (camsActives)
        {
            //Et que le joueur appuie sur la fleche de gauche
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //On change le ID de la cam active
                numeroCamActif -= 1;
                Invoke("changerCamSelectionne", 0.1f);
            }
            //ET que le joueur appuie sur la fleche de droite
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //On change le ID de la cam active
                numeroCamActif += 1;
                Invoke("changerCamSelectionne", 0.1f);
            }

            //On bloque le ID a 1 car il n'y a pas de cam en bas de 0
            if (numeroCamActif < 1)
            {
                numeroCamActif = 1;
            }
            //Et on ne veut pas que le ID depasse le nb de cam
            else if (numeroCamActif > lesCamsDeSurveillances.Length)
            {
                numeroCamActif = lesCamsDeSurveillances.Length;
            }
        }
        // Sinon ou ferme le cam
        else
        {
            //On ferme tous les cams
            foreach (Camera cam in lesCamsDeSurveillances)
            {
                cam.gameObject.SetActive(false);
            }

            /*************Remarque*****************/
            //On ne desactive jamais la main cam
            //Parce que sinon, les animations du telephone (qui est attacher a la main cam)
            //ne sont pas sauvegarder
            mainCam.GetComponent<Camera>().enabled = true;
            mainCam.GetComponent<AudioListener>().enabled = true;

            //Et on reactive le curseur
            Invoke("activerDesactiverCurseur", 0f);
        }
        
    }

    void OuvrirFermerLesCams()
    {
        mainCam.GetComponent<Camera>().enabled = false;
        mainCam.GetComponent<AudioListener>().enabled = false;
        //camCuisine.gameObject.SetActive(camsActives);

        foreach (Camera cam in lesCamsDeSurveillances)
        {
            if (cam.gameObject.name == "Camera" + numeroCamActif + "")
            {
                cam.gameObject.SetActive(true);
            }
            else
            {
                cam.gameObject.SetActive(false);
            }
        }
    }

    void changerCamSelectionne()
    {
        foreach (Camera cam in lesCamsDeSurveillances)
        {
            if (cam.gameObject.name == "Camera" + numeroCamActif + "")
            {
                cam.gameObject.SetActive(true);
            }
            else
            {
                cam.gameObject.SetActive(false);
            }
        }
    }

    void activerDesactiverCurseur()
    {
        //On desactive le curseur
        curseur.gameObject.SetActive(!camsActives);
    }
}
