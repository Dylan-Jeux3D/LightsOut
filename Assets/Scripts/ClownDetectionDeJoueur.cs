using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ClownDetectionDeJoueur : MonoBehaviour
{
    public static bool JoueurMort;
    /*public LayerMask layerToIgnore;
    public Color couleurSphere;*/
    public GameObject Joueur;
    public Camera CamJoueur;
    public GameObject teteClown;
    public AudioClip sonJumpscare;
    public GameObject sonChase;
    //public GameObject leClown;

    public GameObject leTelephone;
    public GameObject sanity;

    Animator animator;
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        JoueurMort = false;
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit collision;

        //On calcule la distance entre le joueur (target) et le clown (origin) pour ainsi le mettre
        //Comme direction pour le RayCast sinon, le celui-ci pointe dans le vide
        Vector3 direction = (Joueur.transform.position + new Vector3(0f, -2f, 0f)) - transform.position;

        /*if (Physics.SphereCast(transform.position, 10f, transform.forward, out collision, layerToIgnore))
        {
            print(collision.collider.gameObject.name);
        }*/
        
        if (Physics.Raycast(transform.position + new Vector3(0f, 2f, 0f), direction, out collision, 10f))
        {
            if (collision.collider.name == "Joueur" && !JoueurMort)
            {
                nav.SetDestination(Joueur.transform.position);
                nav.speed = 2.5f;
                animator.SetBool("chase", true);
                sonChase.SetActive(true);
            }
        }
        Debug.DrawRay(transform.position + new Vector3(0f, 2f, 0f), direction,Color.blue);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Joueur")
        {
            //On joue l'animation de Jumpscare
            animator.SetTrigger("Jumpscare");

            //On le fait arreter de pourchasser le joueur
            animator.SetBool("chase", false);

            //On desactive son NavMeshAgent pour pas qui bouge
            nav.enabled = false;

            //On tue le joueur
            JoueurMort = true;

            //On enleve la graviter au joueur (pour qu'il puisse floter dans les airs)
            Joueur.GetComponent<Rigidbody>().useGravity = false;


            /*********Ajustement pour cadrer l'aniation avec la camera (mes animations sont vraiment bizarre)*****************/

            //On le grossie
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            //On modifie un peu son localScale pour qu'il puisse etre centre avec le joueur et plus proche
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 2f, transform.localPosition.z);

            //On bouge la cam au transform du clown (dans l'animation, le transform du clown est au niveau de son visage)
            CamJoueur.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

            //On oriente la cam vers un point alligné avec la tete du clown (que j'ai créé moi-même)
            CamJoueur.transform.LookAt(teteClown.transform.position);

            //On desactive le son de Chase
            sonChase.SetActive(false);


            //Et finalement, on fait jouer le son du jumpscare
            GetComponent<AudioSource>().PlayOneShot(sonJumpscare);

            //On enleve les element qui sont dans la vu du joueur
            leTelephone.SetActive(false);
            sanity.SetActive(false);

            //On appelle la fonction pour changer de scene
            Invoke("allerVersMenuFin", 2f);
        }
    }

    void allerVersMenuFin()
    {
        //On affiche le menu de fin
        SceneManager.LoadScene("LightsOutMort");
    }
}
