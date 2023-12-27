using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControleJoueur : MonoBehaviour
{
    public float vitesseDeplacement; //la vitesse de deplacement
    public float vitesseRotation; //la vitesse de rotation de la cam
    float forceFace; //Force de deplacement de face
    float forceCote; //Force de deplacement de coté
    bool auSol; //bool indiquant si le joueur est au sol
    public GameObject cam; //la main camera
    public float maxDistanceRaycast; //La distance maximale du raycast
    public bool breakerOuvert; //bool qui dit si le breaker est ouvert ou non
    public Image curseur; //Image du curseur dans le UI
    public Image cadena;//Image du cadena dans le UI

    /* les variables pour les sons */
    [Header("Les Sons")]
    public AudioClip sonInteractionBreaker;
    public AudioClip sonOuvrirPorte;
    public AudioClip sonFermerPorte;
    public AudioClip sonInterupteurOuvrir;
    public AudioClip sonInterupteurFermer;
    public GameObject sonMarcheBois;
    public AudioClip sonClee;

    private CharacterController characterController; //le character controller du joueur
    Vector3 vitesseActuel; // la vitesse actuel du joueur
    public float gravite = -9.81f; //ajout de gravité pour que le joueur reste coller au plancher
    AudioSource audioSource; //l'audiosource

    /* Les variables en lien avec le keypad */
    [Header("Le Keypad")]
    public GameObject imageKeypad; //l'image du keypad
    public bool keypadOuvert; //bool qui determine si le keypad est ouvet
    public string mdp; //Le mot de passe pour le keypad

    /* Les numeros de la combinaison du mot de passe */
    string nummdp1;
    string nummdp2;
    string nummdp3;
    string nummdp4;

    public TextMeshPro[] lesNumsCobinaison; //Les boites de texte contenant les numeros a entrer
    public TextMeshPro nomDuJoueur; //Le nom du joueur

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //On barre le curseur (de windows) a la fenetre de jeu + on veut pas le voir

        nomDuJoueur.text = gestionAvertissement.nomJoueur; //On affecte le nom du joueur avec le nom qui a été entré au menu du debut

        cam.transform.localRotation = Quaternion.Euler(0, 0, cam.transform.rotation.z); //On met la rotation de la cam a 0

        breakerOuvert = breaker.breakerOuvert; //On affecte breakerOuvert a la valeur du script breaker

        curseur.color = Color.grey; //On met la couleur du curseur a gris

        //On affecte les components a leur variable
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        /* On genere une combinaison aléatoire pour le mot de passe*/
        nummdp1 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp2 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp3 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp4 = UnityEngine.Random.Range(0, 10).ToString();

        mdp = nummdp1 + nummdp2 + nummdp3 + nummdp4;
        print(mdp);

        //On affecte les numeros a un texte in-game pour que le joueur puisse trouver la combinaison
        foreach (TextMeshPro unNum in lesNumsCobinaison)
        {
            if (unNum.gameObject.name == "num1")
            {
                unNum.text = nummdp1;
            }
            else if(unNum.gameObject.name == "num2")
            {
                unNum.text = nummdp2;
            }
            else if (unNum.gameObject.name == "num3")
            {
                unNum.text = nummdp3;
            }
            else if (unNum.gameObject.name == "num4")
            {
                unNum.text = nummdp4;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<controleCams>().camsActives 
            && !ClownDetectionDeJoueur.JoueurMort 
            && !keypadOuvert 
            && !GetComponent<gestionMenuTache>().menuTacheOuvert)
        {
            //On va chercher les GetAxis pour affecter les forces de mouvement du joueur
            forceFace = Input.GetAxisRaw("Vertical");

            forceCote = Input.GetAxisRaw("Horizontal");

            float rotationHorizontal = Input.GetAxis("Mouse X") * vitesseRotation;
            transform.Rotate(0f, rotationHorizontal, 0f);

            float rotationVertical = Input.GetAxis("Mouse Y") * vitesseRotation;
            cam.transform.Rotate(-rotationVertical, 0f, 0f);
        }
        //Si le joueur a un menu autre que le menu des taches d'ouvert, ou que le joueur soit mort
        else
        {
            //On le fige sur place en mettant les force de deplacement a 0
            forceFace = 0f;
            forceCote = 0f;
        }

        /******************** Mouvement du joueur fait avec l'aide du tuto youtube de Brackeys (https://youtu.be/_QajrabyTJc?si=S6qVAYJEduDYQWkN)****************************/
        Vector3 mouvement = transform.right * forceCote + transform.forward * forceFace;

        characterController.Move(mouvement.normalized * Time.deltaTime * vitesseDeplacement);

        vitesseActuel.y += gravite * Time.deltaTime;
        characterController.Move(vitesseActuel * Time.deltaTime);

        /****************************Section pour blocker la rotation de la cam****************************/
        //fonction pour blocker la rotation de la cam pour eviter qu'elle fasse des 360 (axe des x)
        //Concus avec l'aide du forum de Unity
        if (cam.transform.localEulerAngles.x < 300 && cam.transform.localEulerAngles.x > 290)
        {
            cam.transform.localRotation = Quaternion.Euler(-60, cam.transform.rotation.y, cam.transform.rotation.z);
        }

        if (cam.transform.localEulerAngles.x > 70 && cam.transform.localEulerAngles.x < 80)
        {
            cam.transform.localRotation = Quaternion.Euler(70, cam.transform.rotation.y, cam.transform.rotation.z);
        }
        /*********************************************************************************************************/


        /********************************* Est ce que le joueur touche le sol ? *************************************/
        RaycastHit infoCollision;

        auSol = Physics.Raycast(transform.position, Vector3.down, out infoCollision, 1f);

        Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.red);
        if (auSol && vitesseActuel.y < 0f)
        {
            vitesseActuel.y = -2f;
        }

        if (auSol && mouvement.magnitude > 0.5f)
        {
            sonMarcheBois.SetActive(true);
        }
        else
        {
            sonMarcheBois.SetActive(false);
        }


        /************************* Section pour les differentes interactions *************************/

        RaycastHit collision;


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out collision, maxDistanceRaycast))
        {
            /********* Interaction avec les portes **********/
            if (collision.collider.tag == "PorteDebarre" && Input.GetKeyDown(KeyCode.E))
            {
                collision.collider.gameObject.GetComponent<controlePortes>().porteOuverte = !collision.collider.gameObject.GetComponent<controlePortes>().porteOuverte;
                
                if(!collision.collider.gameObject.GetComponent<controlePortes>().porteOuverte)
                {
                    audioSource.PlayOneShot(sonFermerPorte, 1f);
                }
                else
                {
                    audioSource.PlayOneShot(sonOuvrirPorte, 1f);
                }
            }
            //Si la porte est barré
            if (collision.collider.tag == "PorteBarre")
            {
                //On montre l'icone de cadena
                curseur.enabled = false;
                cadena.enabled = true;
            }
            //Sinon
            else
            {
                //On desactive le cadena et on active le curseur
                curseur.enabled = true;
                cadena.enabled = false;
            }

            /********* Interaction avec les interupteurs **********/
            if ((collision.collider.tag == "Interupteur" || collision.collider.tag == "InterupteurDebut") 
                && Input.GetKeyDown(KeyCode.E) 
                && breakerOuvert 
                && !GererNiveauSanity.noSanity)
            {
                if (collision.collider.gameObject.GetComponent<Interupteur>().lumOuverte)
                {
                    audioSource.PlayOneShot(sonInterupteurFermer, 1f);
                }
                else
                {
                    audioSource.PlayOneShot(sonInterupteurOuvrir, 1f);
                }
                collision.collider.gameObject.GetComponent<Interupteur>().lumOuverte = !collision.collider.gameObject.GetComponent<Interupteur>().lumOuverte;
                collision.collider.gameObject.GetComponent<Animator>().SetBool("LumieresOuvertes", collision.collider.gameObject.GetComponent<Interupteur>().lumOuverte);
            }

            /********* Interaction avec le breaker **********/
            if (collision.collider.tag == "Breaker" && Input.GetKeyDown(KeyCode.E) && !GererNiveauSanity.noSanity)
            {
                if (!breakerOuvert)
                {
                    collision.collider.gameObject.GetComponent<breaker>().AllumerBreaker();
                    GetComponent<gestionMenuTache>().invokerVerificationTache();

                }
                else
                {
                    collision.collider.gameObject.GetComponent<breaker>().FermerBreaker();
                }
                breakerOuvert = !breakerOuvert;
                audioSource.PlayOneShot(sonInteractionBreaker, 3f);
            }

            /********* Interaction avec les clées **********/
            if (collision.collider.tag == "Clee" && Input.GetKey(KeyCode.E))
            {
                collision.collider.gameObject.GetComponent<GestionPorteEtClee>().PeutOuvrirLaPorte = true;
                collision.collider.tag = "Untagged";
                audioSource.PlayOneShot(sonClee, 2f);

                //On vérifie si la tache en cours a été complété
                GetComponent<gestionMenuTache>().invokerVerificationTache();
            }

            /********* Interaction avec le keypad **********/
            if (collision.collider.tag == "Keypad" && Input.GetKey(KeyCode.E) && !GererNiveauSanity.noSanity)
            {
                imageKeypad.SetActive(true);
                keypadOuvert = true;
                Cursor.lockState = CursorLockMode.None;
            }

            //Si le joueur pointe vers un obect interactif
            if(collision.collider.gameObject.layer == 6)
            {
                //On change la couleur du curseur a blanc
                curseur.color = Color.white;
            }
            else
            {
                //Sinon il redevient gris
                curseur.color = Color.grey;
            }
        }
        //Si le raycast du joueur ne touche rien
        else
        {
            //On desactive le cadena pour les portes
            cadena.enabled = false;

            //On active le curseur normale et on met sa couleur a gris
            curseur.enabled = true;
            curseur.color = Color.grey;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistanceRaycast, Color.white);

        /*********************************************************************************************************/
    }
}
