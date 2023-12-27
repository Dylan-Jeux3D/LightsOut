using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ClownDetectionDeJoueur : MonoBehaviour
{
    public static bool JoueurMort; //Bool static qui indique au jeu si le joueur est mort
    public GameObject Joueur; //Le joueur
    public Camera CamJoueur; //La main cam
    public GameObject teteClown; //La tete du clown
    public AudioClip sonJumpscare; //Le son de jumpscare
    public GameObject sonChase; //Le son de poursuite

    public GameObject leTelephone; //Le téléphone
    public GameObject[] lesMenus; //Tableau des menu a fermer
    public GameObject curseur; //Le curseur
    bool chase; //bool qui indique si le joueur se fait poursuivre par le monstre

    Animator animator; //L'animator
    NavMeshAgent nav; //Le navMeshAgent
    // Start is called before the first frame update
    void Start()
    {
        //A udebut le joueur n'est pas mort
        JoueurMort = false;

        //On affecte les variables a leurs components
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
        
        //Si le raycast du monstre (qui pointe vers le joueur, touche le joueur)
        if (Physics.Raycast(transform.position + new Vector3(0f, 2f, 0f), direction, out collision, 10f))
        {
            if (collision.collider.name == "Joueur" && !JoueurMort)
            {
                //Le monstre chase le joueur
                chase = true;
            }
        }

        //Si le monstre chase et que le joueur est encore en vie
        if (chase && !JoueurMort)
        {
            //On set sa destination a la position du joueur
            nav.SetDestination(Joueur.transform.position);

            //On augmente sa vitesse
            nav.speed = 2.5f;

            //On active l'animation de chase (meme que marche mais plus vite)
            animator.SetBool("chase", true);

            //On active le son du poursuite
            sonChase.SetActive(true);
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
            Joueur.GetComponent<controleCams>().camsActives = false;
            Joueur.GetComponent<AudioSource>().Stop();
            leTelephone.SetActive(false);
            curseur.SetActive(false);

            foreach(GameObject menu in lesMenus)
            {
                menu.SetActive(false);
            }

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
