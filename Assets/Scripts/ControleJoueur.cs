using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Les Sons")]
    public AudioClip sonInteractionBreaker;
    public AudioClip sonOuvrirPorte;
    public AudioClip sonFermerPorte;
    public AudioClip sonInterupteurOuvrir;
    public AudioClip sonInterupteurFermer;
    public GameObject sonMarcheBois;

    private CharacterController characterController;
    Vector3 vitesseActuel;
    public float gravite = -9.81f;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam.transform.localRotation = Quaternion.Euler(0, 0, cam.transform.rotation.z);

        //ajoutGravite = 200f;

        porteOuverte = false;
        lumieresOuvertes = false;
        breakerOuvert = breaker.GetComponent<breaker>().breakerOuvert;

        curseur.color = Color.grey;

        characterController = GetComponent<CharacterController>();

        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // GetComponent<Rigidbody>().AddRelativeForce(forceCote, 0f, forceFace, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<controleCams>().camsActives)
        {
            forceFace = Input.GetAxisRaw("Vertical");

            forceCote = Input.GetAxisRaw("Horizontal");

            float rotationHorizontal = Input.GetAxis("Mouse X") * vitesseRotation;
            transform.Rotate(0f, rotationHorizontal, 0f);

            float rotationVertical = Input.GetAxis("Mouse Y") * vitesseRotation;
            cam.transform.Rotate(-rotationVertical, 0f, 0f);
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
            if (collision.collider.tag == "Porte" && Input.GetKeyDown(KeyCode.E))
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

            if ((collision.collider.tag == "Interupteur" || collision.collider.tag == "InterupteurDebut") && Input.GetKeyDown(KeyCode.E) && breakerOuvert)
            {
                lumieresOuvertes = !lumieresOuvertes;
                collision.collider.gameObject.GetComponent<Animator>().SetBool("LumieresOuvertes", lumieresOuvertes);

                if(lumieresOuvertes)
                {
                    audioSource.PlayOneShot(sonInterupteurFermer, 1f);
                }
                else
                {
                    audioSource.PlayOneShot(sonInterupteurOuvrir, 1f);
                }
            }

            if (collision.collider.tag == "Breaker" && Input.GetKeyDown(KeyCode.E))
            {
                if (!breakerOuvert)
                {
                    collision.collider.gameObject.GetComponent<breaker>().AllumerBreaker();
                    
                }
                else
                {
                    collision.collider.gameObject.GetComponent<breaker>().FermerBreaker();
                }
                breakerOuvert = !breakerOuvert;
                audioSource.PlayOneShot(sonInteractionBreaker, 1f);
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
            curseur.color = Color.grey;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistanceRaycast, Color.white);

        /*********************************************************************************************************/
    }




    /*private void OnCollisionEnter(Collision infoCollision)
    {
        if (infoCollision.gameObject.tag == "Escalier")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }*/
}
