using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class controleCams : MonoBehaviour
{
    public GameObject telephone; //Le telephone du joueur
    public bool camsActives; //Est ce que les cams de securite
    public GameObject lebreaker;
    public GameObject nomDuJoueur;

    [Header("Les Cams")]
    public Camera mainCam; // La camera du joueur
    public Camera[] lesCamsDeSurveillances; // Les cameras de surveillance

    int numeroCamActif; //Int qui represente le ID de la cam de securite qui est active
    public Image curseur; //Le curseur pour pouvoir le desactiver

    public GameObject volumeCams;
    public GameObject camsUI;
    public TextMeshProUGUI numCamsUI;


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
        if (telephone.GetComponent<ControleTelephone>().peutOuvrirCams 
            && Input.GetKeyDown(KeyCode.Space) 
            && lebreaker.GetComponent<breaker>().breakerOuvert 
            && !GererNiveauSanity.noSanity)
        {
            //On active ou desactive les cams
            camsActives = !camsActives;

            //On joue l'animation du telephone
            telephone.GetComponent<Animator>().SetBool("CamsOuvertes", camsActives);

            //Si les cams sont actives
            if (camsActives)
            {
                //On les active
                Invoke("OuvrirLesCams", 0.6f);

                //Et on desactive le curseur
                Invoke("activerDesactiverCurseur", 0.6f);
            }
        }

        //Si les cams sont active
        if (camsActives)
        {
            //Et que le joueur appuie sur la fleche de gauche
            if (Input.GetKeyDown(KeyCode.A))
            {
                //On change le ID de la cam active
                numeroCamActif -= 1;
                Invoke("changerCamSelectionne", 0.1f);
            }
            //ET que le joueur appuie sur la fleche de droite
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //On change le ID de la cam active
                numeroCamActif += 1;
                Invoke("changerCamSelectionne", 0.1f);
            }

            //On bloque le ID a 1 car il n'y a pas de cam en bas de 0
            if (numeroCamActif < 1)
            {
                numeroCamActif = lesCamsDeSurveillances.Length;
            }
            //Et on ne veut pas que le ID depasse le nb de cam
            else if (numeroCamActif > lesCamsDeSurveillances.Length)
            {
                numeroCamActif = 1;
            }

            gameObject.transform.position = gameObject.transform.position;
        }
        // Sinon ou ferme le cam
        else
        {
            //On ferme tous les cams
            foreach (Camera cam in lesCamsDeSurveillances)
            {
                cam.gameObject.SetActive(false);
            }

            nomDuJoueur.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

            /*************Remarque*****************/
            //On ne desactive jamais la main cam
            //Parce que sinon, les animations du telephone (qui est attacher a la main cam)
            //ne sont pas sauvegarder
            mainCam.GetComponent<Camera>().enabled = true;
            mainCam.GetComponent<AudioListener>().enabled = true;
            volumeCams.SetActive(false);
            camsUI.SetActive(false);

            //Et on reactive le curseur
            Invoke("activerDesactiverCurseur", 0f);
        }

        //On oriente le nom du jouer vers la cam qui est active 
        foreach(Camera camera in lesCamsDeSurveillances)
        {
            if (camera.gameObject.activeSelf)
            {
                nomDuJoueur.transform.LookAt(camera.transform.position);
            }
        }

        numCamsUI.text = numeroCamActif.ToString() + " / " + lesCamsDeSurveillances.Length;
        
    }

    void OuvrirLesCams()
    {
        mainCam.GetComponent<Camera>().enabled = false;
        mainCam.GetComponent<AudioListener>().enabled = false;
        //camCuisine.gameObject.SetActive(camsActives);

        volumeCams.SetActive(true);
        camsUI.SetActive(true);

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
