using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ClownEnnemieNavigation : MonoBehaviour
{
    public GameObject[] lesPointsDeNav;
    public int idPositionNav;
    public GameObject sonMarche;
    bool estAuPointNav;
    NavMeshAgent nav;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        idPositionNav = 0;
        Invoke("AllerVersProchainPointNav", 0f);
        InvokeRepeating("verifierSiLeClownEstAuPointNav", 3f, 3f);
        sonMarche.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.velocity.magnitude <= 0.01f)
        {
            animator.SetBool("marche", false);
            sonMarche.SetActive(false);
            estAuPointNav = true;
        }
        else
        {
            animator.SetBool("marche", true);
            sonMarche.SetActive(true);
            estAuPointNav = false;
        }


    }

    void verifierSiLeClownEstAuPointNav()
    {
        foreach (GameObject navPoint in lesPointsDeNav)
        {
            if (navPoint.name == "PointNav" + idPositionNav && transform.position == navPoint.transform.position && estAuPointNav)
            {
                Invoke("AllerVersProchainPointNav", 3f);
            }
        }
    }

    void AllerVersProchainPointNav()
    {
        idPositionNav++;

        if (idPositionNav > lesPointsDeNav.Length)
        {
            idPositionNav = 1;
        }

        foreach (GameObject navPoint in lesPointsDeNav)
        {
            if (navPoint.name == "PointNav" + idPositionNav)
            {
                nav.SetDestination(navPoint.transform.position);
            }
        }
    }
}
