using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ClownEnnemieNavigation : MonoBehaviour
{
    public GameObject[] lesPointsDeNav; //Les gameobjects representant les points vers lesquels le monstre peut se diriger
    public int idPositionNav; //Le id du points nav actif
    public GameObject sonMarche; //Le son que fait le monstre quand il marche
    bool estAuPointNav; //Bool qui indique si le monstre est au point de nav
    NavMeshAgent nav; //Le navmeshAgent du monstre
    Animator animator; //L'animator du monstre
    // Start is called before the first frame update
    void Start()
    {
        //On assigne les variable a leur component
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //Le monstre commence au point 0
        idPositionNav = 0;

        //Et on commence a le faire marcher
        Invoke("AllerVersProchainPointNav", 0f);
        InvokeRepeating("verifierSiLeClownEstAuPointNav", 3f, 3f);
        sonMarche.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Si sa vitesse et nulle (ou presque)
        if (nav.velocity.magnitude <= 0.01f)
        {
            //Il s'arrete
            animator.SetBool("marche", false);
            sonMarche.SetActive(false);
            estAuPointNav = true;
        }
        //Sinon il continu son chemin
        else
        {
            animator.SetBool("marche", true);
            sonMarche.SetActive(true);
            estAuPointNav = false;
        }


    }

    /****************** Fonction qui verifie si le monstre est arriver a sa destination ******************************/
    void verifierSiLeClownEstAuPointNav()
    {
        foreach (GameObject navPoint in lesPointsDeNav)
        {
            //Si  sa vitesse est nulle est que sa position est la meme que le point nav
            if (navPoint.name == "PointNav" + idPositionNav && transform.position == navPoint.transform.position && estAuPointNav)
            {
                //On le dirige vers un nouveau point nav
                Invoke("AllerVersProchainPointNav", 3f);
            }
        }
    }

    /********************** Fonction qui donne un nouveau point nav ***************************/
    void AllerVersProchainPointNav()
    {
        /*Vielle méthode (linéaire)*/
        //idPositionNav++;

        /*if (idPositionNav > lesPointsDeNav.Length)
        {
            idPositionNav = 1;
        }*/

        /*Nouvelle méthode (Aléatoire)*/

        //On lui donne un point nav aléatoire dans le tableau des points nav
        idPositionNav = Random.Range(1, lesPointsDeNav.Length);

        //Pour chaque point nav
        foreach (GameObject navPoint in lesPointsDeNav)
        {
            //Si le idpoint nav est dans le nom du pointnav
            if (navPoint.name == "PointNav" + idPositionNav)
            {
                //Le monstre se dirige vers ce point la
                nav.SetDestination(navPoint.transform.position);
            }
        }
    }
}
