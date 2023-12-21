using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControleJoueur : MonoBehaviour
{
    public float vitesseDeplacement;
    public float vitesseRotation;
    float forceFace;
    float forceCote;

    bool auSol;

    //float ajoutGravite;

    public GameObject cam;

    public float maxDistanceRaycast;
    bool porteOuverte;
    bool lumieresOuvertes;
    bool breakerOuvert;

    public GameObject breaker;

    public Image curseur;
    public Image cadena;

    [Header("Les Sons")]
    public AudioClip sonInteractionBreaker;
    public AudioClip sonOuvrirPorte;
    public AudioClip sonFermerPorte;
    public AudioClip sonInterupteurOuvrir;
    public AudioClip sonInterupteurFermer;
    public GameObject sonMarcheBois;
    public AudioClip sonClee;

    private CharacterController characterController;
    Vector3 vitesseActuel;
    public float gravite = -9.81f;
    AudioSource audioSource;

    [Header("Le Keypad")]
    public GameObject imageKeypad;
    public bool keypadOuvert;
    public string mdp; //Le mot de passe pour le keypad
    string nummdp1;
    string nummdp2;
    string nummdp3;
    string nummdp4;

    public TextMeshPro[] lesNumsCobinaison;
    public TextMeshPro nomDuJoueur;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        nomDuJoueur.text = gestionAvertissement.nomJoueur;

        cam.transform.localRotation = Quaternion.Euler(0, 0, cam.transform.rotation.z);

        //ajoutGravite = 200f;

        //porteOuverte = false;
        //lumieresOuvertes = false;
        breakerOuvert = breaker.GetComponent<breaker>().breakerOuvert;

        curseur.color = Color.grey;

        characterController = GetComponent<CharacterController>();

        audioSource = GetComponent<AudioSource>();
        nummdp1 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp2 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp3 = UnityEngine.Random.Range(0, 10).ToString();
        nummdp4 = UnityEngine.Random.Range(0, 10).ToString();

        mdp = nummdp1 + nummdp2 + nummdp3 + nummdp4;
        print(mdp);

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

    void FixedUpdate()
    {
        // GetComponent<Rigidbody>().AddRelativeForce(forceCote, 0f, forceFace, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<controleCams>().camsActives 
            && !ClownDetectionDeJoueur.JoueurMort 
            && !keypadOuvert 
            && !GetComponent<gestionMenuTache>().menuTacheOuvert)
        {
            forceFace = Input.GetAxisRaw("Vertical");

            forceCote = Input.GetAxisRaw("Horizontal");

            float rotationHorizontal = Input.GetAxis("Mouse X") * vitesseRotation;
            transform.Rotate(0f, rotationHorizontal, 0f);

            float rotationVertical = Input.GetAxis("Mouse Y") * vitesseRotation;
            cam.transform.Rotate(-rotationVertical, 0f, 0f);
        }
        else
        {
            forceFace = 0f;
            forceCote = 0f;
        }

        Vector3 mouvement = transform.right * forceCote + transform.forward * forceFace;

        characterController.Move(mouvement.normalized * Time.deltaTime * vitesseDeplacement);

        

        vitesseActuel.y += gravite * Time.deltaTime;
        characterController.Move(vitesseActuel * Time.deltaTime);

        

        // print(cam.transform.localEulerAngles.x);

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


        /*********************************Est ce que le joueur touche le sol ?*************************************/
        RaycastHit infoCollision;

        auSol = Physics.Raycast(transform.position, Vector3.down, out infoCollision, 1f);
        //auSol = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out infoCollision, 1f);

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


        /*************************Section pour les differentes interactions*************************/

        RaycastHit collision;


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out collision, maxDistanceRaycast))
        {
            if (collision.collider.tag == "PorteDebarre" && Input.GetKeyDown(KeyCode.E))
            {
                porteOuverte = !porteOuverte;
                collision.collider.gameObject.GetComponent<Animator>().SetBool("PorteOuverte", porteOuverte);
                
                if(!porteOuverte)
                {
                    audioSource.PlayOneShot(sonFermerPorte, 1f);
                }
                else
                {
                    audioSource.PlayOneShot(sonOuvrirPorte, 1f);
                }
            }
            if (collision.collider.tag == "PorteBarre")
            {
                curseur.enabled = false;
                cadena.enabled = true;
            }
            else
            {
                curseur.enabled = true;
                cadena.enabled = false;
            }
            if ((collision.collider.tag == "Interupteur" || collision.collider.tag == "InterupteurDebut") 
                && Input.GetKeyDown(KeyCode.E) 
                && breakerOuvert 
                && !GererNiveauSanity.noSanity)
            {
                if (lumieresOuvertes)
                {
                    audioSource.PlayOneShot(sonInterupteurFermer, 1f);
                }
                else
                {
                    audioSource.PlayOneShot(sonInterupteurOuvrir, 1f);
                }
                lumieresOuvertes = !lumieresOuvertes;
                collision.collider.gameObject.GetComponent<Animator>().SetBool("LumieresOuvertes", lumieresOuvertes);
            }

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

            if (collision.collider.tag == "Clee" && Input.GetKey(KeyCode.E))
            {
                collision.collider.gameObject.GetComponent<GestionPorteEtClee>().PeutOuvrirLaPorte = true;
                collision.collider.tag = "Untagged";
                audioSource.PlayOneShot(sonClee, 2f);
                GetComponent<gestionMenuTache>().invokerVerificationTache();
            }

            if (collision.collider.tag == "Keypad" && Input.GetKey(KeyCode.E) && breakerOuvert && !GererNiveauSanity.noSanity)
            {
                imageKeypad.SetActive(true);
                keypadOuvert = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if(collision.collider.gameObject.layer == 6)
            {
                curseur.color = Color.white;
            }
            else
            {
                curseur.color = Color.grey;
            }
        }
        else
        {
            cadena.enabled = false;
            curseur.enabled = true;
            curseur.color = Color.grey;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistanceRaycast, Color.white);

        /*********************************************************************************************************/
    }
}
